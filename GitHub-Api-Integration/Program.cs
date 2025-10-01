using GitHub_Api_Integration;

public class Program
{
    private static async Task Main(string[] args)
    {
        while(true)
        {
            Console.WriteLine("Type your github username, or type end to quit");
            string? input = Console.ReadLine();

            if (input != null && input.Length > 0)
            {
                if (input.ToLower() == "end")
                    break;

                GitClient gitClient = new GitClient(input);
                var events = gitClient.GetEvents();
                foreach (var eve in events.Result)
                    Console.WriteLine($"[{eve.CreatedAt}] {eve.Actor.Login} did {eve.Type} in repo {eve.Repo.Name}");
            }
            else
                Console.WriteLine("Value cannot be empty");
        }
    } 
}