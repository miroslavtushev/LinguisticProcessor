using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    class JavascriptNamesExtractor : BaseNamesExtractor
    {
        public JavascriptNamesExtractor(string pathToProject) : base(pathToProject)
        {

            RegularExpression = new Regex($"{RegularExpressions.JavascriptSingleLineComment}|" +
                                            $"{RegularExpressions.JavascriptMultiLineComment}|" +
                                            $"{RegularExpressions.JavascriptString}|" +
                                            $"{RegularExpressions.JavascriptHexNum}|" +
                                            $"{RegularExpressions.JavascriptIdentifier}",
                                            RegexOptions.ExplicitCapture);
            TargetLanguage = Constants.JAVASCRIPT_LANG;
        }

        /// <summary>
        /// Checks if the identifier is a keyword. Language-specific
        /// </summary>
        /// <param name="ident">Identifier</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsKeyword(string ident)
        {
            return RegularExpressions.JavascriptKeywords.Contains(ident);
        }
    }
}
