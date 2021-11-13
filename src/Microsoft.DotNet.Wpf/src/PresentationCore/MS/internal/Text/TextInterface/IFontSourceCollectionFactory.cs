namespace MS.Internal.Text.TextInterface
{
    internal interface IFontSourceCollectionFactory
    {
        IFontSourceCollection Create(string uriString);
    }
}
