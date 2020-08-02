using System.Configuration;

namespace SEEL.LinguisticProcessor
{
    internal static class Constants
    {
		internal static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		internal static readonly string USERNAME = ConfigurationManager.AppSettings["username"];
		internal static readonly string PASSWORD = ConfigurationManager.AppSettings["password"];
        internal static readonly string PROJECTS_PATH = ConfigurationManager.AppSettings["projects_path"];
        internal static readonly string JAVA_PROJECTS_PATH = ConfigurationManager.AppSettings["java_projects_path"];
        internal static readonly string CSHARP_PROJECTS_PATH = ConfigurationManager.AppSettings["csharp_projects_path"];
        internal static readonly string PYTHON_PROJECTS_PATH = ConfigurationManager.AppSettings["python_projects_path"];
        internal static readonly string JAVASCRIPT_PROJECTS_PATH = ConfigurationManager.AppSettings["javascript_projects_path"];
        internal static readonly string GO_PROJECTS_PATH = ConfigurationManager.AppSettings["go_projects_path"];
        internal static readonly string JAVA_LANG = ConfigurationManager.AppSettings["java_language"];
        internal static readonly string CSHARP_LANG = ConfigurationManager.AppSettings["charp_language"];
        internal static readonly string PYTHON_LANG = ConfigurationManager.AppSettings["python_language"];
        internal static readonly string JAVASCRIPT_LANG = ConfigurationManager.AppSettings["javascript_language"];
        internal static readonly string GO_LANG = ConfigurationManager.AppSettings["go_language"];
    }
}
