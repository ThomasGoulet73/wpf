using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontCollection : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFontCollection*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFontCollection* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFontCollection*, uint>)Vtbl[1];

            fixed (IDWriteFontCollection* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFontCollection*, uint>)Vtbl[2];

            fixed (IDWriteFontCollection* handle = &this)
            {
                return function(handle);
            }
        }

        public uint GetFontFamilyCount()
        {
            fixed (IDWriteFontCollection* handle = &this)
            {
                var function = (delegate* unmanaged<IDWriteFontCollection*, uint>)Vtbl[3];

                return function(handle);
            }
        }

        public int GetFontFamily(uint index, IDWriteFontFamily** fontFamily)
        {
            fixed (IDWriteFontCollection* handle = &this)
            {
                var function = (delegate* unmanaged<IDWriteFontCollection*, uint, IDWriteFontFamily**, int>)Vtbl[4];

                return function(handle, index, fontFamily);
            }
        }

        public int FindFamilyName(ushort* familyName, uint* index, int* exists)
        {
            fixed (IDWriteFontCollection* handle = &this)
            {
                var function = (delegate* unmanaged<IDWriteFontCollection*, ushort*, uint*, int*, int>)Vtbl[5];

                return function(handle, familyName, index, exists);
            }
        }
    }
}
