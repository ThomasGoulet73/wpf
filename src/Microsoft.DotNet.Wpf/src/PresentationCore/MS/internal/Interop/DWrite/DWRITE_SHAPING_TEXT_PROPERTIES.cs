namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// Shaping output properties per input character.
    /// </summary>
    internal struct DWRITE_SHAPING_TEXT_PROPERTIES
    {
        private ushort _value;

        /// <summary>
        /// This character can be shaped independently from the others
        /// (usually set for the space character).
        /// </summary>
        internal ushort isShapedAlone
        {
            get
            {
                return (ushort)(_value & 1);
            }
            set
            {
                _value = (ushort)((_value & ~1) | (value & 1));
            }
        }

        /// <summary>
        /// Reserved for use by shaping engine.
        /// </summary>
        internal ushort reserved1
        {
            get
            {
                return (ushort)((_value >> 1) & 1);
            }
            set
            {
                _value = (ushort)((_value & ~(1 << 1)) | ((value & 1) << 1));
            }
        }

        /// <summary>
        /// Glyph shaping can be cut after this point without affecting shaping
        /// before or after it. Otherwise, splitting a call to GetGlyphs would
        /// cause a reflow of glyph advances and shapes.
        /// </summary>
        internal ushort canBreakShapingAfter
        {
            get
            {
                return (ushort)((_value >> 2) & 1);
            }
            set
            {
                _value = (ushort)((_value & ~(1 << 2)) | ((value & 1) << 2));
            }
        }

        /// <summary>
        /// Reserved for use by shaping engine.
        /// </summary>
        internal ushort reserved
        {
            get
            {
                return (ushort)((_value >> 3) & 8191);
            }
            set
            {
                _value = (ushort)((_value & ~(8191 << 3)) | ((value & 8191) << 3));
            }
        }
    }
}
