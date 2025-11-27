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
