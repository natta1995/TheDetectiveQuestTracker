using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Services
{
    public class MurderQuestGenerator
    {
        private readonly Random _rng = new();

        // Inte längre en egen lista, vi använder MurderCases.All
        private readonly IReadOnlyList<MurderCase> _cases;

        public MurderQuestGenerator()
        {
            _cases = MurderCases.All;
        }

        public MurderCase? GetCaseById(string caseId) =>
            _cases.FirstOrDefault(c => c.Id == caseId);

        public IReadOnlyList<MurderCase> GetAllCases() => _cases;

        public Quest GenerateFor(User user, MurderCase chosen)
        {
            return new Quest
            {
                Title = chosen.Title,
                Description =
                    $"Victim: {chosen.Victim}\n" +
                    $"Place: {chosen.Place}\n" +
                    $"Cause of death: {chosen.CauseOfDeath}\n" +
                    $"Summary: {chosen.ShortSummary}\n",
                Status = QuestStatus.Accepted,   // direkt till pågående
                Result = QuestResult.None,
                OwnerUsername = user.Username,
                CaseId = chosen.Id
            };
        }

        // Behåll ev. den gamla för “random fall” om du vill:
        public Quest GenerateRandomFor(User user)
        {
            var chosen = _cases[_rng.Next(_cases.Count)];
            return GenerateFor(user, chosen);
        }

    }
}

