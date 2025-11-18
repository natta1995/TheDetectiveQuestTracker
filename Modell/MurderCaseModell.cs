using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheDetectiveQuestTracker.Modell
{
    public class Suspect // Klass för misstänka
    {
        public string Name { get; set; } = "";       // Namn (ex Jonas Andersson)
        public string Label { get; set; } = "";      // Relation till offret (ex. "butlern" eller "brorsonen")
        public string Statement { get; set; } = "";  // Försvarstal el albi
    }

    public class MurderCase // Klass för själva mordfallet
    {
        public string Id { get; set; } = "";         
        public string Title { get; set; } = "";  // Namn på mordfallet
        public string ShortSummary { get; set; } = ""; // Kort beskrivning som visas i listan med mordfall

        public string Victim { get; set; } = ""; // Namn på offret
        public string Place { get; set; } = ""; // Plats för mordet
        public string CauseOfDeath { get; set; } = ""; // Dödsorsak
        public string Weapon { get; set; } = ""; // Mordvapen

        public string CrimeSceneDescription { get; set; } = ""; // Beskrivning av brottsplatsen

        public List<Suspect> Suspects { get; set; } = new(); // Lista på misstänkta
        public List<string> Clues { get; set; } = new();      // Lista på ledtrådar som kan hittas

        public int KillerIndex { get; set; } = -1;       // Indexet i Suspects-listan för mördaren
        public string SolutionText { get; set; } = "";   // Upplösning av fallet
    }
}
