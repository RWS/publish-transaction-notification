using Newtonsoft.Json.Linq;
using Tridion.ContentManager.Extensibility;
using Tridion.ContentManager.Extensibility.Events;
using Tridion.ContentManager.Notifications;
using Tridion.ContentManager.Publishing;

namespace PublishTransactionNotification.EventHandler
{
    [TcmExtension("Sends a notification to the user when a Publish Transaction finishes")]
    public class SendNotificationHandler : TcmExtension
    {
        public SendNotificationHandler()
        {
            EventSystem.Subscribe<PublishTransaction, SaveEventArgs>(SendNotification, EventPhases.Processed);
        }

        private static void SendNotification(PublishTransaction subject, TcmEventArgs e, EventPhases phases)
        {
            // only process when it is finished
            if (!subject.IsCompleted)
            {
                return;
            }

            JObject details = JObject.FromObject(new
            {
                creatorId = subject.Creator.Id.ToString(),
                state = subject.State.ToString(),
                title = subject.Title
            });

            NotificationMessage message = new NotificationMessage
            {
                Action = "tcm:finished",
                SubjectIds = new[] { subject.Id.ToString() },
                Details = details.ToString()
            };

            subject.Session.NotificationsManager.BroadcastNotification(message);
        }
    }
}
