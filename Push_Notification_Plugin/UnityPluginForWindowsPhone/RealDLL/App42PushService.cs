using Microsoft.Phone.Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UnityPluginForWindowsPhone
{
    public class App42PushService
    {
        public delegate void PushServiceRegistrationCallback(object response, bool isError);
        public delegate void PushServiceMessageCallback(object sender, Dictionary<String, String> response);
        private PushServiceRegistrationCallback mRegistrationCallback = null;
        private PushServiceMessageCallback mMessageCallback;
        public App42PushService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="registrationCallBack">Registration Callback with parameter </param>
        /// <param name="messageCallback"></param>
        public void CreatePushChannel(String channelName, PushServiceRegistrationCallback registrationCallBack, PushServiceMessageCallback messageCallback)
        {
            /// Holds the push channel that is created or found.
            HttpNotificationChannel pushChannel;
            mRegistrationCallback = registrationCallBack;
            mMessageCallback = messageCallback;
            // Try to find the push channel.
            pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                pushChannel.Open();

                // Bind this new channel for toast events.
                pushChannel.BindToShellToast();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
            }
        }
        /// <summary>
        /// Event handler for when the push channel Uri is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
            });
            Deployment.Current.Dispatcher.BeginInvoke(new PushServiceRegistrationCallback(mRegistrationCallback), e.ChannelUri.ToString(), false);
        }

        /// <summary>
        /// Event handler for when a push notification error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            String ErrorMessage = String.Format("A push notification {0} error occurred.  {1} ({2}) {3}", e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                System.Diagnostics.Debug.WriteLine(ErrorMessage);
            });
            Deployment.Current.Dispatcher.BeginInvoke(new PushServiceRegistrationCallback(mRegistrationCallback), ErrorMessage, true);
            // Error handling logic for your particular application would be here.
            //Dispatcher.BeginInvoke(() =>
            //    MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
            //        e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
            //        );
        }

        /// <summary>
        /// Event handler for when a toast notification arrives while your application is running.  
        /// The toast will not display if your application is running so you must add this
        /// event handler if you want to do something with the toast notification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new PushServiceMessageCallback(mMessageCallback),sender, e.Collection);
            
            //StringBuilder message = new StringBuilder();
            //string relativeUri = string.Empty;

            //message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            //// Parse out the information that was part of the message.
            //foreach (string key in e.Collection.Keys)
            //{
            //    message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

            //    if (string.Compare(
            //        key,
            //        "wp:Param",
            //        System.Globalization.CultureInfo.InvariantCulture,
            //        System.Globalization.CompareOptions.IgnoreCase) == 0)
            //    {
            //        relativeUri = e.Collection[key];
            //    }
            //}

        }

    }
}
