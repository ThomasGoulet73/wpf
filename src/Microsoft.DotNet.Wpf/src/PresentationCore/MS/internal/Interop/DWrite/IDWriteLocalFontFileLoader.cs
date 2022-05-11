using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteLocalFontFileLoader : IUnknown
    {
        public void** lpVtbl;

        public int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged<IDWriteLocalFontFileLoader*, Guid*, void**, int>)(lpVtbl[0]))((IDWriteLocalFontFileLoader*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        public uint AddRef()
        {
            return ((delegate* unmanaged<IDWriteLocalFontFileLoader*, uint>)(lpVtbl[1]))((IDWriteLocalFontFileLoader*)Unsafe.AsPointer(ref this));
        }

        public uint Release()
        {
            return ((delegate* unmanaged<IDWriteLocalFontFileLoader*, uint>)(lpVtbl[2]))((IDWriteLocalFontFileLoader*)Unsafe.AsPointer(ref this));
        }

        public int GetFilePathLengthFromKey(void* fontFileReferenceKey, uint fontFileReferenceKeySize, uint* filePathLength)
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, void*, uint, uint*, int>)lpVtbl[4];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle, fontFileReferenceKey, fontFileReferenceKeySize, filePathLength);
            }
        }

        public int GetFilePathFromKey(void* fontFileReferenceKey, uint fontFileReferenceKeySize, ushort* filePath, uint filePathSize)
        {
            var function = (delegate* unmanaged<IDWriteLocalFontFileLoader*, void*, uint, ushort*, uint, int>)lpVtbl[5];

            fixed (IDWriteLocalFontFileLoader* handle = &this)
            {
                return function(handle, fontFileReferenceKey, fontFileReferenceKeySize, filePath, filePathSize);
            }
        }
    }
}
