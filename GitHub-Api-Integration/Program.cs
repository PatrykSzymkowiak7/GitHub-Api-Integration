Console.WriteLine("Type your github username");

string? username = Console.ReadLine();
string uri = String.Format("https://api.github.com/users/{0}/events", username);

var client = new HttpClient();
var response = client.GetAsync(uri).Result.EnsureSuccessStatusCode();
var responseString = await response.Content.ReadAsStringAsync();