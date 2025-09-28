using System.Net;

namespace GitHub_Api_Integration
{
    public class GitClient
    {
        public GitClient(string userName)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubApiIntegration/1.0");
            string uri = String.Format("https://api.github.com/users/{0}/events", userName);
            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);
            }
            catch(Exception ex)
            { 
                throw ex;
            }
        }
    }
}
