namespace MS.Internal.Interop.DWrite
{
    /// <summary>
    /// Shaping output properties per output glyph.
    /// </summary>
    internal struct DWRITE_SHAPING_GLYPH_PROPERTIES
    {
        private ushort _value;

        /// <summary>
        /// Justification class, whether to use spacing, kashidas, or
        /// another method. This exists for backwards compatibility
        /// with Uniscribe's SCRIPT_JUSTIFY enum.
        /// </summary>
        internal ushort justification
        {
            get
            {
                return (ushort)(_value & 15);
            }
            set
            {
                _value = (ushort)((_value & ~15) | (value & 15));
            }
        }

        /// <summary>
        /// Indicates glyph is the first of a cluster.
        /// </summary>
        internal ushort isClusterStart
        {
            get
            {
                return (ushort)((_value >> 4) & 1);
            }
            set
            {
                _value = (ushort)((_value & ~(1 << 4)) | ((value & 1) << 4));
            }
        }

        /// <summary>
        /// Glyph is a diacritic.
        /// </summary>
        internal ushort isDiacritic
        {
            get
            {
                return (ushort)((_value >> 5) & 1);
            }
            set
            {
                _value = (ushort)((_value & ~(1 << 5)) | ((value & 1) << 5));
            }
        }

        /// <summary>
        /// Glyph has no width, mark, ZWJ, ZWNJ, ZWSP, LRM etc.
        /// This flag is not limited to just U+200B.
        /// </summary>
        internal ushort isZeroWidthSpace
        {
            get
            {
                return (ushort)((_value >> 6) & 1);
            }
            set
            {
                _value = (ushort)((_value & ~(1 << 6)) | ((value & 1) << 6));
            }
        }

        /// <summary>
        /// Reserved for use by shaping engine.
        /// </summary>
        internal ushort reserved
        {
            get
            {
                return (ushort)((_value >> 7) & 511);
            }
            set
            {
                _value = (ushort)((_value & ~(511 << 7)) | ((value & 511) << 7));
            }
        }
    }
}
