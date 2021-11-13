namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// Defines a set of typographic features to be applied during shaping.
    /// Notice the character range which this feature list spans is specified
    /// as a separate parameter to GetGlyphs.
    /// </summary>
    internal unsafe struct DWRITE_TYPOGRAPHIC_FEATURES
    {
        /// <summary>
        /// Array of font features.
        /// </summary>
        internal DWRITE_FONT_FEATURE* features;

        /// <summary>
        /// The number of features.
        /// </summary>
        internal uint featureCount;
    }
}
