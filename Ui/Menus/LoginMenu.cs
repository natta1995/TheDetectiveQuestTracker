
// UI/Menus/LoggedInMenu.cs
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class LoggedInMenu
    {
        public static bool Show(User currentUser, IQuestRepository questRepo, MurderQuestGenerator questGen, out User? nextUser)
        {
            nextUser = currentUser;

            var selection = ConsoleMenu.Select(
                title: $"Welcome, Detective {currentUser.Username} 🕵️\nWe’ve been waiting for you...",
                options: new[]
                {
                    "📜 Open your letter",
                    "🚪 Step into yor office",
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
                    QuestMenu.Show(currentUser, questRepo, questGen);
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
