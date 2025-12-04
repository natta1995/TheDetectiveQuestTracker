using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Ui.Components;

namespace TheDetectiveQuestTracker.Ui
{
    internal class IntroScreen
    {
        public static void Show()
        {
            Console.Clear();
            TitleArt.Draw();

            Console.WriteLine("Enter a world of mystery in a rain-soaked London in 1944");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("Loading game");

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(600);
                Console.Write(".");
            }
            Console.ResetColor();
            Console.WriteLine("\n");
            ConsoleHelpers.Pause();
        }
    }
}
