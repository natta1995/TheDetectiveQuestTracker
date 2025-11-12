
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus;

internal static class LoggedOutMenu
{
    public static bool Show(Auth auth, out User? currentUser)
    {
        currentUser = null;

        ConsoleHelpers.ClearWithTitle();
        Console.WriteLine(" Welcome to The Secret Society");
        Console.WriteLine();
        Console.WriteLine("1) 🕵️ Become a member");
        Console.WriteLine("2) 🚪 Enter the society");
        Console.WriteLine();
        Console.WriteLine("0) 🔚 End game");
        Console.Write("Val: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                RegisterFlow(auth);
                ConsoleHelpers.Pause();
                return true;

            case "2":
                currentUser = LoginFlow(auth);
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

    private static void RegisterFlow(Auth auth)
    {
        Console.Write("Alias: ");
        var u = ConsoleHelpers.ReadOrEmpty();

        Console.Write("Code: ");
        var p = ConsoleHelpers.ReadOrEmpty();

        Console.Write("Email (valfritt): ");
        var e = ConsoleHelpers.ReadOrEmpty();

        Console.Write("Phone (valfritt): ");
        var ph = ConsoleHelpers.ReadOrEmpty();

        var (ok, msg) = auth.Register(u, p, e, ph);
        Console.WriteLine(msg);
    }

    private static User? LoginFlow(Auth auth)
    {
        Console.Write("Alias: ");
        var u = ConsoleHelpers.ReadOrEmpty();

        Console.Write("Code: ");
        var p = ConsoleHelpers.ReadOrEmpty();

        var (ok, user, msg) = auth.Login(u, p);
        Console.WriteLine(msg);
        return ok ? user : null;
    }
}
