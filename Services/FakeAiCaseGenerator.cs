using TheDetectiveQuestTracker.Modell;

namespace TheDetectiveQuestTracker.Services
{
    /// <summary>
    /// Fejkad AI-generator som skapar MurderCase-objekt lokalt,
    /// baserat på en kort titel (shortTitle).
    /// Ingen nätverk, ingen riktig AI – bara din egen logik.
    /// </summary>
    public class FakeAiCaseGenerator
    {
        private readonly Random _rng = new();

        public MurderCase GenerateCaseFromTitle(string shortTitle)
        {
            // 1. Välj en prioritet (antingen random eller utifrån titel)
            var possiblePriorities = new[] { CasePriority.Low, CasePriority.Medium, CasePriority.High };
            var priority = possiblePriorities[_rng.Next(possiblePriorities.Length)];

            // 2. Välj plats, dödsorsak, vapen osv från små "byggstenar"
            string place = PickOne(new[]
            {
                "a narrow, rain-soaked alley off Whitechapel Road",
                "a dimly lit boarding house near Paddington Station",
                "a cramped flat overlooking the Thames",
                "a smoke-filled jazz club in Soho",
                "a small terraced house in Bloomsbury",
                "the back room of a tea shop in Kensington"
            });

            string victim = PickOne(new[]
            {
                "a retired army captain",
                "a quiet bookseller",
                "a factory foreman",
                "a widowed schoolteacher",
                "a railway clerk",
                "a local black market contact"
            });

            string causeOfDeath = PickOne(new[]
            {
                "a single stab wound to the chest",
                "a blunt force trauma to the head",
                "suspected poisoning",
                "a gunshot wound to the back",
                "strangulation with a thin cord"
            });

            string weapon = PickOne(new[]
            {
                "a bloodstained kitchen knife",
                "a heavy brass candlestick",
                "a small revolver with two spent rounds",
                "a teacup containing traces of poison",
                "a length of frayed clothesline"
            });

            string crimeSceneDescription =
                "London, 1944. The blackout is in effect and distant sirens still echo after the last raid.\n" +
                "Rain drums steadily against the windows, and the electric lights flicker uncertainly.\n" +
                "In the small room the air smells of coal dust, damp wool, and something metallic.\n" +
                "The victim lies awkwardly on the floor, shadows from the flickering lamp stretching long along the walls.";

            // 3. Skapa misstänkta (3–4 stycken)
            var suspects = new List<Suspect>
            {
                new Suspect
                {
                    Name = PickOne(new[]
                    {
                        "Martha Briggs",
                        "Evelyn Hart",
                        "Arthur Hale",
                        "Victor Blake",
                        "Emily Shaw"
                    }),
                    Label = "the neighbour",
                    Statement = "I heard raised voices earlier, sir, but with the sirens and all that... it’s hard to say who it was."
                },
                new Suspect
                {
                    Name = PickOne(new[]
                    {
                        "Sergeant Daniel Moore",
                        "Nigel Stone",
                        "Harold Briggs",
                        "Leonard Price"
                    }),
                    Label = "a military man on leave",
                    Statement = "I was only passing through, detective. London’s full of noise these days — one more shout hardly stands out."
                },
                new Suspect
                {
                    Name = PickOne(new[]
                    {
                        "Clara Whitfield",
                        "Margaret Doyle",
                        "Adelaide Finch",
                        "Beatrice Bloom"
                    }),
                    Label = "the landlady",
                    Statement = "He owed some rent, that’s true — but murder? In times like these, we cling to what little we have left, not throw it away."
                }
            };

            // ibland lägger vi till en fjärde misstänkt
            if (_rng.NextDouble() < 0.5)
            {
                suspects.Add(new Suspect
                {
                    Name = PickOne(new[]
                    {
                        "Peter Lang",
                        "Lionel Graves",
                        "Christopher Hale"
                    }),
                    Label = "an acquaintance",
                    Statement = "We had words earlier, but that’s all. There’s enough death in this city without adding to it, don’t you think?"
                });
            }

            // 4. Välj en mördare (index i Suspects-listan)
            int killerIndex = _rng.Next(suspects.Count);

            // 5. Skapa några ledtrådar (Clues)
            var clues = new List<string>
            {
                "A ration book with several coupons missing.",
                "Mud on the floor from boots matching one of the suspects.",
                "A half-burnt letter in the fireplace.",
                "A teacup with traces of lipstick that does not match the victim.",
                "A theatre ticket stub from the previous night."
            };

            // 6. SolutionText – enkel upplösning som knyter ihop det
            string killerName = suspects[killerIndex].Name;
            string solutionText =
                $"In the end, the evidence pointed towards {killerName}.\n" +
                "Small contradictions in their story, together with the physical clues at the scene, " +
                "made it clear they had both motive and opportunity. " +
                "Confronted with the facts, they finally confessed.";

            // 7. Bygg och returnera själva MurderCase
            return new MurderCase
            {
                Id = Guid.NewGuid().ToString(),
                Title = shortTitle,                 // vi använder titeln du valde i telefonen
                ShortSummary = $"A death in {place}. Several suspects, all with something to hide.",
                Victim = victim,
                Place = place,
                CauseOfDeath = causeOfDeath,
                Weapon = weapon,
                CrimeSceneDescription = crimeSceneDescription,
                Suspects = suspects,
                Clues = clues,
                KillerIndex = killerIndex,
                SolutionText = solutionText,
                Priority = priority
            };
        }

        private string PickOne(string[] items)
        {
            return items[_rng.Next(items.Length)];
        }
    }
}
