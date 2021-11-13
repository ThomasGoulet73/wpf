using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// An absolute reference to a font face.
    /// It contains font face type, appropriate file references and face identification data.
    /// Various font data such as metrics, names and glyph outlines is obtained from FontFace.
    /// </summary>
    internal unsafe class FontFace : IDisposable
    {
        /// <summary>
        /// The DWrite font face object.
        /// </summary>
        private readonly NativeIUnknownWrapper<IDWriteFontFace> _fontFace;

        /// <summary>
        /// Constructs a font face object.
        /// </summary>
        /// <param name="fontFace">A pointer to the DWrite font face object.</param>
        internal FontFace(IDWriteFontFace* nativePointer)
        {
            _fontFace = new NativeIUnknownWrapper<IDWriteFontFace>(nativePointer);
        }

        /// <summary>
        /// Gets the pointer to the DWrite Font Face object.
        ///
        /// WARNING: AFTER GETTING THIS NATIVE POINTER YOU ARE RESPONSIBLE FOR MAKING SURE THAT THE WRAPPING MANAGED
        /// OBJECT IS KEPT ALIVE BY THE GC OR ELSE YOU ARE RISKING THE POINTER GETTING RELEASED BEFORE YOU'D 
        /// WANT TO
        /// </summary>
        internal IDWriteFontFace* DWriteFontFaceNoAddRef
            => _fontFace.Value;

        /// <summary>
        /// Gets the pointer to the DWrite Font Face object.
        /// </summary>
        internal IntPtr DWriteFontFaceAddRef { get; }

        /// <summary>
        /// Gets the file format type of a font face.
        /// </summary>
        internal FontFaceType Type { get; }

        /// <summary>
        /// Gets the index of a font face in the context of its font files.
        /// </summary>
        internal uint Index { get; }

        /// <summary>
        /// Gets the number of glyphs in the font face.
        /// </summary>
        internal ushort GlyphCount
            => _fontFace.Value->GetGlyphCount();

        /// <summary>
        /// Gets the first font file representing the font face.
        /// </summary>
        internal FontFile GetFileZero()
        {
            uint numberOfFiles = 0;
            IDWriteFontFile*  pfirstDWriteFontFile = null;

            // This first call is to retrieve the numberOfFiles.
            int hr = _fontFace.Value->GetFiles(
                                            &numberOfFiles,
                                            null
                                            );
            Marshal.ThrowExceptionForHR(hr);

            if (numberOfFiles > 0)
            {
                fixed (IDWriteFontFile** ppDWriteFontFiles = new IDWriteFontFile*[numberOfFiles])
                {
                    hr = _fontFace.Value->GetFiles(
                                            &numberOfFiles,
                                            ppDWriteFontFiles
                                            );
                    Marshal.ThrowExceptionForHR(hr);

                    pfirstDWriteFontFile = ppDWriteFontFiles[0];

                    for(uint i = 1; i < numberOfFiles; ++i)
                    {
                        ppDWriteFontFiles[i]->Release();
                        ppDWriteFontFiles[i] = null;
                    }
                }
            }

            return (numberOfFiles > 0) ? new FontFile(pfirstDWriteFontFile) : null;
        }

        /// <summary>
        /// Increments the reference count on this FontFace.
        /// </summary>
        internal void AddRef()
        {
        }

        /// <summary>
        /// Decrements the reference count on this FontFace.
        /// </summary>
        internal void Release()
        {
        }

        /// <summary>
        /// Obtains ideal glyph metrics in font design units. Design glyphs metrics are used for glyph positioning.
        /// </summary>
        /// <param name="glyphIndices">An array of glyph indices to compute the metrics for.</param>
        /// <param name="pGlyphMetrics">Unsafe pointer to flat array of GlyphMetrics structs for output. Passed as
        /// unsafe to allow optimization by the caller of stack or heap allocation.
        /// The metrics returned are in font design units</param>
        internal void GetDesignGlyphMetrics(ushort* pGlyphIndices, uint glyphCount, GlyphMetrics* pGlyphMetrics)
        {
            int hr = _fontFace.Value->GetDesignGlyphMetrics(
                                                          pGlyphIndices,
                                                          glyphCount,
                                                          (DWRITE_GLYPH_METRICS*)pGlyphMetrics,
                                                          0
                                                          );

            Marshal.ThrowExceptionForHR(hr);
        }

        internal void GetDisplayGlyphMetrics(ushort* pGlyphIndices, uint glyphCount, GlyphMetrics* pGlyphMetrics, float emSize, bool useDisplayNatural, bool isSideways, float pixelsPerDip)
        {
            int hr = _fontFace.Value->GetGdiCompatibleGlyphMetrics(
                emSize,
                pixelsPerDip, //FLOAT pixelsPerDip,
                null,
                useDisplayNatural ? 1 : 0, //BOOL useGdiNatural,
                pGlyphIndices,//__in_ecount(glyphCount) UINT16 const* glyphIndices,
                glyphCount, //UINT32 glyphCount,
                (DWRITE_GLYPH_METRICS*)pGlyphMetrics, //__out_ecount(glyphCount) DWRITE_GLYPH_METRICS* glyphMetrics
                isSideways ? 1 : 0 //BOOL isSideways,
                );

            Marshal.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Returns the nominal mapping of UCS4 Unicode code points to glyph indices as defined by the font 'CMAP' table.
        /// Note that this mapping is primarily provided for line layout engines built on top of the physical font API.
        /// Because of OpenType glyph substitution and line layout character substitution, the nominal conversion does not always correspond
        /// to how a Unicode string will map to glyph indices when rendering using a particular font face.
        /// Also, note that Unicode Variant Selectors provide for alternate mappings for character to glyph.
        /// This call will always return the default variant.
        /// </summary>
        /// <param name="codePoints">An array of USC4 code points to obtain nominal glyph indices from.</param>
        /// <returns>Array of nominal glyph indices filled by this function.</returns>
        // "GetGlyphIndices" is defined in WinGDI.h to be "GetGlyphIndicesW" that why we chose
        // "GetArrayOfGlyphIndices"
        internal void GetArrayOfGlyphIndices(uint* pCodePoints, uint glyphCount, ushort* pGlyphIndices)
        {
            int hr = _fontFace.Value->GetGlyphIndices(pCodePoints,
                                                    glyphCount,
                                                    pGlyphIndices
                                                    );

            Marshal.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Finds the specified OpenType font table if it exists and returns a pointer to it.            
        /// </summary>
        /// <param name="openTypeTableTag">The tag of table to find.</param>
        /// <param name="tableData">The table.</param>
        /// <returns>True if table exists.</returns>
        internal bool TryGetFontTable(OpenTypeTableTag openTypeTableTag, out byte[] tableData)
        {
            void* tableDataDWrite;
            void* tableContext;
            uint tableSizeDWrite = 0;
            int exists = 0;
            tableData = null;
            int hr = _fontFace.Value->TryGetFontTable(
                                                   (uint)openTypeTableTag,
                                                   &tableDataDWrite,
                                                   &tableSizeDWrite,
                                                   &tableContext,
                                                   &exists
                                                   );
            Marshal.ThrowExceptionForHR(hr);

            if (exists != 0)
            {
                tableData = new byte[tableSizeDWrite];
                for (uint i = 0; i < tableSizeDWrite; ++i)
                {
                    tableData[i] = ((byte*)tableDataDWrite)[i];
                }

                _fontFace.Value->ReleaseFontTable(
                                           tableContext
                                           );
            }
            return exists != 0;
        }

        /// <summary>
        /// Reads the FontEmbeddingRights
        /// </summary>
        /// <param name="tableData">The font embedding rights value.</param>
        /// <returns>True if os2 table exists and the FontEmbeddingRights was read successfully.</returns>
        internal bool ReadFontEmbeddingRights(out ushort fsType)
        {
            void* os2Table;
            void* tableContext;
            uint tableSizeDWrite = 0;
            int exists = 0;
            fsType = 0;
            int hr = _fontFace.Value->TryGetFontTable(
                                                   DWRITE_MAKE_OPENTYPE_TAG('O','S','/','2'),
                                                   &os2Table,
                                                   &tableSizeDWrite,
                                                   &tableContext,
                                                   &exists
                                                   );
            Marshal.ThrowExceptionForHR(hr);

            const int OFFSET_OS2_fsType = 8;
            bool success = false;
            if (exists != 0)
            {
                if (tableSizeDWrite >= OFFSET_OS2_fsType + 1)
                {
                    byte* readBuffer = (byte*)os2Table + OFFSET_OS2_fsType;
                    fsType = (ushort)((readBuffer[0] << 8) + readBuffer[1]);
                    success = true;
                }
            
                _fontFace.Value->ReleaseFontTable(tableContext);
            }
        
            return success; 
        }

        private static uint DWRITE_MAKE_OPENTYPE_TAG(ushort a, ushort b, ushort c, ushort d)
        {
            return ((uint)d) << 24 | ((uint)c) << 16 | ((uint)b) << 8 | (uint)a;
        }

        public void Dispose()
        {
        }
    }
}
