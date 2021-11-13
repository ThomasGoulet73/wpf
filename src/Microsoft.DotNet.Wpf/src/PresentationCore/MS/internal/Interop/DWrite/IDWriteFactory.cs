using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFactory : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFactory*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFactory* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFactory*, uint>)Vtbl[1];

            fixed (IDWriteFactory* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFactory*, uint>)Vtbl[2];

            fixed (IDWriteFactory* handle = &this)
            {
                return function(handle);
            }
        }

        public int GetSystemFontCollection(void* fontCollection, int checkForUpdate)
        {
            var function = (delegate* unmanaged<IDWriteFactory*, void*, int, int>)Vtbl[3];

            fixed (IDWriteFactory* handle = &this)
            {
                return function(handle, fontCollection, checkForUpdate);
            }
        }

        internal int CreateCustomFontCollection(IDWriteFontCollectionLoader* collectionLoader, void* collectionKey, uint collectionKeySize, IDWriteFontCollection** fontCollection)
        {
            var function = (delegate* unmanaged<IDWriteFactory*, IDWriteFontCollectionLoader*, void*, uint, IDWriteFontCollection**, int>)Vtbl[4];

            fixed (IDWriteFactory* handle = &this)
            {
                return function(handle, collectionLoader, collectionKey, collectionKeySize, fontCollection);
            }
        }

        internal int CreateTextAnalyzer(IDWriteTextAnalyzer** textAnalyzer)
        {
            var function = (delegate* unmanaged<IDWriteFactory*, IDWriteTextAnalyzer**, int>)Vtbl[21];

            fixed (IDWriteFactory* handle = &this)
            {
                int hr = function(handle, textAnalyzer);

                return hr;
            }
        }
    }
}
