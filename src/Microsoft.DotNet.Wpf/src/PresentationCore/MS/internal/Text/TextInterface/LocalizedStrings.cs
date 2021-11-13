using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MS.Internal.Text.TextInterface
{
    /// <summary>
    /// Represents a collection of strings indexed by locale name.
    /// </summary>
    internal class LocalizedStrings : IDictionary<CultureInfo, string>
    {
        public string this[CultureInfo key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ICollection<CultureInfo> Keys => throw new System.NotImplementedException();

        public ICollection<string> Values => throw new System.NotImplementedException();

        public int Count => throw new System.NotImplementedException();

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(CultureInfo key, string value)
        {
            throw new System.NotImplementedException();
        }

        public void Add(KeyValuePair<CultureInfo, string> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<CultureInfo, string> item)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(CultureInfo key)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<CultureInfo, string>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<CultureInfo, string>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(CultureInfo key)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<CultureInfo, string> item)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(CultureInfo key, [MaybeNullWhen(false)] out string value)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
