using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Modell
{
    public enum QuestStatus // Vilken status questen har
    {
        Available, // tillgänglig för att accepteras
        Accepted,   // accepterad av spelaren
        Completed  // Avslutat fall 
    }

    public enum QuestResult // Hur det gick med questen
    {
        None,       // Inget resultat - inte anklagat någon ännu
        Solved,     // spelaren anklagade RÄTT person
        Failed      // spelaren anklagade FEl person
    }
    public class Quest // Själva quest-objektet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? OwnerUsername { get; set; } // Vem som äger questen
        public QuestStatus Status { get; set; } = QuestStatus.Available; // Se ovan

        // Koppla fallet till questen
        public string? CaseId { get; set; }

        public QuestResult Result { get; set; } = QuestResult.None; // Se ovan

    
    }
}
