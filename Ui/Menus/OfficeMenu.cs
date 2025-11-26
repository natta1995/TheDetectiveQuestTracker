using System;
using System.Linq;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;
using TheDetectiveQuestTracker.Ui.Components;
using TheDetectiveQuestTracker.Ui.TheDetectiveQuestTracker.UI;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class OfficeMenu
    {
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
                        "🤵‍♂️ Call on your butler (George)",
                        "📻 Turn on the the wireless",
                        "🏅 Solved cases",
                        "🚪 Leave office",
                    },
                    startIndex: 0
                );

                switch (selection)
                {
                    case 0: // New case
                        Console.Clear();
                        TitleArt.Draw();

                        // 1. Hämta ALLA hårdkodade fall
                        var allmurderCases = gen.GetAllCases();

                        // 2. Hämta alla quests för den här användaren
                        var allUsermurderQuests = questRepo.GetForUser(currentUser.Username).ToList();

                        // 3. Filtrera fram fall som INTE redan har ett quest för den här användaren
                        //    (detta är i praktiken dina "Available" fall)
                        var availableCases = allmurderCases
                            .Where(c => !allUsermurderQuests.Any(q => q.CaseId == c.Id))
                            .ToList();

                        if (!availableCases.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("You have already taken on all available cases.");
                            Console.ResetColor();
                            ConsoleHelpers.Pause();
                            break;
                        }

                        // 4. Bygg meny med de tillgängliga fallen
                        var caseOptions = availableCases
                            .Select(c => $" ☠️ {c.Title} Priority: {c.Priority}")
                            .ToList();

                        caseOptions.Add("⬅ Back");

                        var chosenIndex = ConsoleMenu.Select(
                            title: "Which case file would you like to take on?",
                            options: caseOptions.ToArray(),
                            startIndex: 0
                        );

                        if (chosenIndex == -1 || chosenIndex == caseOptions.Count - 1)
                            break; // back / escape

                        var chosenCase = availableCases[chosenIndex];

                        // 5. Skapa ett quest för JUST det här fallet
                        var newQuest = gen.GenerateFor(currentUser, chosenCase);
                        // 🕒 Sätt tid baserat på prioriteten på fallet
                        var now = DateTime.Now;

                        TimeSpan limit = chosenCase.Priority switch
                        {
                            CasePriority.Low => TimeSpan.FromHours(72),
                            CasePriority.Medium => TimeSpan.FromHours(48),
                            CasePriority.High => TimeSpan.FromHours(24),
                            _ => TimeSpan.FromHours(48)
                        };

                        newQuest.AcceptedAt = now;
                        newQuest.ExpiresAt = now.Add(limit);

                        questRepo.Add(newQuest);
                        


                        Console.Clear();
                        TitleArt.Draw();
                        Console.WriteLine("\nA new case file has been added to your ongoing cases.\n");
                        Console.WriteLine($"Title: {newQuest.Title}");
                        Console.WriteLine($"Deadline: {newQuest.ExpiresAt}");
                        Console.WriteLine(newQuest.Description);
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
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("You don’t have any active cases yet.");
                            Console.ResetColor();
                            ConsoleHelpers.Pause();
                            break;
                        }
 

                        var options = my.Select(q => $" ☠️ {q.Title}").ToList();
                        options.Add("⬅ Back");

                        var selectedIndex = ConsoleMenu.Select(
                            title: $"Which case would you like to review? \n Ongoing cases: {my.Count}",
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

                        CrimeSceneMenu.Show(selectedQuest, selectedCase, questRepo);
                        break;

                    case 2: // Butler
                        Console.Clear();
                        TitleArt.Draw();
                        var msg = ButlerDialogue.GetRandomMessage();
                        Console.WriteLine($"🤵‍♂️ George: {msg}");
                        ConsoleHelpers.Pause();
                        break;
                    case 3: // Turn on the wireless
                        Console.Clear();
                        TitleArt.Draw();
                        RadioPlayer.PlayRadio();
                        ConsoleHelpers.Pause();
                        break;
                  
                    case 4:
                        var allUserQuests = questRepo.GetForUser(currentUser.Username);

                        var solvedQuests = allUserQuests
                            .Where(q => q.Status == QuestStatus.Completed &&
                                        q.Result == QuestResult.Solved)
                            .ToList();

                        Console.Clear();
                        Console.WriteLine("Solved cases:");
                        Console.WriteLine("-------------");

                        if (solvedQuests.Count == 0)
                        {
                            Console.WriteLine("You haven't solved any cases yet.");
                        }
                        else
                        {
                            foreach (var q in solvedQuests)
                            {
                                Console.WriteLine($"- {q.Title}");
                            }
                        }

                        Console.WriteLine();
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey(true);

                        break;
                    case 5: // Leave office
                    case -1: // Escape
                        loop = false;
                        break;
                }
            }
        }
    }
}

