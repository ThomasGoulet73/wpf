using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// The FontCollection encapsulates a collection of fonts.
    /// </summary>
    internal unsafe class FontCollection
    {
        /// <summary>
        /// The DWrite font collection.
        /// </summary>
        private readonly NativeIUnknownWrapper<IDWriteFontCollection> _fontCollection;

        /// <summary>
        /// Contructs a FontCollection object.
        /// </summary>
        /// <param name="fontCollection">The DWrite font collection object that this class wraps.</param>
        internal FontCollection(IDWriteFontCollection* nativePointer)
        {
            _fontCollection = new NativeIUnknownWrapper<IDWriteFontCollection>(nativePointer);
        }

        /// <summary>
        /// Gets the number of families in this font collection.
        /// </summary>
        internal uint FamilyCount
            => _fontCollection.Value->GetFontFamilyCount();

        /// <summary>
        /// Gets a font family by index.
        /// </summary>
        internal FontFamily this[uint familyIndex]
        {
            get
            {
                IDWriteFontFamily* dwriteFontFamily = null;

                int hr = _fontCollection.Value->GetFontFamily(
                                                           familyIndex,
                                                           &dwriteFontFamily
                                                           );
                Marshal.ThrowExceptionForHR(hr);

                return new FontFamily(dwriteFontFamily);
            }
        }

        /// <summary>
        /// Gets a font family by name.
        /// </summary>
        internal FontFamily this[string familyName]
        {
            get
            {
                if (FindFamilyName(familyName, out uint index))
                {
                    return this[index];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Finds the font family with the specified family name.
        /// </summary>
        /// <param name="familyName">Name of the font family. The name is not case-sensitive but must otherwise exactly match a family name in the collection.</param>
        /// <param name="index">Receives the zero-based index of the matching font family if the family name was found or UINT_MAX otherwise.</param>
        /// <returns>TRUE if the family name exists or FALSE otherwise.</returns>
        internal bool FindFamilyName(string familyName, out uint index)
        {
            int exists = 0;
            uint familyIndex = 0;

            fixed (char* familyNamePtr = familyName)
            {
                int hr = _fontCollection.Value->FindFamilyName(
                                                            (ushort*)familyNamePtr,
                                                            &familyIndex,
                                                            &exists
                                                            );
                Marshal.ThrowExceptionForHR(hr);
            }

            index = familyIndex;
            return exists != 0;
        }

        /// <summary>
        /// Gets the font object that corresponds to the same physical font as the specified font face object. The specified physical font must belong 
        /// to the font collection.
        /// </summary>
        /// <param name="fontFace">Font face object that specifies the physical font.</param>
        /// <returns>The newly created font object if successful or NULL otherwise.</returns>
        internal Font GetFontFromFontFace(FontFace fontFaceDWrite)
        {
            throw new NotImplementedException();
        }
    }
}
