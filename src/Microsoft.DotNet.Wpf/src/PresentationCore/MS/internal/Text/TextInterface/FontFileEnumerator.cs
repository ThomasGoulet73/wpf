// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MS.Internal.Interop.DWrite;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace MS.Internal.Text.TextInterface
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    internal unsafe class FontFileEnumerator : IDWriteFontFileEnumeratorMirror
    {
        private const int S_OK = unchecked((int)0L);
        private const int E_INVALIDARG = unchecked((int)0x80070057L);

        private IEnumerator<IFontSource> _fontSourceCollectionEnumerator;
        private FontFileLoader _fontFileLoader;
        private IDWriteFactory* _factory;

        public FontFileEnumerator(IEnumerable<IFontSource> fontSourceCollection, FontFileLoader fontFileLoader, IDWriteFactory* factory)
        {
            _fontSourceCollectionEnumerator = fontSourceCollection.GetEnumerator();                
            _fontFileLoader                 = fontFileLoader;
            factory->AddRef();
            _factory                        = factory;
        }

        [ComVisible(true)]
        public int MoveNext(int* hasCurrentFile)
        {
            int hr = S_OK;
            try
            {
                int hasCurrentFileInt = _fontSourceCollectionEnumerator.MoveNext() ? 1 : 0;
                hasCurrentFile = &hasCurrentFileInt;
            }
            catch(Exception exception)
            {
                hr = Marshal.GetHRForException(exception);
            }

            return hr;
        }

        public int GetCurrentFontFile(IDWriteFontFile** fontFile) 
        {
            if (fontFile == null)
            {
                return E_INVALIDARG;
            }

            return Factory.CreateFontFile(
                                          _factory,
                                          _fontFileLoader,
                                          _fontSourceCollectionEnumerator.Current.Uri,
                                          fontFile
                                          );
        }
    }
}
