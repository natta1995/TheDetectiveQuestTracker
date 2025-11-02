using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Services
{
    public class MurderQuestGenerator
    {
        private readonly Random _rng = new();
        private readonly string[] victims = { "Greve Lind", "Dr. Åkesson", "Operasångerskan Vega" };
        private readonly string[] places = { "Gamla Stan", "Observatoriet", "Hamnmagasinet" };

        public Quest Generate()
        {
            var v = victims[_rng.Next(victims.Length)];
            var p = places[_rng.Next(places.Length)];
            return new Quest
            {
                Title = $"Mordet på {v}",
                Description = $"Offer: {v}\nPlats: {p}\nUppgift: Lös fallet.",
                Status = QuestStatus.Available
            };
        }
    }
}
