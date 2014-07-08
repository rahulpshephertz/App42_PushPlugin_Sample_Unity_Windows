using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
using System;

public class Callback : App42CallBack{
	

	public static string response = "response";

	
	public void OnSuccess(object obj){
		PushNotification pushObj = (PushNotification)obj;
		Debug.Log(response = pushObj.GetExpiry().ToString());
	}
	
	public void OnException(Exception ex){
		Debug.Log("EDITEOR---"+ ex.ToString());
		response=ex.ToString();
	}


}
