using System;
using System.IO;

namespace MS.Internal.Text.TextInterface
{
    internal interface IFontSource
    {
        void TestFileOpenable();

        UnmanagedMemoryStream GetUnmanagedStream();

        DateTime GetLastWriteTimeUtc();

        Uri Uri { get; }

        bool IsComposite { get; }
    }
}
