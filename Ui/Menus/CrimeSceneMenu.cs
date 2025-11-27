using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Ui.Components;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class CrimeSceneMenu
    {
        public static void Show(Quest quest, MurderCase murderCase, IQuestRepository questRepo)
        {
            bool loop = true;

            while (loop && quest.Status == QuestStatus.Accepted)
            {
                // Bygg titeltexten som visas över menyn
                string title =
                    "[Location: Crime scen]\n" +
                    "\n" +
                    $"Case: {murderCase.Title}\n" +
                    $"Status: {quest.Status}\n\n" +
                    $"{murderCase.ShortSummary}\n\n" +
                    "What is your next course of action, detective?";

                string[] options =
                {
                    "🔍 Examine the crime scene",
                    "🕵️‍♂️ Question suspects",
                    "🕰️ Review clues",
                    "⚖️ Accuse a suspect",
                    "🏠 Return home to your office"
                };

                // Använd piltangenter + Enter
                int choice = ConsoleMenu.Select(title, options, startIndex: 0, wrap: true, drawTitleArt: true);

                if (choice == -1) // Escape
                {
                    loop = false;
                    break;
                }

                switch (choice)
                {
                    case 0:
                        ShowCrimeScene(murderCase);
                        break;
                    case 1:
                        QuestionSuspects(murderCase);
                        break;
                    case 2:
                        ShowClues(murderCase);
                        break;
                    case 3:
                        Accuse(murderCase, quest, questRepo);
                        loop = false; // case over after accusation
                        break;
                    case 4:
                    default:
                        loop = false;
                        break;
                }
            }
        }

        private static void ShowCrimeScene(MurderCase c)
        {
            Console.Clear();
            Console.WriteLine("Crime scene:");
            Console.WriteLine();
            Console.WriteLine(c.CrimeSceneDescription);
            Console.WriteLine();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey(true);
        }

        private static void QuestionSuspects(MurderCase c)
        {
            while (true)
            {
                string[] options = new string[c.Suspects.Count + 1];

                for (int i = 0; i < c.Suspects.Count; i++)
                {
                    var s = c.Suspects[i];
                    options[i] = $"👤 {s.Name} ({s.Label})";
                }

                options[^1] = "⬅ Back";

                int choice = ConsoleMenu.Select("Suspects:", options, drawTitleArt: false);

                if (choice == -1 || choice == options.Length - 1)
                    return; // Escape eller "Back"

                var suspect = c.Suspects[choice];
                Console.Clear();
                Console.WriteLine($"{suspect.Name} ({suspect.Label})");
                Console.WriteLine();
                Console.WriteLine(suspect.Statement);
                Console.WriteLine();
                Console.WriteLine("Press any key to return...");
                Console.ReadKey(true);
            }
        }

        private static void ShowClues(MurderCase c)
        {
            Console.Clear();
            Console.WriteLine("Clues:");
            Console.WriteLine();
            foreach (var clue in c.Clues)
            {
                Console.WriteLine($"- {clue}");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey(true);
        }

        private static void Accuse(MurderCase c, Quest quest, IQuestRepository questRepo)
        {
            string[] options = new string[c.Suspects.Count + 1];

            for (int i = 0; i < c.Suspects.Count; i++)
            {
                options[i] = $"👤 {c.Suspects[i].Name} ({c.Suspects[i].Label})";
            }

            options[^1] = "⬅ Cancel";

            int choice = ConsoleMenu.Select("Who do you accuse?", options, drawTitleArt: false);

            if (choice == -1 || choice == options.Length - 1)
                return; // Escape eller "Cancel"

            int guessedIndex = choice; // samma index som i c.Suspects

            Console.Clear();
            if (guessedIndex == c.KillerIndex)
            {
                Console.WriteLine(" 🏆 You have solved the case, detective.");
                quest.Result = QuestResult.Solved;
              
                
            }
            else
            {
                Console.WriteLine(" ❌ That was not the right suspect. Scotland Yard will take it from here.");
                quest.Result = QuestResult.Failed;
              
            }

            // Markera fallet som klart
            quest.Status = QuestStatus.Completed;

            // Spara ändringen
            questRepo.Update(quest);

            Console.WriteLine();
            Console.WriteLine("Solution:");
            Console.WriteLine(c.SolutionText);
            Console.WriteLine();
            Console.WriteLine("Press any key to return to your desk...");
            Console.ReadKey(true);
        }
    }
}

