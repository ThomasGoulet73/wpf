using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteLocalFontFileLoader : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, uint>)Vtbl[1];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, uint>)Vtbl[2];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetFilePathLengthFromKey(void* fontFileReferenceKey, uint fontFileReferenceKeySize, uint* filePathLength)
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, void*, uint, uint*, int>)Vtbl[4];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle, fontFileReferenceKey, fontFileReferenceKeySize, filePathLength);
            }
        }

        public int GetFilePathFromKey(void* fontFileReferenceKey, uint fontFileReferenceKeySize, ushort* filePath, uint filePathSize)
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, void*, uint, ushort*, uint, int>)Vtbl[5];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle, fontFileReferenceKey, fontFileReferenceKeySize, filePath, filePathSize);
            }
        }
    }
}
