namespace TheDetectiveQuestTracker.Modell
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // (Guid = Global unique identifier (tänk personnumer) Om Id inte finns så skapar vi ett igenom Guid metod.)
        public string Username { get; set; } = "";
        public string Password { get; set; } = ""; 
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // (DateTime är en datatyp som string eller int. .UtcKnow är världstid. Hade kunnat använda .Now för tiden i sverige, men kör mer korrekt med UtcNow.)
        public DateTime? LastLogin { get; set; } 
    }
}
