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
            // subscribe to the save event for publish transactions
            EventSystem.Subscribe<PublishTransaction, SaveEventArgs>(SendNotification, EventPhases.Processed);
        }

        private static void SendNotification(PublishTransaction subject, TcmEventArgs e, EventPhases phases)
        {
            // only send a message when the publishing is finished. The publish transaction gets saved at least when created and on a status update 
            if (!subject.IsCompleted)
            {
                return;
            }

            // create an object that contains some data that we want to send to the client as the details of the message
            JObject details = JObject.FromObject(new
            {
                creatorId = subject.Creator.Id.ToString(),
                state = subject.State.ToString(),
                title = subject.Title
            });

            NotificationMessage message = new NotificationMessage
            {
                // we need an identifier that we can use in the UI extension to distinguish our messages from others
                Action = "tcm:finished",
                SubjectIds = new[] { subject.Id.ToString() },
                Details = details.ToString()
            };

            subject.Session.NotificationsManager.BroadcastNotification(message);
        }
    }
}
