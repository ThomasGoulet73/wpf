using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    internal unsafe class Factory
    {
        internal static unsafe delegate* unmanaged<int, void*, void*, int> DWriteCreateFactory;
        internal readonly NativeIUnknownWrapper<IDWriteFactory> _factory;

        // b859ee5a-d838-4b5b-a2e8-1adc7d93db48
        private static readonly Guid IID_IDWriteFactory = new Guid(0xb859ee5a, 0xd838, 0x4b5b, 0xa2, 0xe8, 0x1a, 0xdc, 0x7d, 0x93, 0xdb, 0x48);

        private Factory(IDWriteFactory* nativePointer)
        {
            _factory = new NativeIUnknownWrapper<IDWriteFactory>(nativePointer);
        }

        internal IDWriteFactory* DWriteFactory
        {
            get
            {
                return _factory.Value;
            }
        }

        internal IDWriteFactory* DWriteFactoryAddRef
        {
            get
            {
                _factory.Value->AddReference();

                return _factory.Value;
            }
        }

        private Factory(FactoryType factoryType, IFontSourceCollectionFactory fontSourceCollectionFactory, IFontSourceFactory fontSourceFactory)
        {
            Guid iid = IID_IDWriteFactory;
            IDWriteFactory* factory = null;

            int hr = DWriteCreateFactory((int)factoryType, &iid, &factory);

            Marshal.ThrowExceptionForHR(hr);

            _factory = new NativeIUnknownWrapper<IDWriteFactory>(factory);
        }

        internal static Factory Create(
            FactoryType factoryType,
            IFontSourceCollectionFactory fontSourceCollectionFactory,
            IFontSourceFactory fontSourceFactory)
        {
            return new Factory(factoryType, fontSourceCollectionFactory, fontSourceFactory);
        }

        internal TextAnalyzer CreateTextAnalyzer()
        {
            IDWriteTextAnalyzer* textAnalyzer = null;

            _factory.Value->CreateTextAnalyzer(&textAnalyzer);

            return new TextAnalyzer(textAnalyzer);
        }

        internal FontFace CreateFontFace(Uri filePathUri, uint faceIndex)
        {
            throw new NotImplementedException();
        }

        internal FontFace CreateFontFace(Uri filePathUri, uint faceIndex, FontSimulations fontSimulationFlags)
        {
            throw new NotImplementedException();
        }

        internal FontCollection GetSystemFontCollection()
        {
            return GetSystemFontCollection(false);
        }

        private FontCollection GetSystemFontCollection(bool checkForUpdate)
        {
            IDWriteFontCollection* fontCollection = null;
            int checkForUpdateInt = checkForUpdate ? 1 : 0;
            int hr = _factory.Value->GetSystemFontCollection(&fontCollection, checkForUpdateInt);

            Marshal.ThrowExceptionForHR(hr);

            return new FontCollection(fontCollection);
        }

        internal FontCollection GetFontCollection(Uri uri)
        {
            IDWriteFontCollection* fontCollection = null;

            string uriString = uri.AbsoluteUri;

            fixed (char* uriStringPtr = uriString)
            {
                uint collectionKeySize = (uint)((uriString.Length + 1) * sizeof(char));
                _factory.Value->CreateCustomFontCollection(null, uriStringPtr, collectionKeySize, &fontCollection);
            }

            return new FontCollection(fontCollection);
        }

        internal static bool IsLocalUri(Uri uri)
        {
            return uri.IsFile && uri.IsLoopback && !uri.IsUnc;
        }

        internal static DWRITE_MATRIX GetIdentityTransform()
        {
            DWRITE_MATRIX transform;
            transform.m11 = 1;
            transform.m12 = 0;
            transform.m22 = 1;
            transform.m21 = 0;
            transform.dx = 0;
            transform.dy = 0;

            return transform;
        }
    }
}
