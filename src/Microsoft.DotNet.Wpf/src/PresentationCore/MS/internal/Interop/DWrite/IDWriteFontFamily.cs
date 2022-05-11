using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFamily : IUnknown
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

        public int GetFirstMatchingFont(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, IDWriteFont** matchingFont)
        {
            var function = (delegate* unmanaged<IDWriteFontFamily*, DWRITE_FONT_WEIGHT, DWRITE_FONT_STRETCH, DWRITE_FONT_STYLE, IDWriteFont**, int>)lpVtbl[7];

            fixed (IDWriteFontFamily* handle = &this)
            {
                return function(handle, weight, stretch, style, matchingFont);
            }
        }
    }
}
