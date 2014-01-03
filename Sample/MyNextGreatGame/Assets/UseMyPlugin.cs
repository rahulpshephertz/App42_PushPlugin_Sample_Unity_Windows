using UnityEngine;
#if UNITY_WP8
using System.Collections.Generic;
using System.Text;
using UnityPluginForWindowsPhone;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
#endif
public class UseMyPlugin : MonoBehaviour
{
	private Rect m_ScreenRectangle;
	private GUIStyle m_GUIStyle;
	string message = "No Message yet";
	private string userName = "";
	private string myMsg = "Hi I am using Push App42";
	public const string UnityRegistrationMethod = "StoreDeviceId";
	public string app42Response="";

	void Start()
	{
		m_ScreenRectangle = new Rect(0, 0, Screen.width, 100);
		m_GUIStyle = new GUIStyle {fontSize = 16, alignment = TextAnchor.MiddleCenter,wordWrap=true};
		#if UNITY_WP8
		App42API.Initialize(Constants.ApiKey,Constants.SecretKey);
		App42API.SetLoggedInUser(Constants.UserId);
		App42PushService pushService=new App42PushService();
		pushService.CreatePushChannel(Constants.UserId,PushChannelRegistrationCallback,PushChannelMessageCallback);
		message="Push Channel Start";
        #endif

	}
#if UNITY_WP8
	public void sendPushToUser(string userName,string msg){
		
		App42API.BuildPushNotificationService().SendPushMessageToUser(userName,msg,new Callback());
	}
	
	public void sendPushToAll(string msg){
		
		App42API.BuildPushNotificationService().SendPushMessageToAll(msg,new Callback());
		
	}
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
	/*
	 * This function is called from Native Android Code to store Device Id for Push Notification.
	 * @param deviceId require to register for Push Notification on App42 API
	 * 
	 */
	public void StoreDeviceId(string pushUri) {
		App42API.BuildPushNotificationService().StoreDeviceToken(App42API.GetLoggedInUser(),pushUri,
		                                                         Constants.deviceType,new Callback());
	}	
	/*
	 * This function is called from Native Android Code to whenever any Push Notification receive.
	 * @param msg Push Notification message
	 * 
	 */
	public void Success(string msg) {
		if(msg != null) {
			Debug.Log("Push Message is Here: " + msg);
			message=msg;
			app42Response="";
		}
	}
	void OnGUI()
	{
		GUI.Label(m_ScreenRectangle, message, m_GUIStyle);
		
		userName = GUI.TextField (new Rect (5, 150, 300, 30),userName);
		myMsg = GUI.TextField (new Rect (5, 190, 300, 40),myMsg);
		
		if (GUI.Button(new Rect (5, 240, 300, 50), "Send Push To User"))
		{
			sendPushToUser(userName,myMsg);
		}
		if (GUI.Button(new Rect (5, 300, 300, 50), "Send Push To All"))
		{
			sendPushToAll(myMsg);
		}
		if (GUI.Button(new Rect(5, 360, 300, 50), "Exit game"))
		{
			Application.Quit();
		}
	}
#endif


}
