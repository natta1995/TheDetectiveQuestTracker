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
                Id = "mr_club",
                Title = "Mordet på Mr Club",
                ShortSummary = "En rik herre hittas död i sitt bibliotek. Fyra personer fanns i huset.",

                Victim = "Mr Club",
                Place = "Hans privata bibliotek på Kensington Row",
                CauseOfDeath = "Pistolskott i huvudet",
                Weapon = "En liten revolver vid hans högra hand.",

                CrimeSceneDescription =
                    "Biblioteket är mörkpanelat, med höga bokhyllor och en brasa som nästan brunnit ut.\n" +
                    "Mr Club sitter livlös i sin läderfåtölj, en revolver vid hans hand och ett glas\n" +
                    "halvfullt whisky på bordet bredvid.",

                Suspects = new List<Suspect>
                {
                    new Suspect
                    {
                        Name = "Mr Hargreaves",
                        Label = "butlern",
                        Statement = "”Jag var i köket och lyssnade på radion när skottet föll.”"
                    },
                    new Suspect
                    {
                        Name = "Edward Club",
                        Label = "brorsonen",
                        Statement = "”Jag satt i gästrummet och skrev brev hela kvällen.”"
                    },
                    new Suspect
                    {
                        Name = "Mrs Bloom",
                        Label = "hushållerskan",
                        Statement = "”Jag städade matsalen, jag gick aldrig upp till biblioteket.”"
                    },
                    new Suspect
                    {
                        Name = "Mrs Ashdown",
                        Label = "grannen",
                        Statement = "”Jag gick hem innan regnet började.”"
                    }
                },

                Clues = new List<string>
                {
                    "Mr Club var vänsterhänt, men pistolen ligger vid hans högra hand.",
                    "Radion i köket är avstängd och sladden utdragen.",
                    "Mrs Ashdowns paraply är blött, trots att hon säger att hon gick hem innan regnet."
                },

                KillerIndex = 1, // Edward
                SolutionText =
                    "Pistolen i fel hand visar att det inte var självmord. Flera ljuger om småsaker,\n" +
                    "men bara Edward har ett starkt motiv och ett svagt alibi. När du pressar honom\n" +
                    "faller historien om brevet ihop, och han erkänner mordet."
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
                    $"Offer: {chosen.Victim}\n" +
                    $"Plats: {chosen.Place}\n" +
                    $"Dödssätt: {chosen.CauseOfDeath}\n" +
                    $"Kort: {chosen.ShortSummary}\n",
                Status = QuestStatus.Accepted,
                OwnerUsername = user.Username,
                CaseId = chosen.Id
            };

            return quest;
        }
    }
}
