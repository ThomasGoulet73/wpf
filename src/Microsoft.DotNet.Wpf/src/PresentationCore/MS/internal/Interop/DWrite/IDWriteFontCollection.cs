// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontCollection : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFontCollection*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFontCollection*, uint>)(lpVtbl[1]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFontCollection*, uint>)(lpVtbl[2]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this));
        }

        public uint GetFontFamilyCount()
        {
            return ((delegate* unmanaged<IDWriteFontCollection*, uint>)(lpVtbl[3]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this));
        }

        public int GetFontFamily(uint index, IDWriteFontFamily** fontFamily)
        {
            return ((delegate* unmanaged<IDWriteFontCollection*, uint, IDWriteFontFamily**, int>)(lpVtbl[4]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this), index, fontFamily);
        }

        public int FindFamilyName(ushort* familyName, uint* index, int* exists)
        {
            return ((delegate* unmanaged[Stdcall]<IDWriteFontCollection*, ushort*, uint*, int*, int>)(lpVtbl[5]))((IDWriteFontCollection*)Unsafe.AsPointer(ref this), familyName, index, exists);
        }
    }
}
