using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteTextAnalysisSink : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSink*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteTextAnalysisSink* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSink*, uint>)Vtbl[1];

            fixed (IDWriteTextAnalysisSink* handle = &this)
            {
                return function(handle);
            }
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<IDWriteTextAnalysisSink*, uint>)Vtbl[2];

            fixed (IDWriteTextAnalysisSink* handle = &this)
            {
                return function(handle);
            }
        }
    }
}
