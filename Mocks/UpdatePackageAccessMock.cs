using IwAutoUpdater.DAL.Updates.Contracts;
using System;

namespace Mocks
{
    public class UpdatePackageAccessMock : IUpdatePackageAccess
    {
        public string GetFilenameOnly = String.Empty;
        public int GetFilenameOnlyCalls = 0;
        string IUpdatePackageAccess.GetFilenameOnly()
        {
            ++GetFilenameOnlyCalls;
            return GetFilenameOnly;
        }

        public bool IsRemoteFileNewer = false;
        public int IsRemoteFileNewerCalls = 0;
        bool IUpdatePackageAccess.IsRemoteFileNewer(DateTime existingFileDate)
        {
            ++IsRemoteFileNewerCalls;
            return IsRemoteFileNewer;
        }

        public byte[] GetFile = new byte[0];
        public int GetFileCalls = 0;     
        byte[] IUpdatePackageAccess.GetFile()
        {
            ++GetFileCalls;
            return GetFile;
        }

        void IDisposable.Dispose()
        {
            
        }
    }
}
