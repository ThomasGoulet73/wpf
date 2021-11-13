using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFontFile : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFontFile*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFontFile* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFontFile*, uint>)Vtbl[1];

            fixed (IDWriteFontFile* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFontFile*, uint>)Vtbl[2];

            fixed (IDWriteFontFile* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetReferenceKey(void** fontFileReferenceKey, uint* fontFileReferenceKeySize)
        {
            var function = (delegate* unmanaged<IDWriteFontFile*, void**, uint*, int>)Vtbl[3];

            fixed (IDWriteFontFile* handle = &this)
            {
                return function(handle, fontFileReferenceKey, fontFileReferenceKeySize);
            }
        }

        public int GetLoader(IDWriteFontFileLoader** fontFileLoader)
        {
            var function = (delegate* unmanaged<IDWriteFontFile*, IDWriteFontFileLoader**, int>)Vtbl[4];

            fixed (IDWriteFontFile* handle = &this)
            {
                return function(handle, fontFileLoader);
            }
        }
    }
}
