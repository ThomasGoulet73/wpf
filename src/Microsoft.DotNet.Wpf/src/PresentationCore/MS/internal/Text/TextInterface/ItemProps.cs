using System;
using System.Globalization;
using MS.Internal.Interop;
using MS.Internal.Interop.DWrite;

namespace MS.Internal.Text.TextInterface
{
    internal unsafe class ItemProps
    {
        private NativeIUnknownWrapper<IDWriteNumberSubstitution> _numberSubstitution;
        private DWRITE_SCRIPT_ANALYSIS? _scriptAnalysis;

        /// <remarks>
        /// returns void* because returning IDWriteNumberSubstitution* generates asmmeta generation errors.
        /// </remarks>
        public void* NumberSubstitutionNoAddRef
             => _numberSubstitution.Value;

        /// <remarks>
        /// returns void* because returning DWRITE_SCRIPT_ANALYSIS* generates asmmeta generation errors.
        /// </remarks>
        public void* ScriptAnalysis
        {
            get
            {
                if (!_scriptAnalysis.HasValue)
                    return null;

                DWRITE_SCRIPT_ANALYSIS scriptAnalysis = _scriptAnalysis.Value;

                return &scriptAnalysis;
            }
        }

        public CultureInfo DigitCulture { get; private set; }

        public bool HasExtendedCharacter { get; private set; }

        public bool NeedsCaretInfo { get; private set; }

        public bool IsIndic { get; private set; }

        public bool IsLatin { get; private set; }

        public bool HasCombiningMark { get; private set; }

        public bool CanShapeTogether(ItemProps other)
        {
            throw new NotImplementedException();
        }

        public static ItemProps Create(
            void* scriptAnalysis,
            void* numberSubstitution,
            CultureInfo digitCulture,
            bool hasCombiningMark,
            bool needsCaretInfo,
            bool hasExtendedCharacter,
            bool isIndic,
            bool isLatin)
        {
            ItemProps result = new ItemProps();

            result.DigitCulture = digitCulture;
            result.HasCombiningMark = hasCombiningMark;
            result.HasExtendedCharacter = hasExtendedCharacter;
            result.NeedsCaretInfo = needsCaretInfo;
            result.IsIndic = isIndic;
            result.IsLatin = isLatin;

            if (scriptAnalysis != null)
            {
                result._scriptAnalysis = *(DWRITE_SCRIPT_ANALYSIS*)scriptAnalysis;
            }

            IDWriteNumberSubstitution* tempNumSubstitution = (IDWriteNumberSubstitution*)numberSubstitution;
            if (tempNumSubstitution != null)
            {
                tempNumSubstitution->AddReference();
            }

            result._numberSubstitution = new NativeIUnknownWrapper<IDWriteNumberSubstitution>(tempNumSubstitution);

            return result;
        }
    }
}
