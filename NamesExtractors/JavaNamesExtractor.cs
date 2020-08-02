using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
	/// <summary>
    /// This class uses regular expressions to extract java constructs from the source code 
    /// </summary>
    public class JavaNamesExtractor : BaseNamesExtractor
    {
        public JavaNamesExtractor (string pathToProject) : base (pathToProject)
        {
            RegularExpression = new Regex($"{RegularExpressions.JavaSingleLineComment}|" +
                                                $"{RegularExpressions.JavaMultiLineComment}|" +
                                               $"{RegularExpressions.JavaString}|" +
                                               $"{RegularExpressions.JavaHexNum}|" +
                                               $"{RegularExpressions.JavaAnnotation}|" +
                                               $"{RegularExpressions.JavaIdentifier}"
                                               , RegexOptions.ExplicitCapture);
            TargetLanguage = Constants.JAVA_LANG;     
        }

        /// <summary>
        /// Checks if the identifier is a keyword. Language-specific
        /// </summary>
        /// <param name="ident">Identifier</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsKeyword(string ident)
        {
            return RegularExpressions.JavaKeywords.Contains(ident);
        }

    }
}
