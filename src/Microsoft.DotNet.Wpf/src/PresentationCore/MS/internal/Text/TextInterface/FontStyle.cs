namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// The font style enumeration describes the slope style of a font face, such as Normal, Italic or Oblique.
    /// Values other than the ones defined in the enumeration are considered to be invalid, and they are rejected by font API functions.
    /// </summary>
    internal enum FontStyle
    {
        /// <summary>
        /// Font slope style : Normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Font slope style : Oblique.
        /// </summary>
        Oblique = 1,

        /// <summary>
        /// Font slope style : Italic.
        /// </summary>
        Italic = 2
    }
}
