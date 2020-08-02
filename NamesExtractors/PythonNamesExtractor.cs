using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    class PythonNamesExtractor : NamesExtractors.BaseNamesExtractor
    {
        public PythonNamesExtractor(string pathToProject) : base(pathToProject)
        {

            RegularExpression = new Regex($"{RegularExpressions.PythonComment}|" +
                                            $"{RegularExpressions.PythonString}|" +
                                            $"{RegularExpressions.PythonHexNum}|" +
                                            $"{RegularExpressions.PythonIdentifier}",
                                            RegexOptions.ExplicitCapture);
            TargetLanguage = Constants.PYTHON_LANG;
        }

        /// <summary>
        /// Checks if the identifier is a keyword. Language-specific
        /// </summary>
        /// <param name="ident">Identifier</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsKeyword(string ident)
        {
            return RegularExpressions.PythonKeywords.Contains(ident);
        }
    }
}
