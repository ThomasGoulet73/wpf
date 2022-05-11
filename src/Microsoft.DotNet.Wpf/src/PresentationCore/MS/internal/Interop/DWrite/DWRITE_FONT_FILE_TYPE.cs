// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MS.Internal.Interop.DWrite
{
    internal enum DWRITE_FONT_FILE_TYPE
    {
        DWRITE_FONT_FILE_TYPE_UNKNOWN,
        DWRITE_FONT_FILE_TYPE_CFF,
        DWRITE_FONT_FILE_TYPE_TRUETYPE,
        DWRITE_FONT_FILE_TYPE_OPENTYPE_COLLECTION,
        DWRITE_FONT_FILE_TYPE_TYPE1_PFM,
        DWRITE_FONT_FILE_TYPE_TYPE1_PFB,
        DWRITE_FONT_FILE_TYPE_VECTOR,
        DWRITE_FONT_FILE_TYPE_BITMAP,
        DWRITE_FONT_FILE_TYPE_TRUETYPE_COLLECTION = DWRITE_FONT_FILE_TYPE_OPENTYPE_COLLECTION,
    }
}
