using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Modell
{
    // Så vi skapar klassen hur vi vill att User skall se ut aka vad som skall var i: 
    // Lista på vad som skall finnas i User

    // OBS att sätta tomma stränar för art undvika problem fungerar i enkla proejkt men inte i större kontrollerade projekt
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Guid = Global unique identifier (tänk personnumer) Om Id inte finns så skapar vi ett igenom Guid metod.
        public string Username { get; set; } = "";
        public string Password { get; set; } = ""; // OBS: SÄKERHET LÄGGS TILL SENARE !!!!!!!!!!!!!!! Typ måste ha ex 6 tecken, bokstav, siffra. + HASHning
        public string Emejl { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // DateTime är en datatyp som string eller int. .UtcKnow är världstid. Hade kunnat använda .Now för tiden i sverige, men kör mer korrekt med UtcNow
        public DateTime? LastLogin { get; set; } // ? kan vara att man aldrig loggat in därav kan det vara null
    }
}
