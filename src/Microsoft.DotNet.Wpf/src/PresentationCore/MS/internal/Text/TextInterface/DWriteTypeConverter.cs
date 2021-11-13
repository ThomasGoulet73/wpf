using System;
using System.Windows.Media;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// This class is used to convert data types back anf forth between DWrite and DWriteWrapper.
    /// </summary>
    internal static class DWriteTypeConverter
    {
        internal static DWRITE_MEASURING_MODE Convert(TextFormattingMode measuringMode)
        {
            switch (measuringMode)
            {
                case TextFormattingMode.Ideal:
                    return DWRITE_MEASURING_MODE.DWRITE_MEASURING_MODE_NATURAL;
                case TextFormattingMode.Display:
                    return DWRITE_MEASURING_MODE.DWRITE_MEASURING_MODE_GDI_CLASSIC;
                // We do not support Natural Metrics mode in WPF
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static DWRITE_FONT_WEIGHT Convert(FontWeight fontWeight)
        {
            // The commented cases are here only for completeness so that the code captures all the possible enum values.
            // However, these enum values are commented out since there are several enums having the same value.
            // For example, both Normal and Regular have a value of 400.
            switch (fontWeight)
            {
                case FontWeight.Thin:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_THIN;
                case FontWeight.ExtraLight:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_LIGHT;
                //case FontWeight.UltraLight:
                //    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_ULTRA_LIGHT;
                case FontWeight.Light:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_LIGHT;
                case FontWeight.Normal:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL;
                //case FontWeight.Regular:
                //    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_REGULAR;
                case FontWeight.Medium:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_MEDIUM;
                //case FontWeight.DemiBold
                //: return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_DEMI_BOLD;
                case FontWeight.SemiBOLD:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_SEMI_BOLD;
                case FontWeight.Bold:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_BOLD;
                case FontWeight.ExtraBold:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_BOLD;
                //case FontWeight.UltraBold:
                //    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_ULTRA_BOLD;
                case FontWeight.Black:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_BLACK;
                //case FontWeight.Heavy:
                //    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_HEAVY;
                case FontWeight.ExtraBlack:
                    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_BLACK;
                //case FontWeight.UltraBlack:
                //    return DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_ULTRA_BLACK;
                default:
                    int weight = (int)fontWeight;
                    if (weight >= 1 && weight <= 999)
                    {
                        return (DWRITE_FONT_WEIGHT)fontWeight;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        internal static FontSimulations Convert(DWRITE_FONT_SIMULATIONS fontSimulations)
        {
            switch (fontSimulations)
            {
                case DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_BOLD:
                    return FontSimulations.Bold;
                case DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_OBLIQUE:
                    return FontSimulations.Oblique;
                case DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_NONE:
                    return FontSimulations.None;
                // We did have (DWRITE_FONT_SIMULATIONS_BOLD | DWRITE_FONT_SIMULATIONS_OBLIQUE) as a switch case, but the compiler
                // started throwing C2051 on this when I ported from WPFText to WPFDWrite. Probably some compiler or build setting 
                // change caused this. Just moved it to the default case instead.
                default:
                    if (fontSimulations == (DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_BOLD | DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_OBLIQUE))
                        return (FontSimulations.Bold | FontSimulations.Oblique);
                    else
                        throw new InvalidOperationException();
            }
        }

        internal static FontStretch Convert(DWRITE_FONT_STRETCH fontStretch)
        {
            // The commented cases are here only for completeness so that the code captures all the possible enum values.
            // However, these enum values are commented out since there are several enums having the same value.
            // For example, both Normal and Medium have a value of 5.
            switch (fontStretch)
            {
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_UNDEFINED:
                    return FontStretch.Undefined;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_ULTRA_CONDENSED:
                    return FontStretch.UltraCondensed;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXTRA_CONDENSED:
                    return FontStretch.ExtraCondensed;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_CONDENSED:
                    return FontStretch.Condensed;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_SEMI_CONDENSED:
                    return FontStretch.SemiCondensed;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL:
                    return FontStretch.Normal;
                //case DWRITE_FONT_STRETCH_MEDIUM          : return FontStretch.Medium;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_SEMI_EXPANDED:
                    return FontStretch.SemiExpanded;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXPANDED:
                    return FontStretch.Expanded;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXTRA_EXPANDED:
                    return FontStretch.ExtraExpanded;
                case DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_ULTRA_EXPANDED:
                    return FontStretch.UltraExpanded;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static FontStyle Convert(DWRITE_FONT_STYLE fontStyle)
        {
            switch (fontStyle)
            {
                case DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL:
                    return FontStyle.Normal;
                case DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_ITALIC:
                    return FontStyle.Italic;
                case DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_OBLIQUE:
                    return FontStyle.Oblique;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static FontWeight Convert(DWRITE_FONT_WEIGHT fontWeight)
        {
            // The commented cases are here only for completeness so that the code captures all the possible enum values.
            // However, these enum values are commented out since there are several enums having the same value.
            // For example, both Normal and Regular have a value of 400.
            switch (fontWeight)
            {
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_THIN:
                    return FontWeight.Thin;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_LIGHT:
                    return FontWeight.ExtraLight;
                //case DWRITE_FONT_WEIGHT_ULTRA_LIGHT : return FontWeight.UltraLight;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_LIGHT:
                    return FontWeight.Light;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL:
                    return FontWeight.Normal;
                //case DWRITE_FONT_WEIGHT_REGULAR     : return FontWeight.Regular;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_MEDIUM:
                    return FontWeight.Medium;
                //case DWRITE_FONT_WEIGHT_DEMI_BOLD   : return FontWeight.DemiBold;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_SEMI_BOLD:
                    return FontWeight.SemiBOLD;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_BOLD:
                    return FontWeight.Bold;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_BOLD:
                    return FontWeight.ExtraBold;
                //case DWRITE_FONT_WEIGHT_ULTRA_BOLD  : return FontWeight.UltraBold;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_BLACK:
                    return FontWeight.Black;
                //case DWRITE_FONT_WEIGHT_HEAVY       : return FontWeight.Heavy;
                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_EXTRA_BLACK:
                    return FontWeight.ExtraBlack;
                //case DWRITE_FONT_WEIGHT_ULTRA_BLACK : return FontWeight.UltraBlack;

                case DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_SEMI_LIGHT: // let it fall through - because I don't know what else to do with it
                default:
                    int weight = (int)fontWeight;
                    if (weight >= 1 && weight <= 999)
                    {
                        return (FontWeight)fontWeight;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        internal static FontMetrics Convert(DWRITE_FONT_METRICS dwriteFontMetrics)
        {
            return new FontMetrics()
            {
                Ascent = (ushort)dwriteFontMetrics.ascent,
                CapHeight = (ushort)dwriteFontMetrics.capHeight,
                Descent = (ushort)dwriteFontMetrics.descent,
                DesignUnitsPerEm = (ushort)dwriteFontMetrics.designUnitsPerEm,
                LineGap = (short)dwriteFontMetrics.lineGap,
                StrikethroughPosition = (short)dwriteFontMetrics.strikethroughPosition,
                StrikethroughThickness = (ushort)dwriteFontMetrics.strikethroughThickness,
                UnderlinePosition = (short)dwriteFontMetrics.underlinePosition,
                UnderlineThickness = (ushort)dwriteFontMetrics.underlineThickness,
                XHeight = (ushort)dwriteFontMetrics.xHeight
            };
        }

        internal static DWRITE_FONT_STRETCH Convert(FontStretch fontStretch)
        {
            // The commented cases are here only for completeness so that the code captures all the possible enum values.
            // However, these enum values are commented out since there are several enums having the same value.
            // For example, both Normal and Medium have a value of 5.
            switch (fontStretch)
            {
                case FontStretch.Undefined:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_UNDEFINED;
                case FontStretch.UltraCondensed:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_ULTRA_CONDENSED;
                case FontStretch.ExtraCondensed:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXTRA_CONDENSED;
                case FontStretch.Condensed:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_CONDENSED;
                case FontStretch.SemiCondensed:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_SEMI_CONDENSED;
                case FontStretch.Normal:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL;
                //case FontStretch.Medium:
                //    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_MEDIUM;
                case FontStretch.SemiExpanded:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_SEMI_EXPANDED;
                case FontStretch.Expanded:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXPANDED;
                case FontStretch.ExtraExpanded:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_EXTRA_EXPANDED;
                case FontStretch.UltraExpanded:
                    return DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_ULTRA_EXPANDED;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static DWRITE_FONT_STYLE Convert(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Normal:
                    return DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL;
                case FontStyle.Italic:
                    return DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_ITALIC;
                case FontStyle.Oblique:
                    return DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_OBLIQUE;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
