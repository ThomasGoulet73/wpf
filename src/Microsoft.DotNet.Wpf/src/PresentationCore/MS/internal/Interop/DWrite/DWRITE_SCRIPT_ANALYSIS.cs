namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// Association of text and its writing system script as well as some display attributes.
    /// </summary>
    internal struct DWRITE_SCRIPT_ANALYSIS
    {
        /// <summary>
        /// Zero-based index representation of writing system script.
        /// </summary>
        internal ushort script;

        /// <summary>
        /// Additional shaping requirement of text.
        /// </summary>
        internal DWRITE_SCRIPT_SHAPES shapes;
    }
}
