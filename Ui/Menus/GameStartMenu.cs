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
                title: $"[Location: Your Flat – Foyer ] \n\nA warm, well-appointed foyer with polished parquet floors \nand a brass valet stand, where your rain-soaked coat now hangs.\n",
                options: new[]
                {
                    "✉️ Read the letter on the hall table",
                    "🚪 Enter your office",
                    "🚶 Log out",
                    "🔚 Exit game"
                },
                startIndex: 1,
                drawTitleArt: false
            );

            switch (selection)
            {
                case 0:
                    Console.Clear();
                
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
