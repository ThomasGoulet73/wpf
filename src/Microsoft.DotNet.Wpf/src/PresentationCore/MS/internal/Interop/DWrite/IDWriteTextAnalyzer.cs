// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteTextAnalyzer : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, uint>)(lpVtbl[1]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, uint>)(lpVtbl[2]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this));
        }

        public int AnalyzeScript(IDWriteTextAnalysisSource* analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink* analysisSink)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, IDWriteTextAnalysisSource*, uint, uint, IDWriteTextAnalysisSink*, int>)(lpVtbl[3]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), analysisSource, textPosition, textLength, analysisSink);
        }

        public int AnalyzeNumberSubstitution(IDWriteTextAnalysisSource* analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink* analysisSink)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, IDWriteTextAnalysisSource*, uint, uint, IDWriteTextAnalysisSink*, int>)(lpVtbl[5]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), analysisSource, textPosition, textLength, analysisSink);
        }

        public int GetGlyphs(ushort* textString, uint textLength, IDWriteFontFace* fontFace, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, IDWriteNumberSubstitution* numberSubstitution, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, uint maxGlyphCount, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint* actualGlyphCount)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, uint, IDWriteFontFace*, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, IDWriteNumberSubstitution*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, uint, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint*, int>)(lpVtbl[7]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), textString, textLength, fontFace, isSideways, isRightToLeft, scriptAnalysis, localeName, numberSubstitution, features, featureRangeLengths, featureRanges, maxGlyphCount, clusterMap, textProps, glyphIndices, glyphProps, actualGlyphCount);
        }

        public int GetGlyphPlacements(ushort* textString, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, uint textLength, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint glyphCount, IDWriteFontFace* fontFace, float fontEmSize, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, float* glyphAdvances, DWRITE_GLYPH_OFFSET* glyphOffsets)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, uint, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint, IDWriteFontFace*, float, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, float*, DWRITE_GLYPH_OFFSET*, int>)(lpVtbl[8]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), textString, clusterMap, textProps, textLength, glyphIndices, glyphProps, glyphCount, fontFace, fontEmSize, isSideways, isRightToLeft, scriptAnalysis, localeName, features, featureRangeLengths, featureRanges, glyphAdvances, glyphOffsets);
        }

        public int GetGdiCompatibleGlyphPlacements(ushort* textString, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, uint textLength, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint glyphCount, IDWriteFontFace* fontFace, float fontEmSize, float pixelsPerDip, DWRITE_MATRIX* transform, int useGdiNatural, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, float* glyphAdvances, DWRITE_GLYPH_OFFSET* glyphOffsets)
        {
            return ((delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, uint, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint, IDWriteFontFace*, float, float, DWRITE_MATRIX*, int, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, float*, DWRITE_GLYPH_OFFSET*, int>)(lpVtbl[9]))((IDWriteTextAnalyzer*)Unsafe.AsPointer(ref this), textString, clusterMap, textProps, textLength, glyphIndices, glyphProps, glyphCount, fontFace, fontEmSize, pixelsPerDip, transform, useGdiNatural, isSideways, isRightToLeft, scriptAnalysis, localeName, features, featureRangeLengths, featureRanges, glyphAdvances, glyphOffsets);
        }
    }
}
