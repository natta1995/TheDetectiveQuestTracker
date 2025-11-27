namespace TheDetectiveQuestTracker.Modell
{
    public enum QuestStatus 
    {
        Available, 
        Accepted,   
        Completed  
    }

    public enum QuestResult 
    {
        None,       
        Solved,    
        Failed      
    }

    public class Quest 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? OwnerUsername { get; set; } 
        public QuestStatus Status { get; set; } = QuestStatus.Available; 

        // Koppla fallet till questen
        public string? CaseId { get; set; }

        public QuestResult Result { get; set; } = QuestResult.None; // Se ovan

        public DateTime? AcceptedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public bool Reminder20hSent { get; set; } = false;
        public bool Reminder1hSent { get; set; } = false;

        // Liten hjälp-property om du vill kolla om det gått ut
        public bool IsExpired => ExpiresAt.HasValue && DateTime.Now > ExpiresAt.Value;


    }
}
