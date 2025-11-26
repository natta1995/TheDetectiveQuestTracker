using System;
using System.Linq;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;
using TheDetectiveQuestTracker.Ui.Components;


namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class OfficeMenu

    {
        private static readonly Random _rng = new();

        private static readonly FakeAiCaseGenerator _aiGenerator = new();

        // 🔹 Lista med korta rubriker som Scotland Yard kan erbjuda
        private static readonly string[] _yardTitles =
        {
            "The Murder at the Grand Hotel",
            "The Boy in the Stables",
            "The Lady on the Bridge",
            "Shadows in the Alley",
            "Death at the Boarding House",
            "The Pianist in Room 12",
            "The Body by the Railway Arch",
            "The Widow on Baker Street"
            // lägg till fler här om du vill
        };

        // 🔹 Hjälpmetod: plocka ut två slumpmässiga, olika titlar
        private static (string first, string second) GetTwoRandomYardTitles()
        {
            if (_yardTitles.Length == 0)
                return ("Untitled case", "Another untitled case");

            if (_yardTitles.Length == 1)
                return (_yardTitles[0], _yardTitles[0]);

            int firstIndex = _rng.Next(_yardTitles.Length);
            int secondIndex;
            do
            {
                secondIndex = _rng.Next(_yardTitles.Length);
            }
            while (secondIndex == firstIndex);

            return (_yardTitles[firstIndex], _yardTitles[secondIndex]);
        }
        public static void Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator gen)
        {
            bool loop = true;

            while (loop)
            {
                var selection = ConsoleMenu.Select(
                    title: "🕵️ Study - Where would you like to start ",
                    options: new[]
                    {
                        "🔍 Take on a new case",
                        "📂 Rewiew your ongoing cases",
                        "🤵‍♂️ Call on your butler (George)",
                        "🏅 Gloat over solved cases",
                        "☎️ Call Scotland Yard (Commissioner Penwood)",
                        "🚪 Leave study",
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
                            Console.WriteLine("You have already taken on all available cases. \n Call your old friend Commissioner Arthur Penwood at Scotland Yard to get a new one");
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

                        // 1. Hämta alla quests för användaren
                        var allMyQuests = questRepo.GetForUser(currentUser.Username).ToList();

                        var timeNow = DateTime.Now;

                        // 2. Uppdatera de som har gått ut: Accepted + tidsgräns passerad -> Completed + Failed
                        foreach (var q in allMyQuests)
                        {
                            if (q.Status == QuestStatus.Accepted &&
                                q.ExpiresAt is not null &&
                                q.ExpiresAt <= timeNow &&
                                q.Result == QuestResult.None)
                            {
                                q.Status = QuestStatus.Completed;
                                q.Result = QuestResult.Failed;
                                questRepo.Update(q);
                            }
                        }

                        // 3. Bygg lista över pågående fall (Accepted) efter uppdateringen
                        var my = allMyQuests
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

                        // 4. Räkna akuta vs okej (baserat på ExpiresAt)
                        int urgentCount = 0;
                        int okCount = 0;

                        foreach (var q in my)
                        {
                            if (q.ExpiresAt is null)
                            {
                                okCount++; // inga tidsgränser -> räknas som "okej"
                                continue;
                            }

                            var remaining = q.ExpiresAt.Value - timeNow;

                            if (remaining.TotalSeconds <= 0)
                            {
                                // borde egentligen inte finnas kvar här eftersom vi just filtrerade,
                                // men vi kan ignorera dem
                                continue;
                            }

                            if (remaining.TotalHours <= 24)
                                urgentCount++;
                            else
                                okCount++;
                        }

                        // 5. Bygg texten som beskriver läget
                        string statusLine;
                        if (urgentCount == 0)
                        {
                            statusLine = "All your ongoing cases are under control.";
                        }
                        else if (urgentCount == 1 && okCount == 0)
                        {
                            statusLine = "One case is urgent.";
                        }
                        else if (urgentCount == 1 && okCount == 1)
                        {
                            statusLine = "One case is urgent, the other one is still under control.";
                        }
                        else if (urgentCount == 1)
                        {
                            statusLine = $"One case is urgent, the other {okCount} are still under control.";
                        }
                        else
                        {
                            statusLine = $"{urgentCount} cases are urgent, {okCount} are still under control.";
                        }

                        // 6. Bygg meny-alternativ med titlar + expiry-tid
                        var options = my.Select(q =>
                        {
                            string expiresText = q.ExpiresAt is not null
                                ? q.ExpiresAt.Value.ToString("yyyy-MM-dd HH:mm")
                                : "no deadline";

                            return $" ☠️ {q.Title}  (Expires at: {expiresText})";
                        }).ToList();

                        options.Add("⬅ Back");

                        // 7. Visa meny, med din extra status-text under antal fall
                        var selectedIndex = ConsoleMenu.Select(
                            title: $"Which case would you like to review? \n Ongoing cases: {my.Count}\n{statusLine}",
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
                  
                  
                    case 3:
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


                    case 4:
                    // Call Scotland Yard + AI
                        Console.Clear();
                        TitleArt.Draw();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Calling Scotland Yard...");
                        Console.ResetColor();

                        ConsoleHelpers.Pause();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n\n👮 \"Ah, detective {currentUser.Username}, are you up for another mystery?\"");
                        Console.ResetColor();

                        ConsoleHelpers.Pause();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\n👮 \"My desk is overflowing with files, as you know. I’m more than grateful for your help.\"");
                        Console.ResetColor();

                        ConsoleHelpers.Pause();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\n👮 \"Let me see what files I have here...\"");
                        Console.ResetColor();

                        ConsoleHelpers.Pause();

                        // 1. Hämta två slumpade rubriker
                        var (title1, title2) = GetTwoRandomYardTitles();

                        var offerOptions = new[]
                        {
        title1,
        title2,
        "⬅ Hang up"
    };

                        var offerIndex = ConsoleMenu.Select(
                            title: "👮 \"Which of these cases would you consider, sir?\"",
                            options: offerOptions,
                            startIndex: 0
                        );

                        if (offerIndex == -1 || offerIndex == offerOptions.Length - 1)
                        {
                            Console.WriteLine("\n👮 \"Very well, sir. Perhaps another time.\" *click*");
                            ConsoleHelpers.Pause();
                            break;
                        }

                        var chosenTitle = offerOptions[offerIndex];

                        Console.WriteLine($"\n👮 \"Excellent. I’ll have the file on '{chosenTitle}' sent to your office immediately.\"");
                        ConsoleHelpers.Pause();

                        // 2. Generera AI-fall utifrån vald titel
                        Console.WriteLine("\n(Commissioner Penwood is dictating details to the clerks at Scotland Yard...)");
                        ConsoleHelpers.Pause();

                        MurderCase aiCase;

                        try
                        {
                            aiCase = _aiGenerator.GenerateCaseFromTitle(chosenTitle);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nSomething went wrong while generating the case: {ex.Message}");
                            Console.ResetColor();
                            ConsoleHelpers.Pause();
                            break;
                        }

                        if (aiCase == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nI am terribly sorry, sir. Scotland Yard failed to send a proper case file.");
                            Console.ResetColor();
                            ConsoleHelpers.Pause();
                            break;
                        }

                        // 3. Lägg till fallet i MurderCases så att det dyker upp bland tillgängliga fall
                        MurderCases.Add(aiCase);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nA new case has been added to your archives:\n  {aiCase.Title}");
                        Console.ResetColor();
                        Console.WriteLine("You can now take it on from '🔍 Take on a new case'.");
                        ConsoleHelpers.Pause();
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

