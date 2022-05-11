// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Text.TextInterface
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    internal unsafe class FontFileLoader : IDWriteFontFileLoaderMirror
    {
        private const int S_OK = unchecked((int)0L);
        private const int E_INVALIDARG = unchecked((int)0x80070057L);

        private readonly IFontSourceFactory _fontSourceFactory;

        public FontFileLoader(IFontSourceFactory fontSourceFactory)
        {
            _fontSourceFactory = fontSourceFactory;
        }

        [ComVisible(true)]
        public int CreateStreamFromKey([In] void* fontFileReferenceKey, [In, MarshalAs(UnmanagedType.U4)] uint fontFileReferenceKeySize, [Out] IntPtr* fontFileStream)
        {
            uint numberOfCharacters = fontFileReferenceKeySize / sizeof(char);
            if ((fontFileStream == null)
                || (fontFileReferenceKeySize % sizeof(char) != 0)                      // The fontFileReferenceKeySize must be divisible by sizeof(WCHAR)
                || (numberOfCharacters <= 1)                                            // The fontFileReferenceKey cannot be less than or equal 1 character as it has to contain the NULL character.
                || (((char*)fontFileReferenceKey)[numberOfCharacters - 1] != '\0'))    // The fontFileReferenceKey must end with the NULL character
            {
                return E_INVALIDARG;
            }

            *fontFileStream = IntPtr.Zero;

            string uriString = new string((char*)fontFileReferenceKey);
            int hr = S_OK;

            try
            {
                IFontSource fontSource = _fontSourceFactory.Create(uriString);
                FontFileStream customFontFileStream = new FontFileStream(fontSource);

                IntPtr pIDWriteFontFileStreamMirror = Marshal.GetComInterfaceForObject(
                                                        customFontFileStream,
                                                        typeof(IDWriteFontFileStreamMirror));

                *fontFileStream = pIDWriteFontFileStreamMirror;
            }
            catch (Exception exception)
            {
                hr = Marshal.GetHRForException(exception);
            }

            return hr;
        }
    }
}
