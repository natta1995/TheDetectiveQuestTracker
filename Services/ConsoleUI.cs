using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Services
{
    public static class ConsoleUi
    {
        
        public static void ShowBriefingPaged(string username)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;

            
            var lines = new List<string>
            {
"┌────────────────────────────────────────────────────────────────────────────────────────────────────────────┐",
"│                                                                                                            │",
"│   LONDON, 1944                                                                                             │",
"│   Confidential Briefing — The Secret Society of Detectives                                                 │",
"│                                                                                                            │",
"│   The year is 1944. London lies under the shadow of war. The skies rumble, bombs fall —                    │",
"│   yet in the city’s dark alleys, ordinary crimes never stopped.                                            │",
"│                                                                                                            │",
"│   Scotland Yard is overwhelmed. Too many cases. Too few who dare look the truth in the eye.                │",
"│   That’s why we’ve turned to The Secret Society of Detectives — a group that works in the shadows          │",
"│   when the police cannot.                                                                                  │",
"│                                                                                                            │",
"│   Perhaps you’ll meet the others. Perhaps not.                                                             │",
"│   The streets of London are quiet… too quiet.                                                              │",
"│                                                                                                            │",
"│   But murders still happen. And someone has to solve them.                                                 │",
"│                                                                                                            │",
"│   Your identity will remain hidden — if you choose to walk away.                                           │",
"│   But once you open this case, there’s no turning back.                                                    │",
"│                                                                                                            │",
$"│   Fade into the fog... or open your first murder file, Detective {username}.                                     │",
"│                                                                                                            │",
"└────────────────────────────────────────────────────────────────────────────────────────────────────────────┘",
            };

            foreach (var line in lines)
                Console.WriteLine(line);


            {
                Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Press any key to return…");
                    Console.ResetColor();
                    Console.ReadKey(true);
                }
            }
        }
    }

