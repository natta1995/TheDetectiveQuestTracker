

namespace TheDetectiveQuestTracker.Ui.Components
{
    public static class ConsoleMenu
    {
        public static int Select(string title, string[] options, int startIndex = 0, bool wrap = true, bool drawTitleArt = false)
        {
            if (options is null || options.Length == 0)
                throw new ArgumentException("Menu options cannot be empty.", nameof(options));

            int index = Math.Clamp(startIndex, 0, options.Length - 1);
            bool running = true;

            var prevCursorVisible = Console.CursorVisible;
            Console.CursorVisible = false;

            try
            {
                Console.Clear(); // rensa EN gång i början
                Console.ForegroundColor = ConsoleColor.Yellow;
               

                if (drawTitleArt)
                    TitleArt.Draw();

                if (!string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine(title);
                    Console.WriteLine();
                }

                // Här börjar själva menyn – kom ihåg denna Y-position
                int menuTop = Console.CursorTop;

                while (running)
                {
                    // Flytta tillbaka till början av menyn
                    Console.SetCursorPosition(0, menuTop);

                    // Rita alla rader (markerad rad inverterad)
                    for (int i = 0; i < options.Length; i++)
                    {
                        var row = menuTop + i;

                        // Skydda mot att gå utanför bufferten (annars kraschar SetCursorPosition)
                        if (row >= Console.BufferHeight)
                            break; // eller continue; men break räcker – fler rader får inte plats ändå

                        // Ställ markören på rätt rad
                        Console.SetCursorPosition(0, row);

                        // Rensa hela raden
                        Console.Write(new string(' ', Console.WindowWidth));

                        // Flytta tillbaka till början av raden
                        Console.SetCursorPosition(0, row);

                        // Skriv valet
                        if (i == index)
                            Invert(() => Console.WriteLine($"> {options[i]}"));
                        else
                            Console.WriteLine($"  {options[i]}");
                    }


                    // Läs tangent
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            index = index - 1 < 0 ? wrap ? options.Length - 1 : 0 : index - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            index = index + 1 >= options.Length ? wrap ? 0 : options.Length - 1 : index + 1;
                            break;
                        case ConsoleKey.Enter:
                            return index;
                        case ConsoleKey.Escape:
                            return -1;
                        default:
                            // Snabbval 1..9
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

            static void Invert(Action yellow)
            {
                var fg = Console.ForegroundColor;
                var bg = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                yellow();
                Console.ForegroundColor = fg;
                Console.BackgroundColor = bg;
            }
        }


    }
}
