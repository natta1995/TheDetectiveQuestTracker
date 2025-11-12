// UI/Menus/LoggedOutMenu.cs
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Services;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class LoggedOutMenu
    {
        public static bool Show(Auth auth, out User? currentUser)
        {
            currentUser = null;

            var selection = ConsoleMenu.Select(
                title: " Welcome to The Secret Society",
                options: new[]
                {
                    "🕵️ Become a member",
                    "🚪 Enter the society",
                    "🔚 End game"
                },
                startIndex: 0
            );

            switch (selection)
            {
                case 0:
                    RegisterFlow(auth);
                    ConsoleHelpers.Pause();
                    return true;

                case 1:
                    currentUser = LoginFlow(auth);
                    ConsoleHelpers.Pause();
                    return true;

                case 2:
                case -1: // Escape
                    return false;

                default:
                    return true;
            }
        }

        private static void RegisterFlow(Auth auth)
        {
            Console.Clear();
            TitleArt.Draw();
            Console.WriteLine("🕵️ Become a member\n");

            Console.Write("Alias: ");
            var u = ConsoleHelpers.ReadOrEmpty();

            Console.Write("Code: ");
            var p = ConsoleHelpers.ReadOrEmpty();

            Console.Write("Email (valfritt): ");
            var e = ConsoleHelpers.ReadOrEmpty();

            Console.Write("Phone (valfritt): ");
            var ph = ConsoleHelpers.ReadOrEmpty();

            var (_, msg) = auth.Register(u, p, e, ph);
            Console.WriteLine(msg);
        }

        private static User? LoginFlow(Auth auth)
        {
            Console.Clear();
            TitleArt.Draw();
            Console.WriteLine("🚪 Enter the society\n");

            Console.Write("Alias: ");
            var u = ConsoleHelpers.ReadOrEmpty();

            Console.Write("Code: ");
            var p = ConsoleHelpers.ReadOrEmpty();

            var (ok, user, msg) = auth.Login(u, p);
            Console.WriteLine(msg);
            return ok ? user : null;
        }
    }
}
