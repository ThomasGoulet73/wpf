namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// The DWRITE_FONT_FEATURE structure specifies properties used to identify and execute typographic feature in the font.
    /// </summary>
    internal struct DWRITE_FONT_FEATURE
    {
        /// <summary>
        /// The feature OpenType name identifier.
        /// </summary>
#pragma warning disable CS0169
        DWRITE_FONT_FEATURE_TAG nameTag;
#pragma warning restore CS0169

        /// <summary>
        /// Execution parameter of the feature.
        /// </summary>
        /// <remarks>
        /// The parameter should be non-zero to enable the feature.  Once enabled, a feature can't be disabled again within
        /// the same range.  Features requiring a selector use this value to indicate the selector index. 
        /// </remarks>
#pragma warning disable CS0169
        uint parameter;
#pragma warning restore CS0169
    }
}
