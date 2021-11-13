using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteTextAnalyzer : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, uint>)Vtbl[1];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, uint>)Vtbl[2];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle);
            }
        }

        public int AnalyzeScript(IDWriteTextAnalysisSource* analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink* analysisSink)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, IDWriteTextAnalysisSource*, uint, uint, IDWriteTextAnalysisSink*, int>)Vtbl[3];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, analysisSource, textPosition, textLength, analysisSink);
            }
        }

        public int AnalyzeNumberSubstitution(IDWriteTextAnalysisSource* analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink* analysisSink)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, IDWriteTextAnalysisSource*, uint, uint, IDWriteTextAnalysisSink*, int>)Vtbl[5];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, analysisSource, textPosition, textLength, analysisSink);
            }
        }

        public int GetGlyphs(ushort* textString, uint textLength, IDWriteFontFace* fontFace, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, IDWriteNumberSubstitution* numberSubstitution, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, uint maxGlyphCount, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint* actualGlyphCount)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, uint, IDWriteFontFace*, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, IDWriteNumberSubstitution*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, uint, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint*, int>)Vtbl[7];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, textString, textLength, fontFace, isSideways, isRightToLeft, scriptAnalysis, localeName, numberSubstitution, features, featureRangeLengths, featureRanges, maxGlyphCount, clusterMap, textProps, glyphIndices, glyphProps, actualGlyphCount);
            }
        }

        public int GetGlyphPlacements(ushort* textString, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, uint textLength, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint glyphCount, IDWriteFontFace* fontFace, float fontEmSize, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, float* glyphAdvances, DWRITE_GLYPH_OFFSET* glyphOffsets)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, uint, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint, IDWriteFontFace*, float, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, float*, DWRITE_GLYPH_OFFSET*, int>)Vtbl[8];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, textString, clusterMap, textProps, textLength, glyphIndices, glyphProps, glyphCount, fontFace, fontEmSize, isSideways, isRightToLeft, scriptAnalysis, localeName, features, featureRangeLengths, featureRanges, glyphAdvances, glyphOffsets);
            }
        }

        public int GetGdiCompatibleGlyphPlacements(ushort* textString, ushort* clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES* textProps, uint textLength, ushort* glyphIndices, DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps, uint glyphCount, IDWriteFontFace* fontFace, float fontEmSize, float pixelsPerDip, DWRITE_MATRIX* transform, int useGdiNatural, int isSideways, int isRightToLeft, DWRITE_SCRIPT_ANALYSIS* scriptAnalysis, ushort* localeName, DWRITE_TYPOGRAPHIC_FEATURES** features, uint* featureRangeLengths, uint featureRanges, float* glyphAdvances, DWRITE_GLYPH_OFFSET* glyphOffsets)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalyzer*, ushort*, ushort*, DWRITE_SHAPING_TEXT_PROPERTIES*, uint, ushort*, DWRITE_SHAPING_GLYPH_PROPERTIES*, uint, IDWriteFontFace*, float, float, DWRITE_MATRIX*, int, int, int, DWRITE_SCRIPT_ANALYSIS*, ushort*, DWRITE_TYPOGRAPHIC_FEATURES**, uint*, uint, float*, DWRITE_GLYPH_OFFSET*, int>)Vtbl[9];

            fixed (IDWriteTextAnalyzer* handle = &this)
            {
                return function(handle, textString, clusterMap, textProps, textLength, glyphIndices, glyphProps, glyphCount, fontFace, fontEmSize, pixelsPerDip, transform, useGdiNatural, isSideways, isRightToLeft, scriptAnalysis, localeName, features, featureRangeLengths, featureRanges, glyphAdvances, glyphOffsets);
            }
        }
    }
}
