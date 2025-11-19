using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Ui
{
    using global::TheDetectiveQuestTracker.UI;
    using System;
    using System.Threading;

    namespace TheDetectiveQuestTracker.UI
    {
        internal static class RadioPlayer
        {
            public static void PlayRadio()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                


                string radioArt = @"
┌────────────────────────────────────────┐
│              WIRELESS SET              │
├────────────────────────────────────────┤
│   ┌──────────────────────────────┐     │
│   │ ~~~ sssshhh ~~~  •••  ~~~    │     │
│   │ ~~~ bzzzzt ~~~   ▓▓   ~~~    │     │
│   └──────────────────────────────┘     │
│                                        │
│   ◉               ○               ◉    │
│                                        │
└────────────────────────────────────────┘
";

                Console.WriteLine(radioArt);
                Console.ResetColor();

                PlayStatic();
                
                Console.WriteLine();
                Console.WriteLine("\"The rain keeps falling, as if even the sky has grown weary of a war that refuses to end…...\"");
                Console.WriteLine();
                PlayStatic();
                
            }

            private static void PlayStatic()
            {
                // Fake radio-static med slumpade tecken
                var random = new Random();
                for (int i = 0; i < 3; i++)
                {
                    string line = "";
                    for (int j = 0; j < 30; j++)
                    {
                        char c = random.Next(3) switch
                        {
                            0 => '.',
                            1 => '~',
                            _ => '#'
                        };
                        line += c;
                    }
                    Console.WriteLine("   " + line);
                    Thread.Sleep(150);
                }
            }

        }
    }

}
