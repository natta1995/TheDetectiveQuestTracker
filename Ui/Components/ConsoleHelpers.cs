
using System;

namespace TheDetectiveQuestTracker.Ui.Components;

internal static class ConsoleHelpers
{
    public static void Pause(string? message = null)
    {
        Console.WriteLine();
        Console.Write(message ?? "Press any key to continue...");
        Console.ReadKey(true);
    }

    public static string ReadOrEmpty() => Console.ReadLine() ?? string.Empty;

    public static void ClearWithTitle()
    {
        //Console.Clear();
        //TitleArt.Draw();
    }

    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }

        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
