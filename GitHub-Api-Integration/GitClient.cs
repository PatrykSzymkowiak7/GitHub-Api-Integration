using GitHub_Api_Integration.Models;
using System.Net;
using System.Text.Json;

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
                var events = JsonSerializer.Deserialize<List<GitHubEvent>>(responseBody);
                foreach(var eve in events)
                {
                    Console.WriteLine($"[{eve.CreatedAt}] {eve.Actor.Login} did {eve.Type} in repo {eve.Repo.Name}");
                }
            }
            catch(Exception ex)
            { 
                throw ex;
            }
        }

        
    }
}
