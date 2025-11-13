using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDetectiveQuestTracker.Modell;


namespace TheDetectiveQuestTracker.Services
{
    public class MurderQuestGenerator
    {
        private readonly Random _rng = new();

        private readonly List<MurderCase> _cases = new()
        {
            new MurderCase
            {
               

            Id = "club_library_murder",

            Title = "The Library Murder on Kensington Row",

            ShortSummary = "A wealthy gentleman is found dead in his private library. Four individuals were inside the house that evening.",

            Victim = "Sir Reginald Blackwood",
            Place = "His private library on Kensington Row",
            CauseOfDeath = "A gunshot wound to the head",
            Weapon = "A small revolver found near his right hand.",

            CrimeSceneDescription =
                "The library is panelled in dark oak, its tall bookshelves stretching up toward the ceiling.\n" +
                "A dying fire crackles faintly in the hearth. Sir Reginald sits slumped in his leather armchair,\n" +
                "a revolver near his right hand and a half-finished glass of whisky on the side table.\n" +
                "Rain patters rhythmically against the tall window overlooking the quiet street.",

            Suspects = new List<Suspect>
            {
                new Suspect
                {
                    Name = "Mr. Hargreaves",
                    Label = "the butler",
                    Statement = "\"I was in the kitchen, sir, listening to the wireless when the shot rang out — or so they tell me. I heard nothing at all.\""
                },
                new Suspect
                {
                    Name = "Edmund Blackwood",
                    Label = "the nephew",
                    Statement = "\"I spent the evening in the guest room, writing letters. I never left the room, I assure you.\""
                },
                new Suspect
                {
                    Name = "Mrs. Beatrice Bloom",
                    Label = "the housekeeper",
                    Statement = "\"I was polishing the silver in the dining room. I did not go near the library, not once.\""
                },
                new Suspect
                {
                    Name = "Mrs. Evelyn Ashdown",
                    Label = "the neighbour",
                    Statement = "\"I left before the rain began. I heard nothing unusual, and the gentleman was quite well when I saw him last.\""
                }
            },

            Clues = new List<string>
            {
                "Sir Reginald was left-handed, but the revolver lies near his right hand.",
                "The wireless in the kitchen is switched off, with its cord unplugged from the wall.",
                "Mrs. Ashdown’s umbrella is still wet by the door, despite her claim that she left before the rain began."
            },

            KillerIndex = 1, // Edmund (the nephew)

            SolutionText =
                "The revolver in the wrong hand makes suicide unlikely from the start. Several suspects tell small lies, but only one has a true motive.\n\n" +
                "Edmund's story about writing letters collapses under scrutiny; he cannot recall simple details, and no such letter is found.\n" +
                "Faced with the inconsistencies, he finally confesses — he shot Sir Reginald in the library to secure his inheritance."
        }

                };

        public MurderCase? GetCaseById(string caseId) =>
            _cases.FirstOrDefault(c => c.Id == caseId);

        public Quest GenerateFor(User user)
        {
            var chosen = _cases[_rng.Next(_cases.Count)];

            var quest = new Quest
            {
                Title = chosen.Title,
                Description =
                    $"Victim: {chosen.Victim}\n" +
                    $"Place: {chosen.Place}\n" +
                    $"Case of death: {chosen.CauseOfDeath}\n" +
                    $"Summary: {chosen.ShortSummary}\n",
                Status = QuestStatus.Accepted,
                OwnerUsername = user.Username,
                CaseId = chosen.Id
            };

            return quest;
        }
    }
}
