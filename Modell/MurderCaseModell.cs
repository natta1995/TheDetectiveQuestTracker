using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheDetectiveQuestTracker.Modell
{
    public class Suspect
    {
        public string Name { get; set; } = "";
        public string Label { get; set; } = "";      // t.ex. "butlern", "brorsonen"
        public string Statement { get; set; } = "";  // vad de säger
    }

    public class MurderCase
    {
        public string Id { get; set; } = "";         // t.ex. "mr_club"
        public string Title { get; set; } = "";
        public string ShortSummary { get; set; } = ""; // kort intro-text

        public string Victim { get; set; } = "";
        public string Place { get; set; } = "";
        public string CauseOfDeath { get; set; } = "";
        public string Weapon { get; set; } = "";

        public string CrimeSceneDescription { get; set; } = "";

        public List<Suspect> Suspects { get; set; } = new();
        public List<string> Clues { get; set; } = new();

        public int KillerIndex { get; set; } = -1;       // vilken misstänkt som är skyldig
        public string SolutionText { get; set; } = "";   // visas när fallet avslutas
    }
}
