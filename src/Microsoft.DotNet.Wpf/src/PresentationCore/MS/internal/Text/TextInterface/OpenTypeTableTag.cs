namespace MS.Internal.Text.TextInterface
{
    internal enum OpenTypeTableTag
    {
        CharToIndexMap      = (byte)'p' << 24 | (byte)'a' << 16 | (byte)'m' << 8 | (byte)'c',        /* 'cmap' */
        ControlValue        = (byte)' ' << 24 | (byte)'t' << 16 | (byte)'v' << 8 | (byte)'c',        /* 'cvt ' */
        BitmapData          = (byte)'T' << 24 | (byte)'D' << 16 | (byte)'B' << 8 | (byte)'E',        /* 'EBDT' */
        BitmapLocation      = (byte)'C' << 24 | (byte)'L' << 16 | (byte)'B' << 8 | (byte)'E',        /* 'EBLC' */
        BitmapScale         = (byte)'C' << 24 | (byte)'S' << 16 | (byte)'B' << 8 | (byte)'E',        /* 'EBSC' */
        Editor0             = (byte)'0' << 24 | (byte)'t' << 16 | (byte)'d' << 8 | (byte)'e',        /* 'edt0' */
        Editor1             = (byte)'1' << 24 | (byte)'t' << 16 | (byte)'d' << 8 | (byte)'e',        /* 'edt1' */
        Encryption          = (byte)'p' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'c',        /* 'cryp' */
        FontHeader          = (byte)'d' << 24 | (byte)'a' << 16 | (byte)'e' << 8 | (byte)'h',        /* 'head' */
        FontProgram         = (byte)'m' << 24 | (byte)'g' << 16 | (byte)'p' << 8 | (byte)'f',        /* 'fpgm' */
        GridfitAndScanProc  = (byte)'p' << 24 | (byte)'s' << 16 | (byte)'a' << 8 | (byte)'g',        /* 'gasp' */
        GlyphDirectory      = (byte)'r' << 24 | (byte)'i' << 16 | (byte)'d' << 8 | (byte)'g',        /* 'gdir' */
        GlyphData           = (byte)'f' << 24 | (byte)'y' << 16 | (byte)'l' << 8 | (byte)'g',        /* 'glyf' */
        HoriDeviceMetrics   = (byte)'x' << 24 | (byte)'m' << 16 | (byte)'d' << 8 | (byte)'h',        /* 'hdmx' */
        HoriHeader          = (byte)'a' << 24 | (byte)'e' << 16 | (byte)'h' << 8 | (byte)'h',        /* 'hhea' */
        HorizontalMetrics   = (byte)'x' << 24 | (byte)'t' << 16 | (byte)'m' << 8 | (byte)'h',        /* 'hmtx' */
        IndexToLoc          = (byte)'a' << 24 | (byte)'c' << 16 | (byte)'o' << 8 | (byte)'l',        /* 'loca' */
        Kerning             = (byte)'n' << 24 | (byte)'r' << 16 | (byte)'e' << 8 | (byte)'k',        /* 'kern' */
        LinearThreshold     = (byte)'H' << 24 | (byte)'S' << 16 | (byte)'T' << 8 | (byte)'L',        /* 'LTSH' */
        MaxProfile          = (byte)'p' << 24 | (byte)'x' << 16 | (byte)'a' << 8 | (byte)'m',        /* 'maxp' */
        NamingTable         = (byte)'e' << 24 | (byte)'m' << 16 | (byte)'a' << 8 | (byte)'n',        /* 'name' */
        OS_2                = (byte)'2' << 24 | (byte)'/' << 16 | (byte)'S' << 8 | (byte)'O',        /* 'OS/2' */
        Postscript          = (byte)'t' << 24 | (byte)'s' << 16 | (byte)'o' << 8 | (byte)'p',        /* 'post' */
        PreProgram          = (byte)'p' << 24 | (byte)'e' << 16 | (byte)'r' << 8 | (byte)'p',        /* 'prep' */
        VertDeviceMetrics   = (byte)'X' << 24 | (byte)'M' << 16 | (byte)'D' << 8 | (byte)'V',        /* 'VDMX' */
        VertHeader          = (byte)'a' << 24 | (byte)'e' << 16 | (byte)'h' << 8 | (byte)'v',        /* 'vhea' */
        VerticalMetrics     = (byte)'x' << 24 | (byte)'t' << 16 | (byte)'m' << 8 | (byte)'v',        /* 'vmtx' */
        PCLT                = (byte)'T' << 24 | (byte)'L' << 16 | (byte)'C' << 8 | (byte)'P',        /* 'PCLT' */
        TTO_GSUB            = (byte)'B' << 24 | (byte)'U' << 16 | (byte)'S' << 8 | (byte)'G',        /* 'GSUB' */
        TTO_GPOS            = (byte)'S' << 24 | (byte)'O' << 16 | (byte)'P' << 8 | (byte)'G',        /* 'GPOS' */
        TTO_GDEF            = (byte)'F' << 24 | (byte)'E' << 16 | (byte)'D' << 8 | (byte)'G',        /* 'GDEF' */
        TTO_BASE            = (byte)'E' << 24 | (byte)'S' << 16 | (byte)'A' << 8 | (byte)'B',        /* 'BASE' */
        TTO_JSTF            = (byte)'F' << 24 | (byte)'T' << 16 | (byte)'S' << 8 | (byte)'J',        /* 'JSTF' */
    }
}
