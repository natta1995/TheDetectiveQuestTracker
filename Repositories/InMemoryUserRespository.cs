using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void Create(User user) => _users.Add(user);

        public User? FindByUsername(string username)
            => _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public void Update(User user) { /* inget behövs för InMemory */ }
    }

    namespace TheDetectiveQuestTracker.Repositories
    {
        public interface IQuestRepository
        {
            void Add(Quest q);
            IEnumerable<Quest> GetForUser(string username);
        }

        public class InMemoryQuestRepository : IQuestRepository
        {
            private readonly List<Quest> _items = new();

            public void Add(Quest q) => _items.Add(q);

            public IEnumerable<Quest> GetForUser(string username)
                => _items.Where(x => x.OwnerUsername == username);
        }
    }

}
