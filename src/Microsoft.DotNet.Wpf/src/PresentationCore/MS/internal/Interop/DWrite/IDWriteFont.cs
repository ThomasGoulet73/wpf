// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFont : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFont*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFont*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFont*, uint>)(lpVtbl[1]))((IDWriteFont*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFont*, uint>)(lpVtbl[2]))((IDWriteFont*)Unsafe.AsPointer(ref this));
        }

        public DWRITE_FONT_WEIGHT GetWeight()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_WEIGHT>)lpVtbl[4];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_STRETCH GetStretch()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_STRETCH>)lpVtbl[5];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_STYLE GetStyle()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_STYLE>)lpVtbl[6];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_SIMULATIONS GetSimulations()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_SIMULATIONS>)lpVtbl[10];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        internal void GetMetrics(DWRITE_FONT_METRICS* fontMetrics)
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_METRICS*, void>)lpVtbl[11];

            fixed (IDWriteFont* handle = &this)
            {
                function(handle, fontMetrics);
            }
        }

        internal int HasCharacter(uint unicodeValue, int* exists)
        {
            var function = (delegate* unmanaged<IDWriteFont*, uint, int*, int>)lpVtbl[12];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle, unicodeValue, exists);
            }
        }

        public int CreateFontFace(IDWriteFontFace** fontFace)
        {
            var function = (delegate* unmanaged<IDWriteFont*, IDWriteFontFace**, int>)lpVtbl[13];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle, fontFace);
            }
        }
    }
}
