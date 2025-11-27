using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace TheDetectiveQuestTracker.Ui.Components;

internal static class TitleArt
{
    private static readonly string Art = @"
┌────────────────────────────────────────────────────────────────────────────────────────────────────────────-┐
│                                                                                                             │
│   _____ _          ___      _          _   _            ___              _     _____            _           │
│  |_   _| |_  ___  |   \ ___| |_ ___ __| |_(_)_ _____   / _ \ _  _ ___ __| |_  |_   _| _ __ _ __| |_____ _ _ │
│    | | | ' \/ -_) | |) / -_)  _/ -_) _|  _| \ V / -_) | (_) | || / -_|_-<  _|   | || '_/ _` / _| / / -_) '_|│
│    |_| |_||_\___| |___/\___|\__\___\__|\__|_|\_/\___|  \__\_\\_,_\___/__/\__|   |_||_| \__,_\__|_\_\___|_|  │
│                                                                                                             │
└────────────────────────────────────────────────────────────────────────────────────────────────────────────-┘
";

    public static void Draw()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(Art);
        Console.ResetColor();
    }

 
}

