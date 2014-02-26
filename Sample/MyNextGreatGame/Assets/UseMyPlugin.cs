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
    private string myMsg = "Hi I am using Push App42";
    public const string UnityRegistrationMethod = "StoreDeviceId";
    public string app42Response = "";
    byte mNotificationType;
	#if UNITY_WP8
    App42PushService pushService = new App42PushService();
    #endif
	void Start()
    {
        m_ScreenRectangle = new Rect(0, 0, Screen.width, 100);
        m_GUIStyle = new GUIStyle { fontSize = 16, alignment = TextAnchor.MiddleCenter, wordWrap = true };

#if UNITY_WP8
		App42API.Initialize(Constants.ApiKey,Constants.SecretKey);
		message="Push Channel Start";
#endif
    }
#if UNITY_WP8
	public void sendPushToUser(string userName,string msg){
        
		switch (mNotificationType)
            {
                case NotificationType.TOAST:
			        //To send Toast Push Message
                    App42API.BuildPushNotificationService().SendPushMessageToUser(userName, "msg", new Callback()); break;
                case NotificationType.TILE:
                    Tile tile = new Tile();
                    tile.title = "Tile";
                    tile.backTitle = "Tile Message";
                    tile.count = "10";
                    tile.backContent = "back";
                    tile.backBackgroundImage = "Blue.jpg";
                    tile.backgroundImage = "Red.jpg";
					// To send Tile Push message
                    App42API.BuildPushNotificationService().SendPushTileMessageToUser(userName, tile, new Callback());
                    break;
            }
        
	}
	
	public void sendPushToAll(string msg){
       
            switch (mNotificationType)
            {
                case NotificationType.TOAST:
                    App42API.BuildPushNotificationService().SendPushMessageToAll("msg", new Callback()); break;
                case NotificationType.TILE:
                    Tile tile = new Tile();
                    tile.title = "Tile";
                    tile.backTitle = "Tile Message";
                    tile.count = "10";
                    tile.backContent = "back";
                    tile.backBackgroundImage = "Blue.jpg";
                    tile.backgroundImage = "Red.jpg";
                    App42API.BuildPushNotificationService().SendPushTileMessageToAll(tile, new Callback());
                    break;
            }
     
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
#endif
    void OnGUI()
    {
        GUI.Label(m_ScreenRectangle, message, m_GUIStyle);

        Constants.UserId = GUI.TextField(new Rect(5, 150, 300, 50), Constants.UserId);
        mNotificationType = NotificationType.TILE;
		#if UNITY_WP8
		if (GUI.Button (new Rect (220, 220, 200, 50), "Register")) 
		{
		 switch (mNotificationType)
			{
				case NotificationType.TOAST:
					pushService.CreatePushChannel (Constants.UserId, false, PushChannelRegistrationCallback, PushChannelMessageCallback);
					break;
			case NotificationType.TILE:
				   pushService.CreatePushChannel (Constants.UserId, true, PushChannelRegistrationCallback, PushChannelMessageCallback);
				     break;
			}
		}
		if (GUI.Button (new Rect (5, 220, 150, 50), "App42 Login")) 
		{	
			App42API.SetLoggedInUser(Constants.UserId);
		}
        Constants.RemoteUserid = GUI.TextField(new Rect(5, 290, 300, 50), Constants.RemoteUserid);

        if (GUI.Button(new Rect(5, 350, 300, 50), "Send Push To User"))
        {
            sendPushToUser(Constants.RemoteUserid, myMsg);
        }
	
        if (GUI.Button(new Rect(5, 410, 300, 50), "Send Push To All"))
        {
            sendPushToAll(myMsg);
        }
		#endif
        if (GUI.Button(new Rect(5, 470, 300, 50), "Exit game"))
        {
            Application.Quit();
        }
    } 
}
public class NotificationType
{
	public const byte TOAST = 0;
	public const byte TILE= 1;
	public const byte RAW = 2;
}