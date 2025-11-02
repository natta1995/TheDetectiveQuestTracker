using TheDetectiveQuestTracker.Modell;        // User
using TheDetectiveQuestTracker.Repositories;  // InMemoryUserRepository
using TheDetectiveQuestTracker.Services;      // Auth

namespace TheDetectiveQuestTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repo = new InMemoryUserRepository();
            var auth = new Auth(repo);
            User? currentUser = null;

            bool running = true;

            while (running)
            {
                Console.Clear();

                // === Ingen inloggad ===
                if (currentUser == null)
                {
                    Console.WriteLine("=== The Detective Quest Tracker ===");
                    Console.WriteLine("1) Register");
                    Console.WriteLine("2) Login");
                    Console.WriteLine("0) Exit");
                    Console.Write("Val: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Username: ");
                            var u = Console.ReadLine() ?? "";

                            Console.Write("Password: ");
                            var p = Console.ReadLine() ?? "";

                            Console.Write("Email (valfritt): ");
                            var e = Console.ReadLine() ?? "";

                            Console.Write("Phone (valfritt): ");
                            var ph = Console.ReadLine() ?? "";

                            var (okR, msgR) = auth.Register(u, p, e, ph);
                            Console.WriteLine(msgR);
                            Console.ReadKey();
                            break;

                        case "2":
                            Console.Write("Username: ");
                            var lu = Console.ReadLine() ?? "";

                            Console.Write("Password: ");
                            var lp = Console.ReadLine() ?? "";

                            var (okL, user, msgL) = auth.Login(lu, lp);
                            Console.WriteLine(msgL);
                            if (okL && user != null)
                                currentUser = user;
                            Console.ReadKey();
                            break;

                        case "0":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Ogiltigt val.");
                            Console.ReadKey();
                            break;
                    }
                }
                // === Inloggad meny ===
                else
                {
                    Console.WriteLine("=== The Detective Quest Tracker ===");
                    Console.WriteLine($"Välkommen, {currentUser.Username}!");
                    Console.WriteLine();
                    Console.WriteLine("1) (Här kommer Quest-menyn senare)");
                    Console.WriteLine("9) Logout");
                    Console.WriteLine("0) Exit");
                    Console.Write("Val: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("TODO: Quest-menyn läggs till här.");
                            Console.ReadKey();
                            break;

                        case "9":
                            currentUser = null;
                            Console.WriteLine("Utloggad.");
                            Console.ReadKey();
                            break;

                        case "0":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Ogiltigt val.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}

