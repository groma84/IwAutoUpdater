using System;
using System.Collections.Generic;
using IwAutoUpdater.DAL.LocalFiles.Contracts;

namespace Mocks
{
    public class DirectoryMock : IDirectory
    {
        public int DeleteCalled = 0;
        void IDirectory.Delete(string fullPath)
        {
            ++DeleteCalled;
        }

        List<string> GetFiles = new List<string>();
        public int GetFilesCalled = 0;
        IEnumerable<string> IDirectory.GetFiles(string fullPath, string searchPattern)
        {
            ++GetFilesCalled;
            return GetFiles;
        }
    }
}