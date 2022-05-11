// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFile : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFontFile*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFontFile*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFontFile*, uint>)(lpVtbl[1]))((IDWriteFontFile*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFontFile*, uint>)(lpVtbl[2]))((IDWriteFontFile*)Unsafe.AsPointer(ref this));
        }

        public int GetReferenceKey(void** fontFileReferenceKey, uint* fontFileReferenceKeySize)
        {
            return ((delegate* unmanaged<IDWriteFontFile*, void**, uint*, int>)(lpVtbl[3]))((IDWriteFontFile*)Unsafe.AsPointer(ref this), fontFileReferenceKey, fontFileReferenceKeySize);
        }

        public int GetLoader(IDWriteFontFileLoader** fontFileLoader)
        {
            return ((delegate* unmanaged<IDWriteFontFile*, IDWriteFontFileLoader**, int>)(lpVtbl[4]))((IDWriteFontFile*)Unsafe.AsPointer(ref this), fontFileLoader);
        }

        public int Analyze(int* isSupportedFontType, DWRITE_FONT_FILE_TYPE* fontFileType, DWRITE_FONT_FACE_TYPE* fontFaceType, uint* numberOfFaces)
        {
            return ((delegate* unmanaged<IDWriteFontFile*, int*, DWRITE_FONT_FILE_TYPE*, DWRITE_FONT_FACE_TYPE*, uint*, int>)(lpVtbl[5]))((IDWriteFontFile*)Unsafe.AsPointer(ref this), isSupportedFontType, fontFileType, fontFaceType, numberOfFaces);
        }
    }
}
