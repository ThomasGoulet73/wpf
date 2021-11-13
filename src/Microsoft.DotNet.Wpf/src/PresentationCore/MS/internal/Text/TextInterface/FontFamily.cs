using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Represents a set of fonts that share the same design but are differentiated
    /// by weight, stretch, and style.
    /// </summary>
    internal unsafe class FontFamily : FontList
    {
        /// <summary>
        /// A reference to the regular font in this family. This is used by upper layers in WPF.
        /// </summary>
        private Font _regularFont;

        /// <summary>
        /// Contructs a FontFamily object.
        /// </summary>
        /// <param name="fontFamily">The DWrite font family object that this class wraps.</param>
        internal FontFamily(IDWriteFontFamily* nativePointer)
            : base((IDWriteFontList*)nativePointer)
        {
        }

        /// <summary>
        /// Gets a localized strings object that contains the family names for the font family, indexed by locale name.
        /// </summary>
        internal LocalizedStrings FamilyNames { get; }

        /// <summary>
        /// Returns a name that uniquely identifies this family.
        /// The name culture doesn't matter, as the name is supposed to be used only
        /// for FontFamily construction. This is used by WPF only.
        /// </summary>
        internal string OrdinalName { get; }

        /// <summary>
        /// Gets the font metrics for the regular font in this family.
        /// </summary>
        internal FontMetrics Metrics
        {
            get
            {
                if (_regularFont == null)
                {
                    _regularFont = GetFirstMatchingFont(FontWeight.Normal, FontStretch.Normal, FontStyle.Normal);
                }
                return _regularFont.Metrics;
            }
        }

        internal FontMetrics DisplayMetrics(float emSize, float pixelsPerDip)
        {
            Font regularFont = GetFirstMatchingFont(FontWeight.Normal, FontStretch.Normal, FontStyle.Normal);     
            return regularFont.DisplayMetrics(emSize, pixelsPerDip);
        }

        /// <summary>
        /// Gets the font that best matches the specified properties.
        /// </summary>
        /// <param name="weight">Requested font weight.</param>
        /// <param name="stretch">Requested font stretch.</param>
        /// <param name="style">Requested font style.</param>
        /// <returns>The newly created font object.</returns>
        internal Font GetFirstMatchingFont(FontWeight fontWeight, FontStretch fontStretch, FontStyle fontStyle)
        {
            IDWriteFontFamily* fontFamily = (IDWriteFontFamily*)FontListValue;

            DWRITE_FONT_WEIGHT dwriteFontWeight = DWriteTypeConverter.Convert(fontWeight);
            DWRITE_FONT_STRETCH dwriteFontStretch = DWriteTypeConverter.Convert(fontStretch);
            DWRITE_FONT_STYLE dwriteFontStyle = DWriteTypeConverter.Convert(fontStyle);

            IDWriteFont* font = null;

            int hr = fontFamily->GetFirstMatchingFont(dwriteFontWeight, dwriteFontStretch, dwriteFontStyle, &font);
            Marshal.ThrowExceptionForHR(hr);

            return new Font(font);
        }
    }
}
