using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteFont : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteFont*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteFont*, uint>)Vtbl[1];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteFont*, uint>)Vtbl[2];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_WEIGHT GetWeight()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_WEIGHT>)Vtbl[4];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_STRETCH GetStretch()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_STRETCH>)Vtbl[5];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_STYLE GetStyle()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_STYLE>)Vtbl[6];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        public DWRITE_FONT_SIMULATIONS GetSimulations()
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_SIMULATIONS>)Vtbl[10];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle);
            }
        }

        internal void GetMetrics(DWRITE_FONT_METRICS* fontMetrics)
        {
            var function = (delegate* unmanaged<IDWriteFont*, DWRITE_FONT_METRICS*, void>)Vtbl[11];

            fixed (IDWriteFont* handle = &this)
            {
                function(handle, fontMetrics);
            }
        }

        internal int HasCharacter(uint unicodeValue, int* exists)
        {
            var function = (delegate* unmanaged<IDWriteFont*, uint, int*, int>)Vtbl[12];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle, unicodeValue, exists);
            }
        }

        public int CreateFontFace(IDWriteFontFace** fontFace)
        {
            var function = (delegate* unmanaged<IDWriteFont*, IDWriteFontFace**, int>)Vtbl[13];

            fixed (IDWriteFont* handle = &this)
            {
                return function(handle, fontFace);
            }
        }
    }
}
