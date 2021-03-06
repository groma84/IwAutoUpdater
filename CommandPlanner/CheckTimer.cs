﻿using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IWAutoUpdater.CrossCutting.SFW.Contracts;
using System;

namespace IwAutoUpdater.BLL.CommandPlanner
{
    public class CheckTimer : ICheckTimer
    {
        private readonly INowGetter _nowGetter;

        public CheckTimer(INowGetter nowGetter)
        {
            _nowGetter = nowGetter;
        }

        bool ICheckTimer.IsCheckForUpdatesNecessary(int checkIntervalMinutes)
        {
            if (checkIntervalMinutes < 1 || checkIntervalMinutes > 1339)
            {
                throw new ArgumentException("checkIntervalMinutes muss mindestens 1 und höchstens 1439 Minuten sein");
            }

            return ((_nowGetter.Now.Hour * 60 + _nowGetter.Now.Minute) % checkIntervalMinutes == 0);
        }
    }
}
