using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Represents a physical font in a font collection.
    /// </summary>
    internal unsafe class Font
    {
        /// <summary>
        /// An entry in the _fontFaceCache array, maps Font to FontFace.
        /// </summary>
        private struct FontFaceCacheEntry
        {
            internal Font font;
            internal FontFace fontFace;
        }

        /// <summary>
        /// The DWrite font object that this class wraps.
        /// </summary>
        private readonly NativeIUnknownWrapper<IDWriteFont> _font;

        /// <summary>
        /// FontMetrics for this font. Lazily allocated.
        /// </summary>
        private FontMetrics _fontMetrics;

        /// <summary>
        /// Mutex used to control access to _fontFaceCache, which is locked when
        /// _mutex > 0.
        /// </summary>
        private static int _mutex;

        /// <summary>
        /// Size of the _fontFaceCache, maximum number of FontFace instances cached.
        /// </summary>
        /// <remarks>
        /// Cache size could be based upon measurements of the TextFormatter micro benchmarks.
        /// English test cases allocate 1 - 3 FontFace instances, at the opposite extreme
        /// the Korean test maxes out at 13.  16 looks like a reasonable cache size.
        ///
        /// However, dwrite circa win7 has an issue aggressively consuming address space and
        /// therefore we need to be conservative holding on to font references.
        /// </remarks>
        private const int _fontFaceCacheSize = 4;

        /// <summary>
        /// Cached FontFace instances.
        /// </summary>
        private static FontFaceCacheEntry[] _fontFaceCache;

        /// <summary>
        /// Most recently used element in the FontFace cache.
        /// </summary>
        private static int _fontFaceCacheMRU;

        internal Font(IDWriteFont* nativePointer)
        {
            _font = new NativeIUnknownWrapper<IDWriteFont>(nativePointer);
        }

        /// <summary>
        /// Gets the pointer to the DWrite Font object.
        /// </summary>
        internal IntPtr DWriteFontAddRef
        {
            get
            {
                _font.Value->AddReference();
                return (IntPtr)_font.Value;
            }
        }

        /// <summary>
        /// Gets the weight of the font.
        /// </summary>
        internal FontWeight Weight
            => DWriteTypeConverter.Convert(_font.Value->GetWeight());

        /// <summary>
        /// Gets the stretch of the font.
        /// </summary>
        internal FontStretch Stretch
            => DWriteTypeConverter.Convert(_font.Value->GetStretch());

        /// <summary>
        /// Gets the style of the font.
        /// </summary>
        internal FontStyle Style
            => DWriteTypeConverter.Convert(_font.Value->GetStyle());

        /// <summary>
        /// Returns whether this is a symbol font.
        /// </summary>
        internal bool IsSymbolFont { get; }

        /// <summary>
        /// Gets a localized strings collection containing the face names for the font (e.g., Regular or Bold), indexed by locale name.
        /// </summary>
        internal LocalizedStrings FaceNames { get; }

        /// <summary>
        /// Gets the simulation flags.
        /// </summary>
        internal FontSimulations SimulationFlags
            => DWriteTypeConverter.Convert(_font.Value->GetSimulations());

        /// <summary>
        /// Gets the font metrics.
        /// </summary>
        internal FontMetrics Metrics
        {
            get
            {
                if (_fontMetrics == null)
                {
                    DWRITE_FONT_METRICS fontMetrics;
                    _font.Value->GetMetrics(
                                     &fontMetrics
                                     );

                    _fontMetrics = DWriteTypeConverter.Convert(fontMetrics);
                }
                return _fontMetrics;
            }
        }

        /// <summary>
        /// Gets the version of the font.
        /// </summary>
        internal double Version { get; }

        /// <summary>
        /// Gets the font metrics for display device.
        /// </summary>
        internal FontMetrics DisplayMetrics(float emSize, float pixelsPerDip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears the FontFace cache, releasing all native resources.
        /// </summary>
        /// <remarks>
        /// This method does not guarantee that the cache will be cleared.
        /// If the cache is busy, nothing happens.
        /// </remarks>
        internal static void ResetFontFaceCache()
        {
            FontFaceCacheEntry[] fontFaceCache = null;

            // NB: If the cache is busy, we do nothing.
            if (System.Threading.Interlocked.Increment(ref _mutex) == 1)
            {
                fontFaceCache = _fontFaceCache;
                _fontFaceCache = null;
            }
            System.Threading.Interlocked.Decrement(ref _mutex);

            if (fontFaceCache != null)
            {
                for (int i=0; i < _fontFaceCacheSize; i++)
                {
                    if (fontFaceCache[i].fontFace != null)
                    {
                        fontFaceCache[i].fontFace.Release();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a FontFace matching this Font.
        /// </summary>
        /// <remarks>
        /// Caller must use FontFace::Release to free native resources allocated by the FontFace.
        /// While FontFace does have a finalizer, it is not hard to exhaust available address space
        /// by enumerating all installed FontFaces synchronously, before the gc has a chance to
        /// kick off finalization.
        /// </remarks>
        internal FontFace GetFontFace()
        {
            FontFace fontFace = null;

            if (System.Threading.Interlocked.Increment(ref _mutex) == 1)
            {
                if (_fontFaceCache != null)
                {
                    FontFaceCacheEntry entry;
                    // Try the fast path first -- is caller accessing exactly the mru entry?
                    if ((entry = _fontFaceCache[_fontFaceCacheMRU]).font == this)
                    {
                        entry.fontFace.AddRef();
                        fontFace = entry.fontFace;
                    }
                    else
                    {
                        // No luck, do a search through the cache.
                        fontFace = LookupFontFaceSlow();
                    }
                }
            }
            System.Threading.Interlocked.Decrement(ref _mutex);

            // If the cache was busy or did not contain this Font, create a new FontFace.
            if (fontFace == null)
            {
                fontFace = AddFontFaceToCache();
            }

            return fontFace;
        }

        /// <summary>
        /// Gets a localized strings collection containing the specified informational strings, indexed by locale name.
        /// </summary>
        /// <param name="informationalStringID">Identifies the string to get.</param>
        /// <param name="informationalStrings">Receives the newly created localized strings object.</param>
        /// <returns>Whether the requested string was found or not.</returns>
        internal bool GetInformationalStrings(InformationalStringID informationalStringID, out LocalizedStrings informationalStrings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the font supports the specified character.
        /// </summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <returns>TRUE if the font supports the specified character or FALSE if not.</returns>
        internal bool HasCharacter(uint unicodeValue)
        {
            int exists = 0;
            int hr = _font.Value->HasCharacter(
                                            unicodeValue,
                                            &exists
                                            );
            Marshal.ThrowExceptionForHR(hr);
            return exists != 0;
        }

        private FontFace AddFontFaceToCache()
        {
            FontFace fontFace = CreateFontFace();
            FontFace bumpedFontFace = null;

            // NB: if the cache is busy, we simply return the new FontFace
            // without bothering to cache it.
            if (System.Threading.Interlocked.Increment(ref _mutex) == 1)
            {
                if (_fontFaceCache == null)
                {
                    _fontFaceCache = new FontFaceCacheEntry[_fontFaceCacheSize];
                }
            
                // Default to a slot that is not the MRU.
                _fontFaceCacheMRU = (_fontFaceCacheMRU + 1) % _fontFaceCacheSize;

                // Look for an empty slot.
                for (int i = 0; i < _fontFaceCacheSize; i++)
                {
                    if (_fontFaceCache[i].font == null)
                    {
                        _fontFaceCacheMRU = i;
                        break;
                    }
                }

                // Keep a reference to any discarded entry, clean it up after releasing
                // the mutex.
                bumpedFontFace = _fontFaceCache[_fontFaceCacheMRU].fontFace;

                // Record the new entry.
                _fontFaceCache[_fontFaceCacheMRU].font = this;
                _fontFaceCache[_fontFaceCacheMRU].fontFace = fontFace;
                fontFace.AddRef();
            }
            System.Threading.Interlocked.Decrement(ref _mutex);

            // If the cache was full and we replaced an unreferenced entry, release it now.
            if (bumpedFontFace != null)
            {
                bumpedFontFace.Release();
            }

            return fontFace;
        }

        private FontFace LookupFontFaceSlow()
        {
            FontFace fontFace = null;

            for (int i = 0; i < _fontFaceCacheSize; i++)
            {
                if (_fontFaceCache[i].font == this)
                {
                    fontFace = _fontFaceCache[i].fontFace;
                    fontFace.AddRef();
                    _fontFaceCacheMRU = i;
                    break;
                }
            }

            return fontFace;
        }

        private FontFace CreateFontFace()
        {
            IDWriteFontFace* dwriteFontFace;

            int hr = _font.Value->CreateFontFace(&dwriteFontFace);
            Marshal.ThrowExceptionForHR(hr);

            return new FontFace(dwriteFontFace);
        }
    }
}
