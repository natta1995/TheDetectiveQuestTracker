// UI/ConsoleMenu.cs
using System;

namespace TheDetectiveQuestTracker.UI
{
    public static class ConsoleMenu
    {
        /// <summary>
        /// Visa en vertikal meny där du navigerar med ↑/↓ och väljer med Enter.
        /// Returnerar valt index, eller -1 om användaren trycker Escape.
        /// </summary>
        public static int Select(string title, string[] options, int startIndex = 0, bool wrap = true, bool drawTitleArt = true)
        {
            if (options is null || options.Length == 0)
                throw new ArgumentException("Menu options cannot be empty.", nameof(options));

            int index = Math.Clamp(startIndex, 0, options.Length - 1);
            bool running = true;

            var prevCursorVisible = Console.CursorVisible;
            Console.CursorVisible = false;

            try
            {
                while (running)
                {
                    Console.Clear();
                    if (drawTitleArt)
                        TitleArt.Draw();

                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine(title);
                        Console.WriteLine();
                    }

                    // Rita alla rader (markerad rad inverterad)
                    for (int i = 0; i < options.Length; i++)
                    {
                        if (i == index) Invert(() => Console.WriteLine($"> {options[i]}"));
                        else Console.WriteLine($"  {options[i]}");
                    }

                    // Läs tangent
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            index = (index - 1 < 0) ? (wrap ? options.Length - 1 : 0) : index - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            index = (index + 1 >= options.Length) ? (wrap ? 0 : options.Length - 1) : index + 1;
                            break;
                        case ConsoleKey.Enter:
                            return index;
                        case ConsoleKey.Escape:
                            return -1;
                        default:
                            // Snabbval 1..9 (frivilligt)
                            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
                            {
                                int quick = (int)key - (int)ConsoleKey.D1; // 0-baserat
                                if (quick < options.Length) return quick;
                            }
                            break;
                    }
                }
            }
            finally
            {
                Console.CursorVisible = prevCursorVisible;
            }

            return index;

            static void Invert(Action write)
            {
                var fg = Console.ForegroundColor;
                var bg = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                write();
                Console.ForegroundColor = fg;
                Console.BackgroundColor = bg;
            }
        }
    }
}
