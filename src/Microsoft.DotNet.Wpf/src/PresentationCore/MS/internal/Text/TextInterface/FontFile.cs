using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Represents a Font File.
    /// </summary> 
    internal unsafe class FontFile : IDisposable
    {
        private const int E_NOINTERFACE = unchecked((int)0x80004002L);

        private static readonly Guid _guidForIDWriteLocalFontFileLoader = new Guid(0xb2d9f3ec, 0xc9fe, 0x4a11, 0xa2, 0xec, 0xd8, 0x62, 0x08, 0xf7, 0xc0, 0xa2);
        private readonly NativeIUnknownWrapper<IDWriteFontFile> _fontFile;

        /// <summary>
        /// Constructs a Font File object.
        /// </summary>
        /// <param name="fontFile">A pointer to the DWrite fontFile object.</param>
        internal FontFile(IDWriteFontFile* nativePointer)
        {
            _fontFile = new NativeIUnknownWrapper<IDWriteFontFile>(nativePointer);
        }

        /// WARNING: AFTER GETTING THIS NATIVE POINTER YOU ARE RESPONSIBLE FOR MAKING SURE THAT THE WRAPPING MANAGED
        /// OBJECT IS KEPT ALIVE BY THE GC OR ELSE YOU ARE RISKING THE POINTER GETTING RELEASED BEFORE YOU'D 
        /// WANT TO.
        ///
        internal IDWriteFontFile* DWriteFontFileNoAddRef =>
            _fontFile.Value;

        public void Dispose()
        {
        }

        internal bool Analyze(
                              out DWRITE_FONT_FILE_TYPE  fontFileType,
                              out DWRITE_FONT_FACE_TYPE  fontFaceType,
                              out uint  numberOfFaces,
                              out int hr
                              )
        {
            int isSupported = 0;
            uint numberOfFacesDWrite = 0;
            DWRITE_FONT_FILE_TYPE dwriteFontFileType;
            DWRITE_FONT_FACE_TYPE dwriteFontFaceType;
            hr = _fontFile.Value->Analyze(
                                    &isSupported,
                                    &dwriteFontFileType,
                                    &dwriteFontFaceType,
                                    &numberOfFacesDWrite
                                    );

            GC.KeepAlive(_fontFile);
            if (hr != 0)
            {
                fontFileType = default;
                fontFaceType = default;
                numberOfFaces = default;
                return false;
            }

            fontFileType = dwriteFontFileType;
            fontFaceType = dwriteFontFaceType;
            numberOfFaces = numberOfFacesDWrite;
            return isSupported != 0;
        }

        /// <summary>
        /// Gets the path of this font file.
        /// </summary>
        /// <returns>The path of this font file.</returns>
        internal string GetUriPath()
        {
            void* fontFileReferenceKey;
            uint sizeOfFontFileReferenceKey;

            IDWriteFontFileLoader* fontFileLoader = null;
            int hr = _fontFile.Value->GetLoader(&fontFileLoader);        
            Marshal.ThrowExceptionForHR(hr);

            void* localFontFileLoaderInterfacePointer = null;
            Guid guid = _guidForIDWriteLocalFontFileLoader;
            hr = fontFileLoader->QueryInterface(&guid, &localFontFileLoaderInterfacePointer);
            if (hr == E_NOINTERFACE)
            {
                hr = _fontFile.Value->GetReferenceKey(&fontFileReferenceKey, &sizeOfFontFileReferenceKey);
                Marshal.ThrowExceptionForHR(hr);
                return new string((char*)fontFileReferenceKey);
            }
            else
            {
                IDWriteLocalFontFileLoader* localFontFileLoader = (IDWriteLocalFontFileLoader*)localFontFileLoaderInterfacePointer;

                try 
                {
                    hr = _fontFile.Value->GetReferenceKey(&fontFileReferenceKey, &sizeOfFontFileReferenceKey);
                    Marshal.ThrowExceptionForHR(hr);

                    uint sizeOfFilePath;
                    hr = localFontFileLoader->GetFilePathLengthFromKey(
                                                                   fontFileReferenceKey,
                                                                   sizeOfFontFileReferenceKey,
                                                                   &sizeOfFilePath
                                                                   );
                    Marshal.ThrowExceptionForHR(hr);

                    Invariant.Assert(sizeOfFilePath >= 0 && sizeOfFilePath < uint.MaxValue);

                    fixed (char* fontFilePath = new char[sizeOfFilePath + 1])
                    {
                        hr = localFontFileLoader->GetFilePathFromKey(
                                            fontFileReferenceKey,
                                            sizeOfFontFileReferenceKey,
                                            (ushort*)fontFilePath,
                                            sizeOfFilePath + 1
                                            );
                        Marshal.ThrowExceptionForHR(hr);
                        return new string(fontFilePath);
                    }
                }
                finally
                {
                    ReleaseInterface(&localFontFileLoader);
                }
            }
        }

        /// <summary>
        /// This method is used to release an IDWriteLocalFontFileLoader. This method
        /// is created to be marked with proper security attributes because when
        /// the call to Release() was made inside GetUriPath() it was causing Jitting.
        /// </summary>
        private static void ReleaseInterface(IDWriteLocalFontFileLoader** ppInterface)
        {
            if (ppInterface != null && *ppInterface != null)
            {
                (*ppInterface)->Release();
                *ppInterface = null;
            }
        }
    }
}
