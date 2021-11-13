namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// The DWRITE_MATRIX structure specifies the graphics transform to be applied
    /// to rendered glyphs.
    /// </summary>
    internal struct DWRITE_MATRIX
    {
        /// <summary>
        /// Horizontal scaling / cosine of rotation
        /// </summary>
        internal float m11;

        /// <summary>
        /// Vertical shear / sine of rotation
        /// </summary>
        internal float m12;

        /// <summary>
        /// Horizontal shear / negative sine of rotation
        /// </summary>
        internal float m21;

        /// <summary>
        /// Vertical scaling / cosine of rotation
        /// </summary>
        internal float m22;

        /// <summary>
        /// Horizontal shift (always orthogonal regardless of rotation)
        /// </summary>
        internal float dx;

        /// <summary>
        /// Vertical shift (always orthogonal regardless of rotation)
        /// </summary>
        internal float dy;
    }
}
