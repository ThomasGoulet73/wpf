// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontList : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFontList*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFontList*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFontList*, uint>)(lpVtbl[1]))((IDWriteFontList*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFontList*, uint>)(lpVtbl[2]))((IDWriteFontList*)Unsafe.AsPointer(ref this));
        }

        public int GetFontCollection(IDWriteFontCollection** fontCollection)
        {
            var function = (delegate* unmanaged<IDWriteFontList*, IDWriteFontCollection**, int>)lpVtbl[3];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle, fontCollection);
            }
        }

        public uint GetFontCount()
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint>)lpVtbl[4];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetFont(uint index, IDWriteFont** font)
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint, IDWriteFont**, int>)lpVtbl[5];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle, index, font);
            }
        }
    }
}
