namespace MS.Internal.Text.TextInterface
{
    internal enum CharAttribute : byte
    {
        None = 0x00,
        IsCombining = 0x01,
        NeedsCaretInfo = 0x02,
        IsIndic = 0x04,
        IsLatin = 0x08,
        IsStrong = 0x10,
        IsExtended = 0x20
    }
}
