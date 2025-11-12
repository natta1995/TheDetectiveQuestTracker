using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;


namespace TheDetectiveQuestTracker.UI.Menus;

internal static class LoggedInMenu
{
    public static bool Show(
        User currentUser,
        IQuestRepository questRepo,
        MurderQuestGenerator questGen,
        out User? nextUser)
    {
        nextUser = currentUser;

        ConsoleHelpers.ClearWithTitle();
        Console.WriteLine($"Welcome, Detective {currentUser.Username} 🕵️");
        Console.WriteLine("We’ve been waiting for you...\n");
        Console.WriteLine("10) 📜 Open your letter");
        Console.WriteLine("1)  🚪 Step into you office");
        Console.WriteLine("9)  🚶 Step out of building");
        Console.WriteLine();
        Console.WriteLine("0) 🔚 Exit");
        Console.Write("Val: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                QuestMenu.Show(currentUser, questRepo, questGen);
                return true;

            case "9":
                nextUser = null;
                Console.WriteLine("Utloggad.");
                ConsoleHelpers.Pause();
                return true;

            case "10":
                ConsoleUi.ShowBriefingPaged(currentUser.Username); // din befintliga UI-funktion
                ConsoleHelpers.Pause();
                return true;

            case "0":
                return false;

            default:
                Console.WriteLine("Invalid choice.");
                ConsoleHelpers.Pause();
                return true;
        }
    }
}
