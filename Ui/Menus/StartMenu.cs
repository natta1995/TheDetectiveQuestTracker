// UI/Menus/LoggedOutMenu.cs
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Services;
using TheDetectiveQuestTracker.Ui.Components;

namespace TheDetectiveQuestTracker.UI.Menus
{
    internal static class LoggedOutMenu
    {
        public static bool Show(Auth auth, out User? currentUser)
        {
            currentUser = null;

            var selection = ConsoleMenu.Select(
                title: " Enter a world of mystery in rain-soaked London in 1944",
                options: new[]
                {
                    "🕵️ Register",
                    "🚪 Login",
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
            Console.WriteLine("🕵️ Register\n");

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
            Console.WriteLine("🚪 Login \n");

            Console.Write("Alias: ");
            var u = ConsoleHelpers.ReadOrEmpty();

            Console.Write("Code: ");
            var p = ConsoleHelpers.ReadOrEmpty();

            Console.Clear();
            var (ok, user, msg) = auth.Login(u, p);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ResetColor();
            return ok ? user : null;
        }
    }
}
