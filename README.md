App42_Push_Sample_Unity_Windows
===============================
# About Application
This application shows how can we integrate our Unity3D game with Push Notification using windows phone Framework.
Once a game registered for Push Notification Unity3D game get Notification message accordingly

This is a sample Windows Phone Unity 3D app is made by using App42 back-end platform. It uses Push Notification of App42 platform. Here are the few easy steps to run this sample app.

__Register with App42 platform__.

Do registeration on [AppHQ console](!https://apphq.shephertz.com/register/app42Login),get login and create an app from App Manager Tab.

Go to the "application keys" and get Api key and secret keys.

Download the Unity project from here and open asset folder.

Open Constants.cs file in sample app and put api key and secret key.

__System Requirements__:

1.Unity 3D

2..Net 3.5
 
3.Windows Phone SDK

__Prior Checks__:

There is two plugin dll for Push Notifcation on Windows Phone

1.Fake Dll

2.Real Dll

Put [Fake Dll](!https://github.com/rahulpshephertz/App42_PushPlugin_Sample_Unity_Windows/tree/master/Push_Notification_Plugin/UnityPluginForWindowsPhone/FakeDLL/bin/Release) in Asset folder

Put [Real Dll](!https://github.com/rahulpshephertz/App42_PushPlugin_Sample_Unity_Windows/tree/master/Push_Notification_Plugin/UnityPluginForWindowsPhone/RealDLL/Bin/Release) in Assets/Plugin/Wp8 folder

Before building Windows Phone application dont forget to attach UseMyPlugin.cs file on MainCamera.
You can also replace Push Notification icon with your icon in \Assets\plugins\Android\assets folder with same name and same type.

After Building Windows Phone Project from Unity,We will have need to Modify WMAppManifest file
Add capabilities 
  <Capability Name="ID_CAP_PUSH_NOTIFICATION" />
  <Capability Name="ID_CAP_PROXIMITY" />
  
__Design Details__:

__Push Registration__: To use Notification message in your game you have to register your game for PushNotification by executing this code defined in UseMyPlugin.cs file of sample project.
```
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
```
Send PushNotification to User using Unity App42 SDK :
```
public void sendPushToUser(string userName,string msg){
		App42API.BuildPushNotificationService().SendPushMessageToUser(userName,msg,new Callback());
}

Send PushNotification to all users using Unity App42 SDK :
	public void sendPushToAll(string msg){
		App42API.BuildPushNotificationService().SendPushMessageToAll(msg,new Callback());	
}
public class Callback : App42CallBack{
	public static string response = "";
	
	public void OnSuccess(object obj){
		response = obj.ToString();
	}
	
	public void OnException(Exception ex){
		Debug.Log("EDITEOR---"+ ex.ToString());
		response=ex.ToString();
	}
}
```

