using System;
using System.Linq;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class QuestMenu
    {
        // Använd en delad Random så att snabba anrop inte ger samma resultat
        private static readonly Random _rng = new();

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

            int index = _rng.Next(messages.Length);
            return messages[index];
        }

        public static void Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)
        {
            bool loop = true;

            while (loop)
            {
                var selection = ConsoleMenu.Select(
                    title: "What will be your next step, detective? 🕵️",
                    options: new[]
                    {
                        "🔍 Open a new murder case file",
                        "📂 Open your desktop and review ongoing cases",
                        "🫖 Call on butler, Mr Gray",
                        "🚪 Leave office"
                    },
                    startIndex: 0
                );

                switch (selection)
                {
                    case 0: // New case
                        Console.Clear();
                        TitleArt.Draw();
                        var q = gen.Generate();
                        q.OwnerUsername = currentUser.Username;
                        q.Status = QuestStatus.Accepted;
                        questRepo.Add(q);

                        Console.WriteLine($"\nCreated: {q.Title}");
                        Console.WriteLine(q.Description);
                        Console.WriteLine($"ID: {q.Id}");
                        ConsoleHelpers.Pause();
                        break;

                    case 1: // Ongoing cases
                        Console.Clear();
                        TitleArt.Draw();
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

                    case 2: // Mr Gray
                        Console.Clear();
                        TitleArt.Draw();
                        var msg = GetRandomMessage();
                        Console.WriteLine($"Mr Gray: {msg}");
                        ConsoleHelpers.Pause();
                        break;

                    case 3: // Back
                    case -1: // Escape
                        loop = false;
                        break;
                }
            }
        }
    }
}

