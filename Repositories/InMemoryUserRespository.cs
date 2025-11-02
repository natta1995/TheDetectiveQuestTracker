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

}
