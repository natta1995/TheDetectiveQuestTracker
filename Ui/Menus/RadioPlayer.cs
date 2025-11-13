using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Ui
{
    using System;
    using System.Threading;

    namespace TheDetectiveQuestTracker.UI
    {
        internal static class RadioPlayer
        {
            public static void PlayRadio()
            {
                Console.Clear();

               
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
                Console.WriteLine("The wireless crackles to life...\n");

                // Lite 'static' + en enkel radio-känsla
                PlayStatic();
                

                Console.WriteLine();
                Console.WriteLine("\"Good evening. This is the BBC Home Service, London...\"");
                Console.WriteLine();
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
