using System.Collections.Generic;

namespace SEEL.LinguisticProcessor.NamesExtractors
{
    /// <summary>
    /// Contains regular expressions for the languages to identify necessary constructs
    /// </summary>
    public static class RegularExpressions
    {
        #region Java
        public static readonly List<string> JavaKeywords = new List<string> { "abstract","continue","for","new","switch",
                                           "assert","default","if","package","synchronized",
                                           "boolean","do","goto","private","this","break","double",
                                           "implements","protected","throw","byte","else","import",
                                           "public","throws","case","enum","instanceof","return","transient",
                                           "catch","extends","int","short","try","char","final","interface",
                                           "static","void","class","finally","long","strictfp","volatile",
                                           "const","float","native","super","while","true","false","null","",
                                           ">","<","=","!","<=",">=","==","!=","&" };
        public const string JavaIdentifier = @"(?<identifier>[A-Za-z\$_][A-Za-z$_0-9\.]*)";
        public const string JavaHexNum = @"(?<hexnum>0x[\d\w]+)";

        // class, field, variable, function (method), comment
        public const string JavaSingleLineComment = @"(//(?<commentSingle>.*))";
        public const string JavaMultiLineComment = @"(/\*(?<commentMulti>(.|[\r\n])*?)\*/)";
        public const string JavaString = @"(?<string>("".*"")|('.*'))";
        public const string JavaAnnotation = @"(@(?<annotation>" + JavaIdentifier + @")(\s*\(.*\))?)";
        public const string JavaGenericType = @"(<(?<genericType>@?" + JavaIdentifier + @"[^;\{\}<>]*)>)";
        // regular class (interf., enum) + generic class
        public const string JavaClassDeclaration = @"((class|interface|enum)\s*(?<class>" + JavaIdentifier + @"))";
        public const string JavaMethodDeclaration = @"((?<methodName>" + JavaIdentifier + @")\s*\(\s*(?<parameters>(" + JavaIdentifier + @"[\s*,])*)?\)\s*(throws\s*(?<exceptions>" + JavaIdentifier + @"[\s*,\s*])*)?\s*\{)";
        public const string JavaVariable = @"(?<variable>" + JavaIdentifier + @")";
        #endregion

        #region CSharp
        // removing "_" as well since it's often used as an "out" argument which should be discraded
        public static readonly List<string> CSharpKeywords = new List<string> { "abstract","as","base","bool","break",
                                            "byte","case","catch","char","checked","class","const","continue","decimal",
                                            "default","delegate","do","double","else","enum","event","explicit","extern",
                                            "false","finally","fixed","float","for","foreach","goto","if","implicit","in",
                                            "int","interface","internal","is","lock","long","namespace","new","null",
                                            "object","operator","out","override","params","private","protected","public",
                                            "readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc","static",
                                            "string","struct","switch","this","throw","true","try","typeof","uint","ulong",
                                            "unchecked","unsafe","ushort","using","using","static","virtual","void",
                                            "volatile","while","",">","<","=","!","<=",">=","==","!=","&","add","alias",
                                            "ascending","async","await","descending","dynamic","from","get","global","group",
                                            "into","join","let","nameof","orderby","partial","remove","select","set","value",
                                            "var","when","where","yield","_" };
        // a lazy way to catch XML tag...
        public const string CSharpXMLTags = @"</?.*?>";
        // have to exclude matching hexadecimal numbers
        public const string CSharpHexNum = @"(?<hexnum>0x[\d\w]+)";
        public const string CSharpIdentifier = @"(?<identifier>[A-Za-z\$_][A-Za-z$_0-9\.\\]*)";
        public const string CSharpSingleLineComment = @"(///?(?<commentSingle>.*))";
        public const string CSharpMultiLineComment = @"(/\*(?<commentMulti>(.|[\r\n])*?)\*/)";
        public const string CSharpString = @"(?<string>(\$?@?""[\s\S]*?"")|('.*'))";
        public const string CSharpAnnotation = @"(#(?<annotation>" + CSharpIdentifier + @")(\s*\(.*\))?)";
        public const string CSharpGenericType = @"(<(?<genericType>@?" + CSharpIdentifier + @"[^;\{\}<>]*)>)";
        public const string CSharpClassDeclaration = @"((partial)?(class|struct|interface|enum)\s*(?<class>" + CSharpIdentifier + @"))";
        public const string CSharpMethodDeclaration = @"((?<methodName>" + CSharpIdentifier + @")\s*\(\s*(?<parameters>(" + CSharpIdentifier + @"[\s*,])*)?\)\s*\{)";


        #endregion

        #region Python
        public static readonly List<string> PythonKeywords = new List<string> { "False", "None", "True", "and", "as", "assert",
                                            "break", "class", "continue", "def", "del", "elif", "else", "except", "finally", "for",
                                            "from", "global", "if", "import", "in", "is", "lambda", "nonlocal", "not", "or", "pass",
                                            "raise", "return", "try", "while", "with", "yield","" };
        // doesn't handle non-ASCII characters
        public const string PythonIdentifier = @"(?<identifier>[A-Za-z_][A-Za-z_0-9\.\\]*)";
        public const string PythonHexNum = @"(?<hexnum>0x[\d\w]+)";
        public const string PythonString = @"((?s)(?<string>('''[^']*(?:'(?!'')[^']*)*''')|(""""""[^""]*(?:""(?!"""")[^""]*)*"""""")|(""[^""\\]*(?:\\.[^""\\]*)*"")|('[^'\\]*(?:\\.[^'\\]*)*')))";
        public const string PythonComment = @"(#[^!](?<commentSingle>.*))";
        #endregion

        #region JavaScript
        public static readonly List<string> JavascriptKeywords = new List<string> { "await", "break", "case", "catch", "class",
                                            "const", "continue", "debugger", "default", "delete", "do", "else", "export", "extends",
                                            "finally", "for", "function", "if", "import", "in", "instanceof", "new", "return",
                                            "super", "switch", "this", "throw", "try", "typeof", "var", "void", "while", "with", "yield", "",
                                            "enum", "implements", "package", "protected", "interface", "private", "public" };
        public const string JavascriptIdentifier = @"(?<identifier>[\$_A-Za-z_][\$A-Za-z_0-9\.]*)";
        public const string JavascriptHexNum = @"(?<hexnum>0x[\d\w]+)";
        public const string JavascriptString = @"(?<string>(""[\s\S]*?"")|('.*')|(`[\s\S]*?`))";
        public const string JavascriptSingleLineComment = @"(//(?<commentSingle>.*))";
        public const string JavascriptMultiLineComment = @"(/\*(?<commentMulti>(.|[\r\n])*?)\*/)";
        #endregion

        #region Go
        public static readonly List<string> GoKeywords = new List<string> { "break", "default", "func", "interface",
                                                                   "select", "case", "defer", "go", "map", "struct",
                                                                    "chan", "else", "goto", "package", "switch", "const",
                                                                    "fallthrough", "if", "range", "type", "continue", "for",
                                                                    "import", "return", "var" };
        public const string GoIdentifier = @"(?<identifier>[A-Za-z_][A-Za-z_0-9]*)";
        public const string GoHexNum = @"(?<hexnum>0x[\d\w]+)";
        public const string GoString = @"(?<string>(""[\s\S]*?"")|(`.*`))";
        public const string GoSingleLineComment = @"(//(?<commentSingle>.*))";
        public const string GoMultiLineComment = @"(/\*(?<commentMulti>(.|[\r\n])*?)\*/)";
        #endregion
    }
}
