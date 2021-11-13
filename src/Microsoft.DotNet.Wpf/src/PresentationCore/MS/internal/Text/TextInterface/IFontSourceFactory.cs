namespace MS.Internal.Text.TextInterface
{
    internal interface IFontSourceFactory
    {
        IFontSource Create(string uriString);
    }
}
