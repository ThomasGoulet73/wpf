// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Windows.Xps.Serialization.RCW
{
    /// <summary>
    /// RCW for xpsobjectmodel.idl found in Windows SDK
    /// This is generated code with minor manual edits. 
    /// i.  Generate TLB
    ///      MIDL /TLB xpsobjectmodel.tlb xpsobjectmodel.IDL //xpsobjectmodel.IDL found in Windows SDK
    /// ii. Generate RCW in a DLL
    ///      TLBIMP xpsobjectmodel.tlb // Generates xpsobjectmodel.dll
    /// iii.Decompile the DLL and copy out the RCW by hand.
    ///      ILDASM xpsobjectmodel.dll
    /// </summary>

    [Guid("FD48C3F3-A58E-4B5A-8826-1DE54ABE72B2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    internal interface IXpsOMGeometryFigureCollection
    {
        void Append([In] IXpsOMGeometryFigure geometryFigure);

        IXpsOMGeometryFigure GetAt([In] uint index);

        uint GetCount();

        void InsertAt([In] uint index, [In] IXpsOMGeometryFigure geometryFigure);

        void RemoveAt([In] uint index);

        void SetAt([In] uint index, [In] IXpsOMGeometryFigure geometryFigure);
    }
}
