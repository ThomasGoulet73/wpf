using System;

namespace MS.Internal.Interop.DWrite
{
    internal unsafe struct IDWriteNumberSubstitution : IUnknown
    {
        private readonly void** Vtbl;

        public int QueryInterface(Guid* guid, void** comObject)
        {
            var function = (delegate* unmanaged<IDWriteNumberSubstitution*, Guid*, void**, int>)Vtbl[0];

            fixed (IDWriteNumberSubstitution* handle = &this)
            {
                return function(handle, guid, comObject);
            }
        }

        public uint AddReference()
        {
            var function = (delegate* unmanaged<uint>)Vtbl[1];

            return function();
        }

        public uint Release()
        {
            var function = (delegate* unmanaged<uint>)Vtbl[2];

            return function();
        }
    }
}
