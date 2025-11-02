using System;
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
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // var repo = new InMemoryUserRepository();
            var repo = new JsonFileUserRepository();
            var auth = new Auth(repo);

            // var questRepo = new InMemoryQuestRepository();
            var questRepo = new JsonFileQuestRepository();
            var questGen = new MurderQuestGenerator();

            User? currentUser = null;

            bool running = true;

            const string TitleArt = @"
           
┌────────────────────────────────────────────────────────────────────────────────────────────────────────────-┐
│                                                                                                             │
│   _____ _          ___      _          _   _            ___              _     _____            _           │
│  |_   _| |_  ___  |   \ ___| |_ ___ __| |_(_)_ _____   / _ \ _  _ ___ __| |_  |_   _| _ __ _ __| |_____ _ _ │
│    | | | ' \/ -_) | |) / -_)  _/ -_) _|  _| \ V / -_) | (_) | || / -_|_-<  _|   | || '_/ _` / _| / / -_) '_|│
│    |_| |_||_\___| |___/\___|\__\___\__|\__|_|\_/\___|  \__\_\\_,_\___/__/\__|   |_||_| \__,_\__|_\_\___|_|  │
│                                                                                                             │
└────────────────────────────────────────────────────────────────────────────────────────────────────────────-┘
                                                                                                                                
            ";

            while (running)
            {
                Console.Clear();

                // === Ingen inloggad ===
                if (currentUser == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{TitleArt}");
                    Console.ResetColor();

                    Console.WriteLine(" Welcome to The Secret Society");
                    Console.WriteLine();
                    Console.WriteLine("1) 🕵️ Become a member");
                    Console.WriteLine("2) 🚪 Enter the society");
                    Console.WriteLine();
                    Console.WriteLine("0) 🔚 End game");
                    Console.Write("Val: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                        
                            Console.Write("Alias: ");
                            var u = Console.ReadLine() ?? "";

                            Console.Write("Code: ");
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
                            Console.Write("Alias: ");
                            var lu = Console.ReadLine() ?? "";

                            Console.Write("Code: ");
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
                            Console.WriteLine("Invalid choice.");
                            Console.ReadKey();
                            break;
                    }
                }
                // === Inloggad meny ===
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{TitleArt}");
                    Console.ResetColor();
                    Console.WriteLine($"Welcome, Detective {currentUser.Username} 🕵️ ");
                    Console.WriteLine("We’ve been waiting for you...");
                    Console.WriteLine();
                    Console.WriteLine("10) 📜 Open you invetation");
                    Console.WriteLine("1) 🚪 Step into the dark");
                    Console.WriteLine("9) Step out");
                    Console.WriteLine();
                    Console.WriteLine("0) 🔚 End game");
                   
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

                        case "10":
                            ConsoleUi.ShowBriefingPaged(currentUser.Username);
                            break;

                        case "0":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
        static void QuestMenu(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            const string TitleArt = @"
           
┌────────────────────────────────────────────────────────────────────────────────────────────────────────────-┐
│                                                                                                             │
│   _____ _          ___      _          _   _            ___              _     _____            _           │
│  |_   _| |_  ___  |   \ ___| |_ ___ __| |_(_)_ _____   / _ \ _  _ ___ __| |_  |_   _| _ __ _ __| |_____ _ _ │
│    | | | ' \/ -_) | |) / -_)  _/ -_) _|  _| \ V / -_) | (_) | || / -_|_-<  _|   | || '_/ _` / _| / / -_) '_|│
│    |_| |_||_\___| |___/\___|\__\___\__|\__|_|\_/\___|  \__\_\\_,_\___/__/\__|   |_||_| \__,_\__|_\_\___|_|  │
│                                                                                                             │
└────────────────────────────────────────────────────────────────────────────────────────────────────────────-┘
                                                                                                                                
            ";

            var loop = true;
            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{TitleArt}");
                Console.ResetColor();
                Console.WriteLine("What will be your next step detective? 🕵️"); 
                Console.WriteLine("1) 🔍 Take a new case");
                Console.WriteLine("2) 📂 Ongoing murder cases");
                Console.WriteLine();
                Console.WriteLine("0) 🚪 Go back");
                Console.Write("Val: ");
                var c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        var q = gen.Generate();
                        q.OwnerUsername = currentUser.Username;
                        q.Status = QuestStatus.Accepted; 
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
                            Console.WriteLine("You don’t have any cases yet.");
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

