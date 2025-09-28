using GitHub_Api_Integration;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Type your github username");
        string? userName = Console.ReadLine();

        if (userName != null && userName.Length > 0)
        {
            GitClient gitClient = new GitClient(userName);
            var events = gitClient.GetEvents();
            foreach (var eve in events)
            {
                Console.WriteLine($"[{eve.CreatedAt}] {eve.Actor.Login} did {eve.Type} in repo {eve.Repo.Name}");
            }
        }
        else
        {
            Console.WriteLine("Username cannot be empty");
        }
    } 
}