using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontList : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFontList*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint>)Vtbl[1];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint>)Vtbl[2];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetFontCollection(IDWriteFontCollection** fontCollection)
        {
            var function = (delegate* unmanaged<IDWriteFontList*, IDWriteFontCollection**, int>)Vtbl[3];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle, fontCollection);
            }
        }

        public uint GetFontCount()
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint>)Vtbl[4];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetFont(uint index, IDWriteFont** font)
        {
            var function = (delegate* unmanaged<IDWriteFontList*, uint, IDWriteFont**, int>)Vtbl[5];

            fixed (IDWriteFontList* handle = &this)
            {
                return function(handle, index, font);
            }
        }
    }
}
