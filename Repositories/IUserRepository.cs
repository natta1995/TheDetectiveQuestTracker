using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Repositories
{
    public interface IUserRepository
    {
        void Create(User user);
        User? FindByUsername(string username);
        void Update(User user);
    }
}
