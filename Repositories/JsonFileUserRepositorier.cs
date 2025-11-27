using System.Text.Json;
using TheDetectiveQuestTracker.Modell;


namespace TheDetectiveQuestTracker.Repositories
{
    public class JsonFileUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };
        private readonly object _lock = new();
        private List<User> _users = new();

        public JsonFileUserRepository(string? filePath = null)
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TheDetectiveQuestTracker");
            Directory.CreateDirectory(dir);
            _filePath = filePath ?? Path.Combine(dir, "users.json");
            Load();
        }

        public void Create(User user)
        {
            lock (_lock)
            {
                if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("The alias is already taken.");
                _users.Add(user);
                Save();
            }
        }

        public User? FindByUsername(string username)
        {
            lock (_lock)
                return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(User user)
        {
            // Om du ändrar något på en user-instans: bara spara hela listan.
            lock (_lock) Save();
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_filePath)) { _users = new(); return; }
                var json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json, _opts) ?? new();
            }
            catch
            {
                TryBackup();
                _users = new();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_users, _opts);
            var tmp = _filePath + ".tmp";
            File.WriteAllText(tmp, json);
            if (File.Exists(_filePath)) File.Delete(_filePath);
            File.Move(tmp, _filePath);
        }

        private void TryBackup()
        {
            try
            {
                var bak = _filePath + ".bak-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                if (File.Exists(_filePath)) File.Copy(_filePath, bak, overwrite: true);
            }
            catch { }
        }
    }
}
