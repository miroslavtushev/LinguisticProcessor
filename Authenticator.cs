using Octokit;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// This class authenticates the app to increase the number of requests available on Github
    /// </summary>
    public static class Authenticator
    {
		public static GitHubClient client = new GitHubClient(new ProductHeaderValue("Program"));

		public static void Authenticate()
        {
            var token = "";
            var basicAuth = new Credentials(token);
            client.Credentials = basicAuth;
            //var username = Constants.USERNAME;
            //var password = Constants.PASSWORD;
            //var basicAuth = new Credentials(username, password);
            //client.Credentials = basicAuth;
        } 
    }
}
