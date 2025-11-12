using System;
using System.Linq;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus;

internal static class QuestMenu
{
    private static string GetRandomMessage() 
    {
        var messages = new[]
        {
        "I’m terribly sorry, sir — someone’s at the door.",
        "I’ve already prepared your tea, just the way you like it.\r\nAllow me to light the fireplace as well; it’s frightfully cold in here tonight.",
        "If anyone in this city can unravel those dreadful cases, it’s you, sir.\r\nDo try not to worry.",
        "Sir, this war seems to have no end.\r\nSo much misery… and the price of coffee is simply outrageous.\r\nThank heavens the tea is holding up.",
         "Sir, the city barely sleeps these days.\r\nThe sirens wail, the streets are dust and echoes…\r\nYet somehow, your tea remains warm.\r\nSmall mercies, sir — small mercies indeed.",
        "It seems to rain over London more and more these days.\r\nAnd there’s an unease in the air, sir — one can feel it.",
        "One does one’s duty, sir.\r\nEven when the walls shake. Especially then.",
        "Whatever they say, sir — you’ve done London proud.\r\nFew can claim that these days."
    };

        var random = new Random();
        int index = random.Next(messages.Length);
        return messages[index];
    }

    public static void Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)



    {
        var loop = true;
        while (loop)
        {
            ConsoleHelpers.ClearWithTitle();
            Console.WriteLine("What will be your next step, detective? 🕵️");
            Console.WriteLine("1) 🔍 Open a new murser case file");
            Console.WriteLine("2) 📂 Open your desktop and dig in to all ongoing cases");
            Console.WriteLine("3) Call on your bulter Mr Gray");
            Console.WriteLine();
            Console.WriteLine("0) 🚪 Go back to loby");
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
                case "3":
                    var msg = GetRandomMessage();
                    Console.WriteLine($"Mr Gray: {msg}");
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
