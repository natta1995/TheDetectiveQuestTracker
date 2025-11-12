using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;

namespace TheDetectiveQuestTracker.Repositories
{
    public class JsonFileQuestRepository : IQuestRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };
        private readonly object _lock = new();
        private List<Quest> _items = new();

        public JsonFileQuestRepository(string? filePath = null)
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TheDetectiveQuestTracker");
            Directory.CreateDirectory(dir);
            _filePath = filePath ?? Path.Combine(dir, "quests.json");
            Load();
        }

        public void Add(Quest q)
        {
            lock (_lock)
            {
                _items.Add(q);
                Save();
            }
        }

        public IEnumerable<Quest> GetForUser(string username)
        {
            lock (_lock)
                return _items.Where(x => x.OwnerUsername == username).ToList();
        }

        // Valfritt men användbart redan nu:
        public Quest? Get(Guid id)
        {
            lock (_lock) return _items.FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Quest> GetAll()
        {
            lock (_lock) return _items.ToList();
        }
        public void Update(Quest q)
        {
            lock (_lock) Save();
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_filePath)) { _items = new(); return; }
                var json = File.ReadAllText(_filePath);
                _items = JsonSerializer.Deserialize<List<Quest>>(json, _opts) ?? new();
            }
            catch
            {
                TryBackup();
                _items = new();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_items, _opts);
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
