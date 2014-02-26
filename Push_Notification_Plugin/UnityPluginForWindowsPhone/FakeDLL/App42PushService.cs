using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace UnityPluginForWindowsPhone
{
    public class App42PushService
    {
        public delegate void PushServiceRegistrationCallback(object response, bool isError);
        public delegate void PushServiceMessageCallback(object sender, Dictionary<String, String> response);
        public App42PushService()
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="registrationCallBack">Registration Callback with parameter </param>
        /// <param name="messageCallback"></param>
        public void CreatePushChannel(String channelName,bool IfEnableTileNotifcation,PushServiceRegistrationCallback registrationCallBack, PushServiceMessageCallback messageCallback)
        { 
            PushServiceRegistrationCallback sevice=new PushServiceRegistrationCallback(registrationCallBack);
            new Thread(() => sevice("Got Wrong Dll", false)).Start();
        }

    }
}
