# The extension
This extension is built as a showcase on how to use the Notification Framework added to SDL Web.
It shows how to create a CM eventhandler to broadcast a message when a Publish Transaction is finished using the Framework and an UI model extension to respond to the broadcasted message to show a notification with the status.

**NOTICE**
Be aware that you should not send any sensitive data using the Notification Framework as the message will be sent to _all_ clients without any respect to Privileges, Rights or Permissions.
A better mechanism is to send a message that item with id tcm:x-y-z has been changed, let the client load it and do your magic.

# Documentation
* [Notification broadcasting Event Handler example](http://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-897A8CDD-4761-45AE-A716-F984F3472733)
* [Notification framework](http://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-0282D1DD-748A-49F1-B231-DB8DC89B6AC9)

# Setup
## Configure the CM event handler
1. Build the solution and copy the output assembly `PublishTransactionNotification\EventHandler\bin\(Release|Debug)\PublishTransactionNotification.EventHandler.dll` to 
 the folder `C:\Program Files (x86)\SDL Web\bin\` on your SDL Web instance
2. Update the extensions section in the `C:\Program Files (x86)\SDL Web\config\Tridion.ContentManager.config` configuration file to load the CM event handler

        <extensions>
            <add assemblyFileName="C:\Program Files (x86)\SDL Web\bin\PublishTransactionNotification.EventHandler.dll"/>
        </extensions>
3. Restart at least the following services for the event handler to be loaded
    * SDL Web Content Manager Publisher
    * SDL Web Content Manager Service Host

## Configure the UI model extension
1. Create a new folder `PublishTransactionNotification` in the folder `C:\Program Files (x86)\SDL Web\web\WebUI\Models\` on your SDL Web instance
2. Copy the following subdirectories from `PublishTransactionNotification\ModelExtension\` into the folder created in step 1:
    * \Configuration
    * \Scripts
3. Create a virtual directory `PublishTransactionNotification` for the model extension in IIS Manager under `Sites\SDL Web\WebUI\Models\` that uses the physical path `C:\Program Files (x86)\SDL Web\web\WebUI\Models\PublishTransactionNotification\`
4. Update the models section in the `C:\Program Files (x86)\SDL Web\web\WebUI\WebRoot\Configuration\System.config` configuration file to load the UI model extension

        <model name="PublishTransactionNotification">
            <installpath>C:\Program Files (x86)\SDL Web\web\WebUI\Editors\PublishTransactionNotification\</installpath>
            <configuration>Configuration\PublishTransactionNotification.config</configuration>
            <vdir>PublishTransactionNotification</vdir>
        </model>
5. Increase the `modification` attribute on the `server` element by 1 to force the browsers to get a new version of the code.

## Exercise
1. Create a CM event handler (`PublishTransactionNotification\EventHandler\SendNotificationHandler.cs`) that will send a message to clients when a PublishTransaction finishes. For this exercise you need to send (at least) the following data:
    * Title
    * Creator
    * Status   
2. Create an UI event handler (`PublishTransactionNotification\ModelExtension\Scripts\NotificationHandler.js`) that will subscribe to notifications. For this exercise you need to make sure that:
    * the eventhandler only processes the notification if it is the notification send by the CM eventhandler created in step 1
    * the eventhandler only processes the notification if the logged in user is the same as the creator of the Publish Transaction
    * the eventhanlder shows a Message Center notification that notifies the user his/her Publish Transaction has finished (together with the state)