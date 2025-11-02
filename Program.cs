using TheDetectiveQuestTracker.Modell;        // User
using TheDetectiveQuestTracker.Repositories;  // InMemoryUserRepository
using TheDetectiveQuestTracker.Repositories.TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;      // Auth



namespace TheDetectiveQuestTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // var repo = new InMemoryUserRepository();
            var repo = new JsonFileUserRepository();
            var auth = new Auth(repo);

            // var questRepo = new InMemoryQuestRepository();
            var questRepo = new JsonFileQuestRepository();
            var questGen = new MurderQuestGenerator();

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
                    Console.WriteLine("1) Mordfall");
                    Console.WriteLine("9) Logout");
                    Console.WriteLine("0) Exit");
                    Console.Write("Val: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            QuestMenu(currentUser, questRepo, questGen);
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
        static void QuestMenu(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("=== Uppdrag (enkelt) ===");
                Console.WriteLine("1) Skapa nytt uppdrag");
                Console.WriteLine("2) Visa mina uppdrag");
                Console.WriteLine("0) Tillbaka");
                Console.Write("Val: ");
                var c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        var q = gen.Generate();
                        q.OwnerUsername = currentUser.Username;
                        q.Status = QuestStatus.Accepted; // direkt-accept för enkel start
                        questRepo.Add(q);

                        Console.WriteLine($"\nSkapade: {q.Title}");
                        Console.WriteLine(q.Description);
                        Console.WriteLine($"ID: {q.Id}");
                        Console.ReadKey();
                        break;

                    case "2":
                        var my = questRepo.GetForUser(currentUser.Username).ToList();
                        if (!my.Any())
                        {
                            Console.WriteLine("Du har inga uppdrag än.");
                        }
                        else
                        {
                            foreach (var mq in my)
                            {
                                Console.WriteLine($"\n[{mq.Status}] {mq.Title}");
                                Console.WriteLine(mq.Description);
                                Console.WriteLine($"ID: {mq.Id}");
                            }
                        }
                        Console.ReadKey();
                        break;

                    case "0":
                        loop = false;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val."); Console.ReadKey();
                        break;
                }
            }
        }

    }
}

