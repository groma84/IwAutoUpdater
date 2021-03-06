﻿using IwAutoUpdater.CrossCutting.Base;

namespace IwAutoUpdater.DAL.Notifications.Contracts
{    
    [DIIgnore]
    public interface INotificationReceiver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="message"></param>
        /// <returns>Erfolgreich?</returns>
        bool SendNotification(string heading, string message);
    }
}