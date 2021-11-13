namespace MS.Internal.Text.TextInterface
{
    internal static class ItemizerHelper
    {
        /// <summary>
        /// Helper function to set ItemFlags.HasExtendedCharacter.
        /// </summary>
        /// <remarks>
        /// The old WPF Itemizer used to set ItemFlags.HasExtendedCharacter flag to TRUE for 
        /// all characters that represent extended Unicode range (>U+FFFF), this includes Surrogates Area.
        ///     Surrogate Area: U+D800  - U+DFFF
        ///     Extended Area:  U+10000 - U+10FFFF
        /// </remarks>
        /// <param name="ch">Character to investigate.</param>
        /// <returns>Value indicating whether the character is in extended UTF-16 range or not.</returns>
        internal static bool IsExtendedCharacter(char ch)
        {
            // NOTE: char is 16 bit, so values > U+FFFF cannot be expressed by char.
            //       Hence check for ((ch & 0x1F0000) > 0) is not necessary.
            return ((ch & 0xF800) == 0xD800);
        }
    }
}
