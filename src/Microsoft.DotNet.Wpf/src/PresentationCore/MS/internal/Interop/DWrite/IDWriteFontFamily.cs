using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFamily : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFontFamily*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFontFamily* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFontFamily*, uint>)Vtbl[1];

            fixed (IDWriteFontFamily* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFontFamily*, uint>)Vtbl[2];

            fixed (IDWriteFontFamily* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetFirstMatchingFont(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, IDWriteFont** matchingFont)
        {
            var function = (delegate* unmanaged<IDWriteFontFamily*, DWRITE_FONT_WEIGHT, DWRITE_FONT_STRETCH, DWRITE_FONT_STYLE, IDWriteFont**, int>)Vtbl[7];

            fixed (IDWriteFontFamily* handle = &this)
            {
                return function(handle, weight, stretch, style, matchingFont);
            }
        }
    }
}
