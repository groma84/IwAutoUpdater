﻿using IwAutoUpdater.DAL.Updates.Contracts;
using System;
using IwAutoUpdater.CrossCutting.Configuration.Contracts;

namespace Mocks
{
    public class UpdatePackageMock : IUpdatePackage
    {
        public IUpdatePackageAccess Access = null;
        IUpdatePackageAccess IUpdatePackage.Access
        {
            get
            {
                return Access;
            }
        }

        public string PackageName = String.Empty;
        string IUpdatePackage.PackageName
        {
            get
            {
                return PackageName;
            }
        }

        public ServerSettings Settings = new ServerSettings();
        ServerSettings IUpdatePackage.Settings
        {
            get
            {
                return Settings;
            }
        }
    }
}
