namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// This interface is used as a level on indirection for classes in managed c++ to be able to utilize methods
    /// from the static class Classification present in PresentationCore.dll.
    /// We cannot make MC++ reference PresentationCore.dll since this will result in cirular reference.
    /// </summary>
    internal interface IClassification
    {
        void GetCharAttribute(
            int unicodeScalar,
            out bool isCombining,
            out bool needsCaretInfo,
            out bool isIndic,
            out bool isDigit,
            out bool isLatin,
            out bool isStrong
            );
    }
}
