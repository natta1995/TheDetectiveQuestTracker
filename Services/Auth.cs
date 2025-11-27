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
        public (bool ok, string msg) Register(string username, string password, string email)
        {
            // 1) Grundkoll
            if (string.IsNullOrWhiteSpace(username)) return (false, "Username is required.");
            if (string.IsNullOrWhiteSpace(password)) return (false, "Password is required.");

            // 2) Lösenordspolicy (enkel)
            var (valid, policyMsg) = ValidatePassword(password);
            if (!valid) return (false, policyMsg);

            // 3) Finns användarnamnet redan?
            var existing = _repo.FindByUsername(username);
            if (existing != null) return (false, "Username already exists");

            // 4) Krävs minst email eller telefon
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email is required");

            // 5) Skapa och spara användare
            var user = new User
            {
                Username = username.Trim(),
                Password = password,            // TODO: hasha i nästa steg
                Email = (email ?? "").Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _repo.Create(user);
            return (true, $"Registration successful! Welcome, {user.Username}.");
        }

        // Logga in
        public (bool ok, User? user, string msg) Login(string username, string password)
        {
            var user = _repo.FindByUsername(username);
            if (user == null || user.Password != password)
                return (false, null, "Wrong alias or Password.");

            user.LastLogin = DateTime.UtcNow;
            _repo.Update(user);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nLoading game");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(700); // väntar 0.7 sekunder
                Console.Write(".");
            }


            Console.Write($"\nDetective {username} verified");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(700); // väntar 0.7 sekunder
                Console.Write(".");
            }
            Console.ResetColor();

            return (true, user, "\n\n🏠🗝️ Entering your flat on Kensington Row.\n");

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
            if (password.Length < 6) return (false, "At least 6 characters.");
            if (!Regex.IsMatch(password, "[A-Z]")) return (false, "At least one uppercase letter (A–Z).");
            if (!Regex.IsMatch(password, "\\d")) return (false, "At least one digit (0–9).");
            if (!Regex.IsMatch(password, "[\\W_]")) return (false, "At least one special character.");
            return (true, "OK");

        }
    }
}

