using GitHub_Api_Integration.Models;
using System.Net;
using System.Text.Json;

namespace GitHub_Api_Integration
{
    public class GitClient
    {
        private string Uri { get; }

        private HttpClient Client { get; }

        public GitClient(string userName)
        {
            Client = new HttpClient();
            Client.Timeout = TimeSpan.FromSeconds(10);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubApiIntegration/1.0");
            Uri = String.Format("https://api.github.com/users/{0}/events", userName);
        }

        public List<GitHubEvent> GetEvents()
        {
            List<GitHubEvent> events = new List<GitHubEvent>();
            HttpResponseMessage response;
            try
            {
                response = Client.GetAsync(Uri).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                events = JsonSerializer.Deserialize<List<GitHubEvent>>(responseBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return events;
        }
    }
}
