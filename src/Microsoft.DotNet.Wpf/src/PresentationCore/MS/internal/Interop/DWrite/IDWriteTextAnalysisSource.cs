using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteTextAnalysisSource : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSource*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteTextAnalysisSource* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSource*, uint>)Vtbl[1];

            fixed (IDWriteTextAnalysisSource* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSource*, uint>)Vtbl[2];

            fixed (IDWriteTextAnalysisSource* handle = &this)
            {
                return function(handle);
            }
        }
    }
}
