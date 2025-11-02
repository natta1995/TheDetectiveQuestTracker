using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;

namespace TheDetectiveQuestTracker.Services
{
    public class Auth
    {
        private readonly IUserRepository _repo;

        public Auth(IUserRepository repo)
        {
            _repo = repo;
        }

        // Registrera ny användare
        public (bool ok, string msg) Register(string username, string password, string email, string phone)
        {
            // 1) Grundkoll
            if (string.IsNullOrWhiteSpace(username)) return (false, "Alias is required.");
            if (string.IsNullOrWhiteSpace(password)) return (false, "Code is required.");

            // 2) Lösenordspolicy (enkel)
            var (valid, policyMsg) = ValidatePassword(password);
            if (!valid) return (false, policyMsg);

            // 3) Finns användarnamnet redan?
            var existing = _repo.FindByUsername(username);
            if (existing != null) return (false, "Användarnamnet är upptaget.");

            // 4) Krävs minst email eller telefon
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                return (false, "Ange minst e-post eller telefonnummer.");

            // 5) Skapa och spara användare
            var user = new User
            {
                Username = username.Trim(),
                Password = password,            // TODO: hasha i nästa steg
                Emejl = (email ?? "").Trim(),
                PhoneNumber = (phone ?? "").Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _repo.Create(user);
            return (true, $"Registrering lyckades! Välkommen, {user.Username}.");
        }

        // Logga in
        public (bool ok, User? user, string msg) Login(string username, string password)
        {
            var user = _repo.FindByUsername(username);
            if (user == null || user.Password != password)
                return (false, null, "Fel användarnamn eller lösenord.");

            user.LastLogin = DateTime.UtcNow;
            _repo.Update(user);

            return (true, user, $"Welcome ... detective, {user.Username}");
        }

        // (Valfri) Logout – här kan du bara nolla ev. session om du använder en
        public void Logout()
        {
            // Om du senare har en SessionContext: SessionContext.CurrentUser = null;
        }

        // -------------------------
        // Enkel lösenordspolicy
        // -------------------------
        private static (bool ok, string msg) ValidatePassword(string password)
        {
            if (password.Length < 6) return (false, "Minst 6 tecken.");
            if (!Regex.IsMatch(password, "[A-Z]")) return (false, "Minst en versal (A–Z).");
            if (!Regex.IsMatch(password, "\\d")) return (false, "Minst en siffra (0–9).");
            if (!Regex.IsMatch(password, "[\\W_]")) return (false, "Minst ett specialtecken.");
            return (true, "OK");
        }
    }
}

