App42_Push_Sample_Unity_Windows
===============================
About Application
This application shows how can we integrate our Unity3D game with Push Notification using windows phone Framework.
Once a game registered for Push Notification Unity3D game get Notification message accordingly

This is a sample Windows Phone Unity 3D app is made by using App42 back-end platform. It uses Push Notification of App42 platform. Here are the few easy steps to run this sample app.

Register with App42 platform.
Create an app once you are on Quick start page after registration.
If you are already registered, login to AppHQ console and create an app from App Manager TabG.
Go to Application Keys and Copy the Api key and secret keys.
Download the Unity project from here and open asset folder.It Constants.cs Open Constants.cs file in sample app and put Api and Secret key.

System Requirements:

1.Unity 3D

2..Net 3.5
 
3.Windows Phone SDK

Prior Checks: 
We have two plugin dll of Push Notifcation for Windows Phone one is FakeDll which can be found from below link
https://github.com/rahulpshephertz/App42_PushPlugin_Sample_Unity_Windows/tree/master/Push_Notification_Plugin/UnityPluginForWindowsPhone/FakeDLL/bin/Release
Put Fake Dll is Asset folder of Unity.
Now take the real dll from below link
https://github.com/rahulpshephertz/App42_PushPlugin_Sample_Unity_Windows/tree/master/Push_Notification_Plugin/UnityPluginForWindowsPhone/RealDLL/Bin/Release
And put this dll in Assets/Plugin/Wp8 folder

Before building Windows Phone application please review following things :
You have attached UseMyPlugin.cs file on MainCamera.
You can also replace Push Notification icon with your icon in \Assets\plugins\Android\assets folder with same name and same type.

After Building Windows Phone Project from Unity,We will have need to Modify WMAppManifest file
Add capabilities 
  <Capability Name="ID_CAP_PUSH_NOTIFICATION" />
  <Capability Name="ID_CAP_PROXIMITY" />
  
Design Details:

Push Registration: To use Notification message in your game you have to register your game for PushNotification by executing this code defined in UseMyPlugin.cs file of sample project.

pushService.CreatePushChannel(Constants.UserId,PushChannelRegistrationCallback,PushChannelMessageCallback);

void PushChannelRegistrationCallback(object msg,bool IsError)
{	
		message=(string)msg;
		if(!IsError)
		{
			StoreDeviceId((string)msg);
		}
		Debug.Log(message);
}

Send PushNotification to User using Unity App42 SDK :
public void sendPushToUser(string userName,string msg){
		App42API.BuildPushNotificationService().SendPushMessageToUser(userName,msg,new Callback());
}

Send PushNotification to all users using Unity App42 SDK :
	public void sendPushToAll(string msg){
		App42API.BuildPushNotificationService().SendPushMessageToAll(msg,new Callback());	
}

Received Message Callback
void PushChannelMessageCallback(object sender,Dictionary<string,string> e)
{	
		// Parse out the information that was part of the message.
		StringBuilder msg = new StringBuilder();
		foreach (string key in e.Keys)
		{
			msg.AppendFormat("{0}: {1}\n", key, e[key]);	
		    //if (string.Compare(
		    //   key,
		    //  "wp:Param",
		    //  System.Globalization.CultureInfo.InvariantCulture,
		    //  System.Globalization.CompareOptions.IgnoreCase) == 0)
		    //{
		    //    relativeUri = e.Collection[key];
		    //}
		}
		message=msg.ToString();
		Debug.Log(message);
}

