
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
}
