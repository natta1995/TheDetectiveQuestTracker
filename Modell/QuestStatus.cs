using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Modell
{
    public enum QuestStatus
    {
        Available,
        Accepted,
        Completed // lägg till den
    }

    public enum QuestResult
    {
        None,       // inget resultat än (om status inte är Completed)
        Solved,     // spelaren anklagade rätt person
        Failed      // spelaren anklagade fel person
    }
    public class Quest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? OwnerUsername { get; set; }
        public QuestStatus Status { get; set; } = QuestStatus.Available;

        // Koppla till caset
        public string? CaseId { get; set; }

        // Nytt: hur det gick
        public QuestResult Result { get; set; } = QuestResult.None;

    
    }
}
