using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFace : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, uint>)Vtbl[1];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, uint>)Vtbl[2];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle);
            }
        }

        internal int GetFiles(uint* numberOfFiles, IDWriteFontFile** fontFiles)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, uint*, IDWriteFontFile**, int>)Vtbl[4];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, numberOfFiles, fontFiles);
            }
        }

        public ushort GetGlyphCount()
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, ushort>)Vtbl[9];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetDesignGlyphMetrics(ushort* glyphIndices, uint glyphCount, DWRITE_GLYPH_METRICS* glyphMetrics, int isSideways)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, ushort*, uint, DWRITE_GLYPH_METRICS*, int, int>)Vtbl[10];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, glyphIndices, glyphCount, glyphMetrics, isSideways);
            }
        }

        public int GetGlyphIndices(uint* codePoints, uint codePointCount, ushort* glyphIndices)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, uint*, uint, ushort*, int>)Vtbl[11];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, codePoints, codePointCount, glyphIndices);
            }
        }

        public int TryGetFontTable(uint openTypeTableTag, void** tableData, uint* tableSize, void** tableContext, int* exists)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, uint, void**, uint*, void**, int*, int>)Vtbl[12];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, openTypeTableTag, tableData, tableSize, tableContext, exists);
            }
        }

        public void ReleaseFontTable(void* tableContext)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, void*, void>)Vtbl[13];

            fixed (IDWriteFontFace* handle = &this)
            {
                function(handle, tableContext);
            }
        }

        public int GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX* transform, int useGdiNatural, ushort* glyphIndices, uint glyphCount, DWRITE_GLYPH_METRICS* glyphMetrics, int isSideways)
        {
            var function = (delegate* unmanaged<IDWriteFontFace*, float, float, DWRITE_MATRIX*, int, ushort*, uint, DWRITE_GLYPH_METRICS*, int, int>)Vtbl[17];

            fixed (IDWriteFontFace* handle = &this)
            {
                return function(handle, emSize, pixelsPerDip, transform, useGdiNatural, glyphIndices, glyphCount, glyphMetrics, isSideways);
            }
        }
    }
}
