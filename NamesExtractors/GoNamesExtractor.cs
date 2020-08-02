using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    /// <summary>
    /// This class uses regular expressions to extract Go constructs from the source code 
    /// </summary>
    public class GoNamesExtractor : BaseNamesExtractor
    {
        public GoNamesExtractor(string pathToProject) : base(pathToProject)
        {
            RegularExpression = new Regex($"{RegularExpressions.GoSingleLineComment}|" +
                                                $"{RegularExpressions.GoMultiLineComment}|" +
                                               $"{RegularExpressions.GoString}|" +
                                               $"{RegularExpressions.GoHexNum}|" +
                                               $"{RegularExpressions.GoIdentifier}"
                                               , RegexOptions.ExplicitCapture);
            TargetLanguage = Constants.GO_LANG;
        }

        /// <summary>
        /// Checks if the identifier is a keyword. Language-specific
        /// </summary>
        /// <param name="ident">Identifier</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsKeyword(string ident)
        {
            return RegularExpressions.GoKeywords.Contains(ident);
        }

    }
}
