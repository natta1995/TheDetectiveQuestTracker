
using System;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;
using TheDetectiveQuestTracker.Services;
using TheDetectiveQuestTracker.UI.Menus;

namespace TheDetectiveQuestTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
           

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Composition root
            IUserRepository userRepo = new JsonFileUserRepository();
            IQuestRepository questRepo = new JsonFileQuestRepository(); // eller InMemoryQuestRepository
            var auth = new Auth(userRepo);
            var questGen = new MurderQuestGenerator();

            User? currentUser = null;
            bool running = true;

            while (running)
            {
                if (currentUser is null)
                    running = StartMenu.Show(auth, out currentUser);
                else
                    running = LoggedInMenu.Show(currentUser, questRepo, questGen, out currentUser);
            }
        }
    }
}
