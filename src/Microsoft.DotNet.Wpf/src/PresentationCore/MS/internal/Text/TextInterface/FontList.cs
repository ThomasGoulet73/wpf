using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Represents a list of fonts.
    /// </summary>
    internal unsafe class FontList : IEnumerable<Font>
    {
        /// <summary>
        /// A pointer to the DWrite font list.
        /// </summary>
        private readonly NativeIUnknownWrapper<IDWriteFontList> _fontList;

        /// <summary>
        /// Constructs a Font List object.
        /// </summary>
        /// <param name="fontList">A pointer to the DWrite font list object.</param>
        internal FontList(IDWriteFontList* nativePointer)
        {
            _fontList = new NativeIUnknownWrapper<IDWriteFontList>(nativePointer);
        }

        /// <summary>
        /// Gets a pointer to the DWrite font list object.
        /// </summary>
        protected IDWriteFontList* FontListValue
            => _fontList.Value;

        /// <summary>
        /// Gets a font given its zero-based index.
        /// </summary>
        internal Font this[uint index]
        {
            get
            {
                IDWriteFont* dwriteFont = null;

                _fontList.Value->GetFont(index, & dwriteFont);

                return new Font(dwriteFont);
            }
        }

        /// <summary>
        /// Gets the count of fonts in the list.
        /// </summary>
        public uint Count
            => _fontList.Value->GetFontCount();

        public IEnumerator<Font> GetEnumerator()
        {
            return new FontsEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FontsEnumerator(this);
        }

        private class FontsEnumerator : IEnumerator<Font>
        {
            private readonly FontList _fontList;
            private long _currentIndex;

            public FontsEnumerator(FontList fontList)
            {
                _fontList = fontList;
                _currentIndex = -1;
            }

            public Font Current
            {
                get
                {
                    if (_currentIndex >= _fontList.Count)
                    {
                        throw new InvalidOperationException(LocalizedErrorMsgs.EnumeratorReachedEnd);
                    }
                    else if (_currentIndex == -1)
                    {
                        throw new InvalidOperationException(LocalizedErrorMsgs.EnumeratorNotStarted);
                    }
                    return _fontList[(uint)_currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_currentIndex >= _fontList.Count) //prevents _currentIndex from overflowing.
                {
                    return false;
                }
                _currentIndex++;
                return _currentIndex < _fontList.Count;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }
    }
}
