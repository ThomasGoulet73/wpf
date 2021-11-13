using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    internal unsafe delegate void* CreateTextAnalysisSink();

    internal unsafe delegate void* GetScriptAnalysisList(void* textAnalysisSink);

    internal unsafe delegate void* GetNumberSubstitutionList(void* textAnalysisSink);

    internal unsafe delegate int CreateTextAnalysisSource(char* text,
                                                  uint length,
                                                  char* culture,
                                                  void* factory,
                                                  bool isRightToLeft,
                                                  char* numberCulture,
                                                  bool ignoreUserOverride,
                                                  uint numberSubstitutionMethod,
                                                  void** ppTextAnalysisSource);

    internal unsafe class TextAnalyzer
    {
        internal const char CharHyphen = '\x002d';
        private const long ERROR_INSUFFICIENT_BUFFER = 122;
        private const int E_INVALIDARG = unchecked((int)0x80070057L);
        private const int FACILITY_WIN32 = 7;

        private readonly NativeIUnknownWrapper<IDWriteTextAnalyzer> _textAnalyzer;

        internal TextAnalyzer(IDWriteTextAnalyzer* nativePointer)
        {
            _textAnalyzer = new NativeIUnknownWrapper<IDWriteTextAnalyzer>(nativePointer);
        }

        internal static IList<Span> Itemize(char* text, uint length, CultureInfo culture, Factory factory, bool isRightToLeftParagraph, CultureInfo numberCulture, bool ignoreUserOverride, uint numberSubstitutionMethod, ClassificationUtility classificationUtility, CreateTextAnalysisSink pfnCreateTextAnalysisSink, GetScriptAnalysisList pfnGetScriptAnalysisList, GetNumberSubstitutionList pfnGetNumberSubstitutionList, CreateTextAnalysisSource pfnCreateTextAnalysisSource)
        {
            IDWriteTextAnalyzer* pTextAnalyzer = null;
            IDWriteTextAnalysisSink* pTextAnalysisSink = null;
            IDWriteTextAnalysisSource* pTextAnalysisSource = null;

            IDWriteFactory* pDWriteFactory = factory.DWriteFactoryAddRef;

            int hr;
            try
            {
                //pDWriteFactory->AddReference();
                hr = pDWriteFactory->CreateTextAnalyzer(&pTextAnalyzer);

                char* pNumberSubstitutionLocaleName = null;

                if (numberCulture != null)
                {
                    fixed (char* pNumberSubstitutionLocaleNamePinned = numberCulture.IetfLanguageTag)
                    {
                        pNumberSubstitutionLocaleName = pNumberSubstitutionLocaleNamePinned;
                    }
                }

                fixed (char* pCultureName = culture.IetfLanguageTag)
                {
                    hr = pfnCreateTextAnalysisSource(text, length, pCultureName, (void*)(pDWriteFactory), isRightToLeftParagraph, pNumberSubstitutionLocaleName, ignoreUserOverride, numberSubstitutionMethod, (void**)&pTextAnalysisSource);
                    Marshal.ThrowExceptionForHR(hr);

                    pTextAnalysisSink = (IDWriteTextAnalysisSink*)pfnCreateTextAnalysisSink();

                    hr = pTextAnalyzer->AnalyzeScript(pTextAnalysisSource, 0, length, pTextAnalysisSink);
                    Marshal.ThrowExceptionForHR(hr);

                    hr = pTextAnalyzer->AnalyzeNumberSubstitution(pTextAnalysisSource, 0, length, pTextAnalysisSink);
                    Marshal.ThrowExceptionForHR(hr);

                    DWriteTextAnalysisNode<DWRITE_SCRIPT_ANALYSIS>* dwriteScriptAnalysisNode = (DWriteTextAnalysisNode<DWRITE_SCRIPT_ANALYSIS>*)pfnGetScriptAnalysisList(pTextAnalysisSink);
                    DWriteTextAnalysisNode<IDWriteNumberSubstitution>* dwriteNumberSubstitutionNode = (DWriteTextAnalysisNode<IDWriteNumberSubstitution>*)pfnGetNumberSubstitutionList(pTextAnalysisSink);

                    TextItemizer textItemizer = new TextItemizer(dwriteScriptAnalysisNode, dwriteNumberSubstitutionNode);

                    return AnalyzeExtendedAndItemize(textItemizer, text, length, numberCulture, classificationUtility);
                }
            }
            finally
            {
                ReleaseItemizationNativeResources(&pDWriteFactory, &pTextAnalyzer, &pTextAnalysisSource, &pTextAnalysisSink);
            }
        }

        internal void GetGlyphsAndTheirPlacements(
                char* textString,
                uint textLength,
                Font font,
                ushort blankGlyphIndex,
                bool isSideways,
                bool isRightToLeft,
                CultureInfo cultureInfo,
                DWriteFontFeature[][] features,
                uint[] featureRangeLengths,
                double fontEmSize,
                double scalingFactor,
                float pixelsPerDip,
                TextFormattingMode textFormattingMode,
                ItemProps itemProps,
                out ushort[] clusterMap,
                out ushort[] glyphIndices,
                out int[] glyphAdvances,
                out GlyphOffset[] glyphOffsets)
        {
            throw new NotImplementedException();
        }

        internal void GetGlyphs(char* textString, uint textLength, Font font, ushort blankGlyphIndex, bool isSideways, bool isRightToLeft, CultureInfo cultureInfo, DWriteFontFeature[][] features, uint[] featureRangeLengths, uint maxGlyphCount, TextFormattingMode textFormattingMode, ItemProps itemProps, ushort* clusterMap, ushort* textProps, ushort* glyphIndices, uint* glyphProps, int* pfCanGlyphAlone, out uint actualGlyphCount)
        {
            // Shaping should not have taken place if ScriptAnalysis is NULL!
            Invariant.Assert(itemProps.ScriptAnalysis != null);

            // These are control characters and WPF will not display control characters.
            if (GetScriptShapes(itemProps) != DWRITE_SCRIPT_SHAPES.DWRITE_SCRIPT_SHAPES_DEFAULT)
            {
                FontFace fontFace = font.GetFontFace();
                try
                {
                    GetBlankGlyphsForControlCharacters(
                        textString,
                        textLength,
                        fontFace,
                        blankGlyphIndex,
                        maxGlyphCount,
                        clusterMap,
                        glyphIndices,
                        pfCanGlyphAlone,
                        out actualGlyphCount
                        );
                }
                finally
                {
                    fontFace.Release();
                }
            }
            else
            {
                string localeName = cultureInfo.IetfLanguageTag;

                uint featureRanges = 0;
                GCHandle[] dwriteFontFeaturesGCHandles = null;
                DWRITE_TYPOGRAPHIC_FEATURES*[] dwriteTypographicFeatures = null;


                if (features != null)
                {
                    featureRanges = (uint)featureRangeLengths.Length;
                    dwriteFontFeaturesGCHandles = new GCHandle[featureRanges];
                    dwriteTypographicFeatures = new DWRITE_TYPOGRAPHIC_FEATURES*[featureRanges];
                }

                FontFace fontFace = font.GetFontFace();
                try
                {
                    if (features != null)
                    {
                        for (uint i = 0; i < featureRanges; ++i)
                        {
                            dwriteFontFeaturesGCHandles[i] = GCHandle.Alloc(features[i], GCHandleType.Pinned);
                            var s = new DWRITE_TYPOGRAPHIC_FEATURES()
                            {
                                features = (DWRITE_FONT_FEATURE*)dwriteFontFeaturesGCHandles[i].AddrOfPinnedObject().ToPointer(),
                                featureCount = (uint)features[i].Length
                            };
                            dwriteTypographicFeatures[i] = &s;
                        }
                    }


                    uint glyphCount = 0;

                    fixed (char* pLocaleName = localeName)
                    fixed (uint* pFeatureRangeLengthsPinned = featureRangeLengths)
                    fixed (DWRITE_TYPOGRAPHIC_FEATURES** dwriteTypographicFeaturesPinned = dwriteTypographicFeatures)
                    {
                        int hr = _textAnalyzer.Value->GetGlyphs(
                            (ushort*)textString,
                            /*checked*/((uint)textLength),
                            fontFace.DWriteFontFaceNoAddRef,
                            isSideways ? 1 : 0,
                            isRightToLeft ? 1 : 0,
                            (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                            (ushort*)pLocaleName,
                            (IDWriteNumberSubstitution*)(itemProps.NumberSubstitutionNoAddRef),
                            dwriteTypographicFeaturesPinned,
                            pFeatureRangeLengthsPinned,
                            featureRanges,
                            /*checked*/((uint)maxGlyphCount),
                            clusterMap,
                            (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps, //The size of DWRITE_SHAPING_TEXT_PROPERTIES is 16 bits which is the same size that LS passes to WPF 
                                                                        //Thus we can safely cast textProps to DWRITE_SHAPING_TEXT_PROPERTIES*
                            glyphIndices,
                            (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps, //The size of DWRITE_SHAPING_GLYPH_PROPERTIES is 16 bits. LS passes a pointer to UINT32 so this cast is safe since 
                                                                          //We will not write into memory outside the boundary. But this is cast will result in an unused space. We are taking this
                                                                          //Approach for now to avoid modifying LS code.
                            &glyphCount
                            );

                        if (hr == E_INVALIDARG)
                        {
                            // If pLocaleName is unsupported (e.g. "prs-af"), DWrite returns E_INVALIDARG.
                            // Try again with the default mapping.
                            hr = _textAnalyzer.Value->GetGlyphs(
                                (ushort*)textString,
                                textLength,
                                fontFace.DWriteFontFaceNoAddRef,
                                isSideways ? 1 : 0,
                                isRightToLeft ? 1 : 0,
                                (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                                null /* default locale mapping */,
                                (IDWriteNumberSubstitution*)(itemProps.NumberSubstitutionNoAddRef),
                                dwriteTypographicFeaturesPinned,
                                pFeatureRangeLengthsPinned,
                                featureRanges,
                                /*checked*/((uint)maxGlyphCount),
                                clusterMap,
                                (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps, //The size of DWRITE_SHAPING_TEXT_PROPERTIES is 16 bits which is the same size that LS passes to WPF 
                                                                            //Thus we can safely cast textProps to DWRITE_SHAPING_TEXT_PROPERTIES*
                                glyphIndices,
                                (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps, //The size of DWRITE_SHAPING_GLYPH_PROPERTIES is 16 bits. LS passes a pointer to UINT32 so this cast is safe since 
                                                                              //We will not write into memory outside the boundary. But this is cast will result in an unused space. We are taking this
                                                                              //Approach for now to avoid modifying LS code.
                                &glyphCount
                                );
                        }

                        if (features != null)
                        {
                            for (uint i = 0; i < featureRanges; ++i)
                            {
                                dwriteFontFeaturesGCHandles[i].Free();
                            }
                        }

                        if (hr == HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER))
                        {
                            // Actual glyph count is not returned by DWrite unless the call tp GetGlyphs succeeds.
                            // It must be re-estimated in case the first estimate was not adequate.
                            // The following calculation is a refactoring of DWrite's logic ( 3*stringLength/2 + 16) after 3 retries.
                            // We'd rather go directly to the maximum buffer size we are willing to allocate than pay the cost of continuously retrying.
                            actualGlyphCount = 27 * maxGlyphCount / 8 + 76;
                        }
                        else
                        {
                            Marshal.ThrowExceptionForHR(hr);

                            if (pfCanGlyphAlone != null)
                            {
                                for (uint i = 0; i < textLength; ++i)
                                {
                                    pfCanGlyphAlone[i] = (((DWRITE_SHAPING_TEXT_PROPERTIES*)textProps)[i].isShapedAlone > 0) ? 1 : 0;
                                    ;
                                }
                            }

                            actualGlyphCount = glyphCount;
                        }
                    }
                }
                finally
                {
                    fontFace.Release();
                }
            }
        }

        internal void GetGlyphPlacements(char* textString, ushort* clusterMap, ushort* textProps, uint textLength, ushort* glyphIndices, uint* glyphProps, uint glyphCount, Font font, double fontEmSize, double scalingFactor, bool isSideways, bool isRightToLeft, CultureInfo cultureInfo, DWriteFontFeature[][] features, uint[] featureRangeLengths, TextFormattingMode textFormattingMode, ItemProps itemProps, float pixelsPerDip, int* glyphAdvances, out GlyphOffset[] glyphOffsets)
        {
            // Shaping should not have taken place if ScriptAnalysis is NULL!
            Invariant.Assert(itemProps.ScriptAnalysis != null);

            // These are control characters and WPF will not display control characters.
            if (GetScriptShapes(itemProps) != DWRITE_SCRIPT_SHAPES.DWRITE_SCRIPT_SHAPES_DEFAULT)
            {
                GetGlyphPlacementsForControlCharacters(
                    textString,
                    textLength,
                    font,
                    textFormattingMode,
                    fontEmSize,
                    scalingFactor,
                    isSideways,
                    pixelsPerDip,
                    glyphCount,
                    glyphIndices,
                    glyphAdvances,
                    out glyphOffsets
                    );
            }
            else
            {
                float[] dwriteGlyphAdvances = new float[glyphCount];
                DWRITE_GLYPH_OFFSET[] dwriteGlyphOffsets = new DWRITE_GLYPH_OFFSET[glyphCount];

                GCHandle[] dwriteFontFeaturesGCHandles = null;
                uint featureRanges = 0;
                DWRITE_TYPOGRAPHIC_FEATURES*[] dwriteTypographicFeatures = null;

                if (features != null)
                {
                    featureRanges = (uint)featureRangeLengths.Length;
                    dwriteFontFeaturesGCHandles = new GCHandle[featureRanges];
                    dwriteTypographicFeatures = new DWRITE_TYPOGRAPHIC_FEATURES*[featureRanges];
                }

                FontFace fontFace = font.GetFontFace();
                try
                {
                    String localeName = cultureInfo.IetfLanguageTag;
                    DWRITE_MATRIX transform = Factory.GetIdentityTransform();

                    if (features != null)
                    {
                        for (uint i = 0; i < featureRanges; ++i)
                        {
                            dwriteFontFeaturesGCHandles[i] = GCHandle.Alloc(features[i], GCHandleType.Pinned);
                            var s = new DWRITE_TYPOGRAPHIC_FEATURES()
                            {
                                features = (DWRITE_FONT_FEATURE*)dwriteFontFeaturesGCHandles[i].AddrOfPinnedObject().ToPointer(),
                                featureCount = (uint)features[i].Length
                            };
                            dwriteTypographicFeatures[i] = &s;
                        }
                    }

                    float fontEmSizeFloat = (float)fontEmSize;

                    fixed (char* pLocaleName = localeName)
                    fixed (uint* pFeatureRangeLengthsPinned = featureRangeLengths)
                    fixed (DWRITE_TYPOGRAPHIC_FEATURES** dwriteTypographicFeaturesPinned = dwriteTypographicFeatures)
                    fixed (float* dwriteGlyphAdvancesPinned = dwriteGlyphAdvances)
                    fixed (DWRITE_GLYPH_OFFSET* dwriteGlyphOffsetsPinned = dwriteGlyphOffsets)
                    {
                        int hr;

                        if (textFormattingMode == TextFormattingMode.Ideal)
                        {
                            hr = _textAnalyzer.Value->GetGlyphPlacements(
                                (ushort*)textString,
                                clusterMap,
                                (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps,
                                textLength,
                                glyphIndices,
                                (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps,
                                glyphCount,
                                fontFace.DWriteFontFaceNoAddRef,
                                fontEmSizeFloat,
                                isSideways ? 1 : 0,
                                isRightToLeft ? 1 : 0,
                                (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                                (ushort*)pLocaleName,
                                dwriteTypographicFeaturesPinned,
                                pFeatureRangeLengthsPinned,
                                featureRanges,
                                dwriteGlyphAdvancesPinned,
                                dwriteGlyphOffsetsPinned
                                );

                            if (E_INVALIDARG == hr)
                            {
                                // If pLocaleName is unsupported (e.g. "prs-af"), DWrite returns E_INVALIDARG.
                                // Try again with the default mapping.
                                hr = _textAnalyzer.Value->GetGlyphPlacements(
                                    (ushort*)textString,
                                    clusterMap,
                                    (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps,
                                    textLength,
                                    glyphIndices,
                                    (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps,
                                    glyphCount,
                                    fontFace.DWriteFontFaceNoAddRef,
                                    fontEmSizeFloat,
                                    isSideways ? 1 : 0,
                                    isRightToLeft ? 1 : 0,
                                    (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                                    null /* default locale mapping */,
                                    dwriteTypographicFeaturesPinned,
                                    pFeatureRangeLengthsPinned,
                                    featureRanges,
                                    dwriteGlyphAdvancesPinned,
                                    dwriteGlyphOffsetsPinned
                                    );
                            }

                        }
                        else
                        {
                            hr = _textAnalyzer.Value->GetGdiCompatibleGlyphPlacements(
                                (ushort*)textString,
                                clusterMap,
                                (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps,
                                textLength,
                                glyphIndices,
                                (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps,
                                glyphCount,
                                fontFace.DWriteFontFaceNoAddRef,
                                fontEmSizeFloat,
                                pixelsPerDip,
                                &transform,
                                0,  // useGdiNatural
                                isSideways ? 1 : 0,
                                isRightToLeft ? 1 : 0,
                                (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                                (ushort*)pLocaleName,
                                dwriteTypographicFeaturesPinned,
                                pFeatureRangeLengthsPinned,
                                featureRanges,
                                dwriteGlyphAdvancesPinned,
                                dwriteGlyphOffsetsPinned
                                );

                            if (hr == E_INVALIDARG)
                            {
                                // If pLocaleName is unsupported (e.g. "prs-af"), DWrite returns E_INVALIDARG.
                                // Try again with the default mapping.
                                hr = _textAnalyzer.Value->GetGdiCompatibleGlyphPlacements(
                                    (ushort*)textString,
                                    clusterMap,
                                    (DWRITE_SHAPING_TEXT_PROPERTIES*)textProps,
                                    textLength,
                                    glyphIndices,
                                    (DWRITE_SHAPING_GLYPH_PROPERTIES*)glyphProps,
                                    glyphCount,
                                    fontFace.DWriteFontFaceNoAddRef,
                                    fontEmSizeFloat,
                                    pixelsPerDip,
                                    &transform,
                                    0,  // useGdiNatural
                                    isSideways ? 1 : 0,
                                    isRightToLeft ? 1 : 0,
                                    (DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis),
                                    null /* default locale mapping */,
                                    dwriteTypographicFeaturesPinned,
                                    pFeatureRangeLengthsPinned,
                                    featureRanges,
                                    dwriteGlyphAdvancesPinned,
                                    dwriteGlyphOffsetsPinned
                                    );
                            }
                        }

                        if (features != null)
                        {
                            for (uint i = 0; i < featureRanges; ++i)
                            {
                                dwriteFontFeaturesGCHandles[i].Free();
                            }
                        }

                        glyphOffsets = new GlyphOffset[glyphCount];
                        if (textFormattingMode == TextFormattingMode.Ideal)
                        {
                            for (uint i = 0; i < glyphCount; ++i)
                            {
                                glyphAdvances[i] = (int)Math.Round(dwriteGlyphAdvances[i] * fontEmSize * scalingFactor / fontEmSizeFloat);
                                glyphOffsets[i].du = (int)(dwriteGlyphOffsets[i].advanceOffset * scalingFactor);
                                glyphOffsets[i].dv = (int)(dwriteGlyphOffsets[i].ascenderOffset * scalingFactor);
                            }
                        }
                        else
                        {
                            for (uint i = 0; i < glyphCount; ++i)
                            {
                                glyphAdvances[i] = (int)Math.Round(dwriteGlyphAdvances[i] * scalingFactor);
                                glyphOffsets[i].du = (int)(dwriteGlyphOffsets[i].advanceOffset * scalingFactor);
                                glyphOffsets[i].dv = (int)(dwriteGlyphOffsets[i].ascenderOffset * scalingFactor);
                            }
                        }

                        Marshal.ThrowExceptionForHR(hr);
                    }
                }
                finally
                {
                    fontFace.Release();
                }
            }
        }

        private static void GetGlyphPlacementsForControlCharacters(char* pTextString, uint textLength, Font font, TextFormattingMode textFormattingMode, double fontEmSize, double scalingFactor, bool isSideways, float pixelsPerDip, uint glyphCount, ushort* pGlyphIndices, int* glyphAdvances, out GlyphOffset[] glyphOffsets)
        {
            if (glyphCount != textLength)
            {
                Marshal.ThrowExceptionForHR(E_INVALIDARG);
            }

            glyphOffsets = new GlyphOffset[textLength];
            FontFace fontFace = font.GetFontFace();

            try
            {
                int hyphenAdvanceWidth = -1;
                for (uint i = 0; i < textLength; ++i)
                {
                    // LS will sometimes replace soft hyphens (which are invisible) with hyphens (which are visible).
                    // So if we are in this code then LS actually did this replacement and we need to display the hyphen (x002D)
                    if (pTextString[i] == CharHyphen)
                    {
                        if (hyphenAdvanceWidth == -1)
                        {
                            DWRITE_GLYPH_METRICS glyphMetrics;
                            int hr;

                            if (textFormattingMode == TextFormattingMode.Ideal)
                            {
                                hr = fontFace.DWriteFontFaceNoAddRef->GetDesignGlyphMetrics(
                                                                                            pGlyphIndices + i,
                                                                                            1,
                                                                                            &glyphMetrics,
                                                                                            0
                                                                                             );
                            }
                            else
                            {
                                hr = fontFace.DWriteFontFaceNoAddRef->GetGdiCompatibleGlyphMetrics(
                                                                                                   (float)fontEmSize,
                                                                                                   pixelsPerDip, //FLOAT pixelsPerDip,
                                                                                                   null, // optional transform
                                                                                                   (textFormattingMode != TextFormattingMode.Display) ? 1 : 0, //BOOL useGdiNatural,
                                                                                                   pGlyphIndices + i, //__in_ecount(glyphCount) UINT16 const* glyphIndices,
                                                                                                   1, //UINT32 glyphCount,
                                                                                                   &glyphMetrics, //__out_ecount(glyphCount) DWRITE_GLYPH_METRICS* glyphMetrics
                                                                                                   isSideways ? 1 : 0
                                                                                                   );
                            }

                            Marshal.ThrowExceptionForHR(hr);
                            double approximatedHyphenAW = Math.Round(glyphMetrics.advanceWidth * fontEmSize / font.Metrics.DesignUnitsPerEm * pixelsPerDip) / pixelsPerDip;
                            hyphenAdvanceWidth = (int)Math.Round(approximatedHyphenAW * scalingFactor);
                        }

                        glyphAdvances[i] = hyphenAdvanceWidth;
                    }
                    else
                    {
                        glyphAdvances[i] = 0;
                    }
                    glyphOffsets[i].du = 0;
                    glyphOffsets[i].dv = 0;
                }
            }
            finally
            {
                fontFace.Release();
            }
        }

        private static IList<Span> AnalyzeExtendedAndItemize(TextItemizer textItemizer, char* text, uint length, CultureInfo numberCulture, ClassificationUtility classification)
        {
            CharAttribute[] charAttribute = new CharAttribute[length];
            fixed (CharAttribute* pCharAttribute = charAttribute)
            {
                // Analyze the extended character ranges.
                AnalyzeExtendedCharactersAndDigits(text, length, textItemizer, pCharAttribute, numberCulture, classification);

                return textItemizer.Itemize(numberCulture, pCharAttribute, length);
            }
        }

        private static void AnalyzeExtendedCharactersAndDigits(char* text, uint length, TextItemizer textItemizer, CharAttribute* pCharAttribute, CultureInfo numberCulture, ClassificationUtility classificationUtility)
        {
            char ch = text[0];

            classificationUtility.GetCharAttribute(
                ch,
                out bool isCombining,
                out bool needsCaretInfo,
                out bool isIndic,
                out bool isDigit,
                out bool isLatin,
                out bool isStrong
                );

            bool isExtended = ItemizerHelper.IsExtendedCharacter(ch);

            uint isDigitRangeStart = 0;
            uint isDigitRangeEnd = 0;
            bool previousIsDigitValue = (numberCulture == null) ? false : isDigit;
            bool currentIsDigitValue;

            // pCharAttribute is assumed to have the same length as text. This is enforced by Itemize().
            pCharAttribute[0] = (isCombining ? CharAttribute.IsCombining : CharAttribute.None)
                               | (needsCaretInfo ? CharAttribute.NeedsCaretInfo : CharAttribute.None)
                               | (isLatin ? CharAttribute.IsLatin : CharAttribute.None)
                               | (isIndic ? CharAttribute.IsIndic : CharAttribute.None)
                               | (isStrong ? CharAttribute.IsStrong : CharAttribute.None)
                               | (isExtended ? CharAttribute.IsExtended : CharAttribute.None);

            uint i;
            for (i = 1; i < length; ++i)
            {
                ch = text[i];

                classificationUtility.GetCharAttribute(
                ch,
                out isCombining,
                out needsCaretInfo,
                out isIndic,
                out isDigit,
                out isLatin,
                out isStrong
                );

                isExtended = ItemizerHelper.IsExtendedCharacter(ch);


                pCharAttribute[i] = (isCombining ? CharAttribute.IsCombining : CharAttribute.None)
                                   | (needsCaretInfo ? CharAttribute.NeedsCaretInfo : CharAttribute.None)
                                   | (isLatin ? CharAttribute.IsLatin : CharAttribute.None)
                                   | (isIndic ? CharAttribute.IsIndic : CharAttribute.None)
                                   | (isStrong ? CharAttribute.IsStrong : CharAttribute.None)
                                   | (isExtended ? CharAttribute.IsExtended : CharAttribute.None);


                currentIsDigitValue = (numberCulture == null) ? false : isDigit;
                if (previousIsDigitValue != currentIsDigitValue)
                {

                    isDigitRangeEnd = i;
                    textItemizer.SetIsDigit(isDigitRangeStart, isDigitRangeEnd - isDigitRangeStart, previousIsDigitValue);

                    isDigitRangeStart = i;
                    previousIsDigitValue = currentIsDigitValue;
                }
            }


            isDigitRangeEnd = i;
            textItemizer.SetIsDigit(isDigitRangeStart, isDigitRangeEnd - isDigitRangeStart, previousIsDigitValue);
        }

        private static void ReleaseItemizationNativeResources(
            IDWriteFactory** ppFactory,
            IDWriteTextAnalyzer** ppTextAnalyzer,
            IDWriteTextAnalysisSource** ppTextAnalysisSource,
            IDWriteTextAnalysisSink** ppTextAnalysisSink)
        {
            if (ppFactory != null && (*ppFactory) != null)
            {
                (*ppFactory)->Release();
                (*ppFactory) = null;
            }
            if (ppTextAnalyzer != null && (*ppTextAnalyzer) != null)
            {
                (*ppTextAnalyzer)->Release();
                (*ppTextAnalyzer) = null;
            }
            if (ppTextAnalysisSource != null && (*ppTextAnalysisSource) != null)
            {
                (*ppTextAnalysisSource)->Release();
                (*ppTextAnalysisSource) = null;
            }
            if (ppTextAnalysisSink != null && (*ppTextAnalysisSink) != null)
            {
                (*ppTextAnalysisSink)->Release();
                (*ppTextAnalysisSink) = null;
            }
        }

        private static void GetBlankGlyphsForControlCharacters(
            char* pTextString,
            uint                                       textLength,
            FontFace                                    fontFace,
            ushort blankGlyphIndex,
            uint                                       maxGlyphCount,
            ushort* clusterMap,
            ushort* glyphIndices,
            int* pfCanGlyphAlone,
            out uint actualGlyphCount
            )
        {
            actualGlyphCount = textLength;
            // There is not enough buffer allocated. Signal to the caller the correct buffer size.
            if (maxGlyphCount<textLength)
            {
                return;
            }

            if (textLength > ushort.MaxValue)
            {
                Marshal.ThrowExceptionForHR(E_INVALIDARG);
            }

            ushort textLengthShort = (ushort)textLength;

            uint softHyphen = (uint)CharHyphen;
            ushort hyphenGlyphIndex = 0;
            for (ushort i = 0; i < textLengthShort; ++i)
            {
                // LS will sometimes replace soft hyphens (which are invisible) with hyphens (which are visible).
                // So if we are in this code then LS actually did this replacement and we need to display the hyphen (x002D)
                if (pTextString[i] == CharHyphen)
                {
                    if (hyphenGlyphIndex == 0)
                    {
                        int hr = fontFace.DWriteFontFaceNoAddRef->GetGlyphIndices(&softHyphen,
                                                    1,
                                                    &hyphenGlyphIndex
                                                    );

                        Marshal.ThrowExceptionForHR(hr);
                    }
                    glyphIndices[i] = hyphenGlyphIndex;
                }
                else
                {
                    glyphIndices[i] = blankGlyphIndex;
                }
                clusterMap[i] = i;
                pfCanGlyphAlone[i] = 1;
            }
        }

        private static DWRITE_SCRIPT_SHAPES GetScriptShapes(ItemProps itemProps)
        {
            return ((DWRITE_SCRIPT_ANALYSIS*)(itemProps.ScriptAnalysis))->shapes;
        }

        private static uint HRESULT_FROM_WIN32(long x)
        {
            return ((uint)x) <= 0 ? ((uint)x) : ((((uint)x) & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000);
        }
    }
}
