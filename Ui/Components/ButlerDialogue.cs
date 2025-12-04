
namespace TheDetectiveQuestTracker.Ui.Components
{
    internal class ButlerDialogue
    {
        // Använd en delad Random så att snabba anrop inte ger samma resultat
        private static readonly Random _rng = new();

        public static string GetRandomMessage()
        {
            var messages = new[]
            {
                "I’m terribly sorry, sir — someone’s at the door.",
                "I’ve already prepared your tea, just the way you like it.\r\nAllow me to light the fireplace as well; it’s frightfully cold in here tonight.",
                "If anyone in this city can unravel those dreadful cases, it’s you, sir.\r\nDo try not to worry.",
                "Sir, this war seems to have no end.\r\nSo much misery… and the price of coffee is simply outrageous.\r\nThank heavens the tea is holding up.",
                "Sir, the city barely sleeps these days.\r\nThe sirens wail, the streets are dust and echoes…\r\nYet somehow, your tea remains warm.\r\nSmall mercies, sir — small mercies indeed.",
                "It seems to rain over London more and more these days.\r\nAnd there’s an unease in the air, sir — one can feel it.",
                "One does one’s duty, sir.\r\nEven when the walls shake. Especially then.",
                "Whatever they say, sir — you’ve done London proud.\r\nFew can claim that these days."
            };

            int index = _rng.Next(messages.Length);
            return messages[index];
        }
    }
}
