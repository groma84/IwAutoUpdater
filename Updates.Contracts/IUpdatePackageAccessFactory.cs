﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.Updates.Contracts
{
    public interface IUpdatePackageAccessFactory
    {
        IUpdatePackageAccess CreateLocalFileAccess(string filePath);
        IUpdatePackageAccess CreateUncPathAccess(string uncPath, string username, string password);
        IUpdatePackageAccess CreateHttpDownloadAccess(string url);
    }
}