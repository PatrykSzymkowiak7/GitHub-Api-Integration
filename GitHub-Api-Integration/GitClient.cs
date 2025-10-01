using GitHub_Api_Integration.Models;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace GitHub_Api_Integration
{
    public class GitClient
    {
        private string Uri { get; }

        private HttpClient Client { get; }

        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        public GitClient(string userName)
        {
            Client = new HttpClient();
            Client.Timeout = TimeSpan.FromSeconds(10);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubApiIntegration/1.0");
            Client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            Uri = String.Format("https://api.github.com/users/{0}/events", userName);
        }

        public async Task<List<GitHubEvent>> GetEvents()
        {
            if(Cache.TryGetValue(Uri, out List<GitHubEvent> cachedEvents))
            {
                return cachedEvents;
            }

            List<GitHubEvent> events = new List<GitHubEvent>();
            HttpResponseMessage response;
            try
            {
                response = Client.GetAsync(Uri).Result;

                if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("User not found");
                    return events;
                }

                if(response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("API rate limit exceeded. Max 60 requests per hour. Try again later");
                    return events;
                }

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(responseBody))
                {
                    events = JsonSerializer.Deserialize<List<GitHubEvent>>(responseBody);
                    if(events != null && events.Count > 0)
                        events = events.OrderByDescending(e => e.CreatedAt).ToList();

                    Cache.Set(Uri, events, TimeSpan.FromMinutes(1));
                }
            }
            catch(TaskCanceledException)
            {
                Console.WriteLine("Timeout - GitHub did not respond in time");
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"Error connecting with API {ex.Message}");
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Error occured while parsing the response to JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }

            return events ?? new List<GitHubEvent>();
        }
    }
}
