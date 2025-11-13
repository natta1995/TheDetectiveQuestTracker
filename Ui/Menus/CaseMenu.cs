
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class CaseMenu
    {
        public static void Show(Quest quest, MurderCase murderCase, IQuestRepository questRepo)
        {
            bool loop = true;

            while (loop && quest.Status == QuestStatus.Accepted)
            {
                Console.Clear();
                TitleArt.Draw();
                Console.WriteLine($"Case: {murderCase.Title}");
                Console.WriteLine($"Status: {quest.Status}");
                Console.WriteLine();
                Console.WriteLine(murderCase.ShortSummary);
                Console.WriteLine();
                Console.WriteLine("What ?");
                Console.WriteLine("1) 🔍 Examine the crime scene");
                Console.WriteLine("2) 🕵️‍♂️ Question suspects");
                Console.WriteLine("3) 🕰️ Review clues");
                Console.WriteLine("4) ⚖️ Accuse a suspect");
                Console.WriteLine("5) 🕯️ Return to your desk");

                Console.Write("> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowCrimeScene(murderCase);
                        break;
                    case "2":
                        QuestionSuspects(murderCase);
                        break;
                    case "3":
                        ShowClues(murderCase);
                        break;
                    case "4":
                        Accuse(murderCase, quest, questRepo);
                        loop = false; // fallet avslutas efter anklagelse
                        break;
                    case "5":
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
                Console.Clear();
                Console.WriteLine("Suspects:");
                for (int i = 0; i < c.Suspects.Count; i++)
                {
                    var s = c.Suspects[i];
                    Console.WriteLine($"{i + 1}) {s.Name} ({s.Label})");
                }
                Console.WriteLine("0) Back");
                Console.Write("> ");

                var input = Console.ReadLine();
                if (!int.TryParse(input, out var choice) || choice == 0)
                    return;

                int index = choice - 1;
                if (index < 0 || index >= c.Suspects.Count)
                    continue;

                var suspect = c.Suspects[index];
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
            Console.Clear();
            Console.WriteLine("Who do you accuse?");
            for (int i = 0; i < c.Suspects.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {c.Suspects[i].Name} ({c.Suspects[i].Label})");
            }
            Console.Write("> ");

            var input = Console.ReadLine();
            if (!int.TryParse(input, out var choice))
                return;

            int guessedIndex = choice - 1;

            Console.Clear();
            if (guessedIndex == c.KillerIndex)
            {
                Console.WriteLine("You have solved the case, detective.");
            }
            else
            {
                Console.WriteLine("That was not the right suspect. Scotland Yard will take it from here.");
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
