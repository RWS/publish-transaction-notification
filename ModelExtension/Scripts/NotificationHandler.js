var notificationHandler = function(event)
{
    // only proceed if the message is the message we broadcasted ourself from the CM eventhandler
    if (event.data.action !== "tcm:finished")
    {
        return;
    }
    var userSettings = Tridion.ContentManager.UserSettings.getInstance();

    // only process the message when it is meant for this user
    if (event.data.details.creatorId === userSettings.getUserId())
    {
        if (Tridion.MessageCenter.getInstance())
        {
            var title = "Publish Transaction Finished";
            var description = `Your publish transaction for item '${event.data.details.title}' has finished with status '${event.data.details.state}'`;

            Tridion.MessageCenter.registerNotification(title, description, true);
        }
    }
};
var notificationBroadcaster = Tridion.Web.UI.Core.NotificationBroadcaster.getInstance();
notificationBroadcaster.addEventListener("notification", notificationHandler);
