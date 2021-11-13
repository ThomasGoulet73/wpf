using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Text.TextInterface
{
    internal static class DWriteLoader
    {
        private const int LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

        internal static unsafe IntPtr LoadDWriteLibraryAndGetProcAddress(out delegate* unmanaged<int, void*, void*, int> DWriteCreateFactory)
        {
            IntPtr hDWriteLibrary = IntPtr.Zero;

            // KB2533623 introduced the LOAD_LIBRARY_SEARCH_SYSTEM32 flag. It also introduced
            // the AddDllDirectory function. We test for presence of AddDllDirectory as an 
            // indirect evidence for the support of LOAD_LIBRARY_SEARCH_SYSTEM32 flag. 
            IntPtr hKernel32 = GetModuleHandleW("kernel32.dll");

            if (hKernel32 != IntPtr.Zero)
            {
                if (GetProcAddress(hKernel32, "AddDllDirectory") != IntPtr.Zero)
                {
                    // All supported platforms newer than Vista SP2 shipped with dwrite.dll.
                    // On Vista SP2, the .NET servicing process will ensure that a MSU containing 
                    // dwrite.dll will be delivered as a prerequisite - effectively guaranteeing that 
                    // this following call to LoadLibraryEx(dwrite.dll) will succeed, and that it will 
                    // not be susceptible to typical DLL planting vulnerability vectors.
                    hDWriteLibrary = LoadLibraryExW("dwrite.dll", IntPtr.Zero, LOAD_LIBRARY_SEARCH_SYSTEM32);
                }
                else
                {
                    // LOAD_LIBRARY_SEARCH_SYSTEM32 is not supported on this OS. 
                    // Fall back to using plain ol' LoadLibrary
                    // There is risk that this call might fail, or that it might be
                    // susceptible to DLL hijacking. 
                    hDWriteLibrary = LoadLibraryW("dwrite.dll");
                }
            }

            if (hDWriteLibrary != IntPtr.Zero)
            {
                DWriteCreateFactory = (delegate* unmanaged<int, void*, void*, int>)GetProcAddress(hDWriteLibrary, "DWriteCreateFactory");
            }
            else
            {
                DWriteCreateFactory = null;
            }

            return hDWriteLibrary;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LoadLibraryExW(string lpModuleName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LoadLibraryW(string lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetModuleHandleW(string moduleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    }
}
