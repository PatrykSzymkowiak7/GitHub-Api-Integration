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
        }
        else
        {
            Console.WriteLine("Username cannot be empty");
        }
    } 
}