using System;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.Collections.Generic;

namespace Mocks
{
    public class SingleFileMock : ISingleFile
    {
        public Dictionary<string, bool> Delete = new Dictionary<string, bool>();
        public int DeleteCalls = 0;
        bool ISingleFile.Delete(string fullPath)
        {
            ++DeleteCalls;
            return Delete[fullPath];
        }

        public Dictionary<string, bool> DoesExist = new Dictionary<string, bool>();
        public int DoesExistCalls = 0;
        bool ISingleFile.DoesExist(string fullPath)
        {
            ++DoesExistCalls;
            return DoesExist[fullPath];
        }

        public Dictionary<string, DateTime> GetLastModified = new Dictionary<string, DateTime>();
        public int GetLastModifiedCalls = 0;
        DateTime ISingleFile.GetLastModified(string fullPath)
        {
            ++GetLastModifiedCalls;
            return GetLastModified[fullPath];
        }


        public Dictionary<string, bool> Write = new Dictionary<string, bool>();
        public int WriteCalls = 0;
        bool ISingleFile.Write(string localFullPath, byte[] remoteFile)
        {
            ++WriteCalls;
            return Write[localFullPath];
        }
    }
}