using System;
using IwAutoUpdater.DAL.LocalFiles.Contracts;
using System.Collections.Generic;

namespace Mocks
{
    public class SingleFileMock : ISingleFile
    {
        public bool Delete = false;
        public int DeleteCalls = 0;
        bool ISingleFile.Delete(string fullPath)
        {
            ++DeleteCalls;
            return Delete;
        }

        public bool DoesExist = false;
        public int DoesExistCalls = 0;
        bool ISingleFile.DoesExist(string fullPath)
        {
            ++DoesExistCalls;
            return DoesExist;
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

        public int ReadAsStringCalls = 0;
        public string ReadAsString = "";
        string ISingleFile.ReadAsString(string fullPath)
        {
            ++ReadAsStringCalls;
            return ReadAsString;
        }
    }
}