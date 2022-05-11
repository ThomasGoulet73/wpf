// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFace : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFontFace*, uint>)(lpVtbl[1]))((IDWriteFontFace*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFontFace*, uint>)(lpVtbl[2]))((IDWriteFontFace*)Unsafe.AsPointer(ref this));
        }

        internal int GetFiles(uint* numberOfFiles, IDWriteFontFile** fontFiles)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, uint*, IDWriteFontFile**, int>)(lpVtbl[4]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), numberOfFiles, fontFiles);
        }

        public ushort GetGlyphCount()
        {
            return ((delegate* unmanaged<IDWriteFontFace*, ushort>)(lpVtbl[9]))((IDWriteFontFace*)Unsafe.AsPointer(ref this));
        }

        public int GetDesignGlyphMetrics(ushort* glyphIndices, uint glyphCount, DWRITE_GLYPH_METRICS* glyphMetrics, int isSideways)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, ushort*, uint, DWRITE_GLYPH_METRICS*, int, int>)(lpVtbl[10]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), glyphIndices, glyphCount, glyphMetrics, isSideways);
        }

        public int GetGlyphIndices(uint* codePoints, uint codePointCount, ushort* glyphIndices)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, uint*, uint, ushort*, int>)(lpVtbl[11]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), codePoints, codePointCount, glyphIndices);
        }

        public int TryGetFontTable(uint openTypeTableTag, void** tableData, uint* tableSize, void** tableContext, int* exists)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, uint, void**, uint*, void**, int*, int>)(lpVtbl[12]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), openTypeTableTag, tableData, tableSize, tableContext, exists);
        }

        public void ReleaseFontTable(void* tableContext)
        {
            ((delegate* unmanaged<IDWriteFontFace*, void*, void>)(lpVtbl[13]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), tableContext);
        }

        public int GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX* transform, int useGdiNatural, ushort* glyphIndices, uint glyphCount, DWRITE_GLYPH_METRICS* glyphMetrics, int isSideways)
        {
            return ((delegate* unmanaged<IDWriteFontFace*, float, float, DWRITE_MATRIX*, int, ushort*, uint, DWRITE_GLYPH_METRICS*, int, int>)(lpVtbl[17]))((IDWriteFontFace*)Unsafe.AsPointer(ref this), emSize, pixelsPerDip, transform, useGdiNatural, glyphIndices, glyphCount, glyphMetrics, isSideways);
        }
    }
}
