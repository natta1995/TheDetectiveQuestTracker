using TheDetectiveQuestTracker.Modell;
using TheDetectiveQuestTracker.Repositories;

namespace TheDetectiveQuestTracker.Services
{
    public class NotificationService
    {
        private readonly IEmailSender _emailSender;

        public NotificationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void CheckAndSendDeadlineWarnings(User user, IQuestRepository questRepo)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                return;

            var now = DateTime.Now;

            var quests = questRepo
                .GetForUser(user.Username)
                .Where(q => q.Status == QuestStatus.Accepted && q.ExpiresAt is not null)
                .ToList();

            foreach (var q in quests)
            {
                var remaining = q.ExpiresAt!.Value - now;

                if (remaining <= TimeSpan.Zero)
                    continue;

                      // 20h-påminnelse
                      if (!q.Reminder20hSent &&
                          remaining <= TimeSpan.FromHours(20) &&
                          remaining > TimeSpan.FromHours(1))
                      {
                          SendReminder(user, q, remaining, "20 hours");
                          q.Reminder20hSent = true;
                          questRepo.Update(q);
                      }

                      // 1h-påminnelse
                      if (!q.Reminder1hSent &&
                          remaining <= TimeSpan.FromHours(1))
                      {
                          SendReminder(user, q, remaining, "1 hour");
                          q.Reminder1hSent = true;
                          questRepo.Update(q);
                      }



            }
        }

        private void SendReminder(User user, Quest q, TimeSpan remaining, string label)
        {
            var subject = $"Case \"{q.Title}\" is nearing its deadline";

            var body =
            $@"

            Your case: ""{q.Title}"" will expire in approximately {remaining.TotalHours:F1} hours.
            
            If the case is not solved before the time is up, Scotland Yard will have to take over this case.

            Expires at: {q.ExpiresAt:yyyy-MM-dd HH:mm} ";

            _emailSender.Send(user.Email!, subject, body);
        }
    }
}

