using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    class CSharpNamesExtractor : BaseNamesExtractor
    {
        public CSharpNamesExtractor(string pathToProject) : base(pathToProject)
        {
  
            RegularExpression = new Regex($"{RegularExpressions.CSharpSingleLineComment}|" +
                                            $"{RegularExpressions.CSharpMultiLineComment}|" +
                                            $"{RegularExpressions.CSharpString}|" +
                                            $"{RegularExpressions.CSharpHexNum}|" +
                                            $"{RegularExpressions.CSharpIdentifier}", 
                                            RegexOptions.ExplicitCapture);
            TargetLanguage = Constants.CSHARP_LANG;
        }

        /// <summary>
        /// Checks if the identifier is a keyword. Language-specific
        /// </summary>
        /// <param name="ident">Identifier</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsKeyword(string ident)
        {
            return RegularExpressions.CSharpKeywords.Contains(ident);
        }

        protected override string LanguageSpecificStep(string input)
        {
            var rgx = new Regex(RegularExpressions.CSharpXMLTags);
            return rgx.Replace(input, "");
        }
    }
}
