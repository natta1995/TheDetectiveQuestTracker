
// UI/Menus/LoggedInMenu.cs
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;
using TheDetectiveQuestTracker.Ui.Components;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class LoggedInMenu
    {
        public static bool Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator questGen, out User? nextUser)
        {
            
            nextUser = currentUser;
            Console.Write("\u001b[3J");
            Console.Clear();

            var selection = ConsoleMenu.Select(
                title: $"Welcome {currentUser.Username} 🕵️",
                options: new[]
                {
                    "📜 The Briefing",
                    "🚪 Enter the Study to Begin",
                    "🚶 Log out",
                    "🔚 End game"
                },
                startIndex: 1
            );

            switch (selection)
            {
                case 0:
                    Console.Clear();
                    TitleArt.Draw();
                    ConsoleUi.ShowBriefingPaged(currentUser.Username); // din befintliga
                    ConsoleHelpers.Pause();
                    return true;

                case 1:
                    OfficeMenu.Show(currentUser, questRepo, questGen);
                    return true;

                case 2:
                    nextUser = null;
                    Console.WriteLine("Utloggad.");
                    ConsoleHelpers.Pause();
                    return true;

                case 3:
                case -1:
                    return false;

                default:
                    return true;
            }
        }
    }
}
