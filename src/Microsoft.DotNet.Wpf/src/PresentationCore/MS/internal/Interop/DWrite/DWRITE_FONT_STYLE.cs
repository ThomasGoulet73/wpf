namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// The font style enumeration describes the slope style of a font face, such as Normal, Italic or Oblique.
    /// Values other than the ones defined in the enumeration are considered to be invalid, and they are rejected by font API functions.
    /// </summary>
    internal enum DWRITE_FONT_STYLE
    {
        /// <summary>
        /// Font slope style : Normal.
        /// </summary>
        DWRITE_FONT_STYLE_NORMAL,

        /// <summary>
        /// Font slope style : Oblique.
        /// </summary>
        DWRITE_FONT_STYLE_OBLIQUE,

        /// <summary>
        /// Font slope style : Italic.
        /// </summary>
        DWRITE_FONT_STYLE_ITALIC
    }
}
