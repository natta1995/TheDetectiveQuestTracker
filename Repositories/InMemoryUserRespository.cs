
using System;
using System.Collections.Generic;
using System.Linq;
using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Repositories
{
    // === USER REPO ===
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void Create(User user) => _users.Add(user);

        public User? FindByUsername(string username)
            => _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public void Update(User user) { /* inget behövs för InMemory */ }
    }

    // === QUEST REPO ===
    public interface IQuestRepository
    {
        void Add(Quest q);

        void Update(Quest q);
        IEnumerable<Quest> GetForUser(string username);
    }

    public class InMemoryQuestRepository : IQuestRepository
    {
        private readonly List<Quest> _items = new();

        public void Add(Quest q) => _items.Add(q);

        public IEnumerable<Quest> GetForUser(string username)
            => _items.Where(x => x.OwnerUsername == username);

        public void Update(Quest q)
        {
            // För in-memory behöver du egentligen inte göra någonting,
            // eftersom du jobbar på samma referensobjekt.
            // Men vi har metoden för att API:t ska vara tydligt.
            var existing = _items.FirstOrDefault(x => x.Id == q.Id);
            if (existing is null) return;

            existing.Title = q.Title;
            existing.Description = q.Description;
            existing.OwnerUsername = q.OwnerUsername;
            existing.Status = q.Status;
            // Lägg till fler fält här om du senare utökar Quest
        }
    }

}
