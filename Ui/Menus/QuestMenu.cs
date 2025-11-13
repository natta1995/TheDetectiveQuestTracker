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
                        "🔍 Take on a new case",
                        "📂 Rewiew your ongoing cases",
                        "🤵‍♂️ Call on butler (Mr. Hargreaves)",
                        "🚪 Leave office"
                    },
                    startIndex: 0
                );

                switch (selection)
                {
                    case 0: // New case
                        Console.Clear();
                        TitleArt.Draw();

                        var newQuest = gen.GenerateFor(currentUser);
                        questRepo.Add(newQuest);

                        Console.WriteLine("\nA new case file has been added to your desk.\n");
                        Console.WriteLine($"Title: {newQuest.Title}");
                        Console.WriteLine(newQuest.Description);
                        Console.WriteLine($"Case ID: {newQuest.Id}");
                        Console.WriteLine("\nPress any key to return to your office...");
                        ConsoleHelpers.Pause();
                        break;

                    case 1: // Ongoing cases
                        Console.Clear();
                        TitleArt.Draw();

                        var my = questRepo.GetForUser(currentUser.Username)
                                          .Where(q => q.Status == QuestStatus.Accepted)
                                          .ToList();

                        if (!my.Any())
                        {
                            Console.WriteLine("You don’t have any active cases yet.");
                            ConsoleHelpers.Pause();
                            break;
                        }

                        var options = my.Select(q => $"{q.Title} (ID: {q.Id})").ToList();
                        options.Add("⬅ Back");

                        var selectedIndex = ConsoleMenu.Select(
                            title: "Which case would you like to review?",
                            options: options.ToArray(),
                            startIndex: 0
                        );

                        if (selectedIndex == -1 || selectedIndex == options.Count - 1)
                            break; // back / escape

                        var selectedQuest = my[selectedIndex];

                        if (selectedQuest.CaseId is null)
                        {
                            Console.WriteLine("This quest is not linked to a case yet.");
                            ConsoleHelpers.Pause();
                            break;
                        }

                        var selectedCase = gen.GetCaseById(selectedQuest.CaseId);
                        if (selectedCase is null)
                        {
                            Console.WriteLine("Could not find case data.");
                            ConsoleHelpers.Pause();
                            break;
                        }

                        CaseMenu.Show(selectedQuest, selectedCase, questRepo);
                        break;

                    case 2: // Mr Gray
                        Console.Clear();
                        TitleArt.Draw();
                        var msg = GetRandomMessage();
                        Console.WriteLine($"🤵‍♂️ Mr Hargreaves: {msg}");
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

