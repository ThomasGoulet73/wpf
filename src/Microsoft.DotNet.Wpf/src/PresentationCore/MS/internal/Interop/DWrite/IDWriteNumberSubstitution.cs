﻿using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteNumberSubstitution : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteNumberSubstitution*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteNumberSubstitution*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteNumberSubstitution*, uint>)(lpVtbl[1]))((IDWriteNumberSubstitution*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteNumberSubstitution*, uint>)(lpVtbl[2]))((IDWriteNumberSubstitution*)Unsafe.AsPointer(ref this));
        }
    }
}
