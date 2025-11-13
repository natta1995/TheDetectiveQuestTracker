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

    public class Quest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? OwnerUsername { get; set; }
        public QuestStatus Status { get; set; } = QuestStatus.Available;

        // Viktig ny grej: koppling till själva mordfallet
        public string? CaseId { get; set; }
    }
}
