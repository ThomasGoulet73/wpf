namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// Specifies algorithmic style simulations to be applied to the font face.
    /// Bold and oblique simulations can be combined via bitwise OR operation.
    /// </summary>
    internal enum DWRITE_FONT_SIMULATIONS
    {
        /// <summary>
        /// No simulations are performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_NONE = 0x0000,

        /// <summary>
        /// Algorithmic emboldening is performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_BOLD = 0x0001,

        /// <summary>
        /// Algorithmic italicization is performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_OBLIQUE = 0x0002
    }
}
