using System;
using System.Linq;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus;

internal static class QuestMenu
{
    public static void Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)
    {
        var loop = true;
        while (loop)
        {
            ConsoleHelpers.ClearWithTitle();
            Console.WriteLine("What will be your next step, detective? 🕵️");
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

                    Console.WriteLine($"\nCreated: {q.Title}");
                    Console.WriteLine(q.Description);
                    Console.WriteLine($"ID: {q.Id}");
                    ConsoleHelpers.Pause();
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
                    ConsoleHelpers.Pause();
                    break;

                case "0":
                    loop = false;
                    break;

                default:
                    Console.WriteLine("Ogiltigt val.");
                    ConsoleHelpers.Pause();
                    break;
            }
        }
    }
}
