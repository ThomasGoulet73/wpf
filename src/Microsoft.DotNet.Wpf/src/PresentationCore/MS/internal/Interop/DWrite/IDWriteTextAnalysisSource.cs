using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteTextAnalysisSource : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteFontFamily*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteFontFamily*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteFontFamily*, uint>)(lpVtbl[1]))((IDWriteFontFamily*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteFontFamily*, uint>)(lpVtbl[2]))((IDWriteFontFamily*)Unsafe.AsPointer(ref this));
        }
    }
}
