using System;
using IwAutoUpdater.DAL.LocalFiles.Contracts;

namespace Mocks
{
    public class DatabaseScriptMock : IDatabaseScript
    {
        public DatabaseScript Load = null;
        DatabaseScript IDatabaseScript.Load(string filePath)
        {
            return Load;
        }
    }
}