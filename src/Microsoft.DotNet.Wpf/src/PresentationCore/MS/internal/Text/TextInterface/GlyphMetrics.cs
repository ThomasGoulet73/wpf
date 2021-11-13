using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Specifies the metrics of an individual glyph.
    /// The units depend on how the metrics are obtained.
    ///
    /// COMPATIBILITY WARNING: The layout of this struct must match exactly the layout of DWRITE_GLYPH_METRICS from DWrite.h
    /// We will perform an unsafe cast from this type to DWRITE_GLYPH_METRICS when obtaining glyph metrics. We do this for speed
    /// because we have to be able to obtain GlyphMetrics quickly, and the heap allocations and copy time were causing severe
    /// performance degradations.
    ///
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct GlyphMetrics
    {
        /// <summary>
        /// Specifies the X offset from the glyph origin to the left edge of the black box.
        /// The glyph origin is the current horizontal writing position.
        /// A negative value means the black box extends to the left of the origin (often true for lowercase italic 'f').
        /// </summary>
        [FieldOffset(0)]
        internal int LeftSideBearing;

        /// <summary>
        /// Specifies the X offset from the origin of the current glyph to the origin of the next glyph when writing horizontally.
        /// </summary>
        [FieldOffset(4)]
        internal uint AdvanceWidth;

        /// <summary>
        /// Specifies the X offset from the right edge of the black box to the origin of the next glyph when writing horizontally.
        /// The value is negative when the right edge of the black box overhangs the layout box.
        /// </summary>
        [FieldOffset(8)]
        internal int RightSideBearing;

        /// <summary>
        /// Specifies the vertical offset from the vertical origin to the top of the black box.
        /// Thus, a positive value adds whitespace whereas a negative value means the glyph overhangs the top of the layout box.
        /// </summary>
        [FieldOffset(12)]
        internal int TopSideBearing;

        /// <summary>
        /// Specifies the Y offset from the vertical origin of the current glyph to the vertical origin of the next glyph when writing vertically.
        /// (Note that the term "origin" by itself denotes the horizontal origin. The vertical origin is different.
        /// Its Y coordinate is specified by verticalOriginY value,
        /// and its X coordinate is half the advanceWidth to the right of the horizontal origin).
        /// </summary>
        [FieldOffset(16)]
        internal uint AdvanceHeight;

        /// <summary>
        /// Specifies the vertical distance from the black box's bottom edge to the advance height.
        /// Positive when the bottom edge of the black box is within the layout box.
        /// Negative when the bottom edge of black box overhangs the layout box.
        /// </summary>
        [FieldOffset(20)]
        internal int BottomSideBearing;

        /// <summary>
        /// Specifies the Y coordinate of a glyph's vertical origin, in the font's design coordinate system.
        /// The y coordinate of a glyph's vertical origin is the sum of the glyph's top side bearing
        /// and the top (i.e. yMax) of the glyph's bounding box.
        /// </summary>
        [FieldOffset(24)]
        internal int VerticalOriginY;
    }
}
