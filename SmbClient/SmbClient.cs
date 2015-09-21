using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;

namespace IngSoft.SmbClient
{
    public class SmbClient : IDisposable
    {
        private string _connectedPath;

        /// <summary>
        /// Create a new smb client for a specific uri with specific credentials
        /// </summary>
        /// <param name="credentials">user-creds including domain</param>
        /// <param name="uri">path to smb share</param>
        public SmbClient(NetworkCredential credentials, Uri uri)
        {
            _connectedPath = uri.LocalPath.Replace('/', '\\').TrimEnd('\\').Replace(uri.Scheme + ":", string.Empty);

            try
            {
                string s = PinvokeWindowsNetworking.connectToRemote(_connectedPath, credentials.UserName, credentials.Password);
                if (s != null) throw new Exception(s);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the file list on the share
        /// </summary>
        /// <returns>Array of FileInfo</returns>
        public FileInfo[] GetFileList()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_connectedPath);
            FileInfo[] ret = dirInfo.GetFiles();
            return ret;
        }

        /// <summary>
        /// Gets the file list on the share with subdirs
        /// </summary>
        /// <returns>Array of FileInfo</returns>
        public FileInfo[] GetFileListRecursive()
        {
            return GetFileListRecursive(_connectedPath);
        }

        private FileInfo[] GetFileListRecursive(string path)
        {
            List<FileInfo> ret = new List<FileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            ret.AddRange(dirInfo.GetFiles());
            var dirs = dirInfo.GetDirectories();
            foreach (var d in dirs)
            {
                ret.AddRange(GetFileListRecursive(d.FullName));
            }
            return ret.ToArray();
        }

        /// <summary>
        /// Fetch a file as byte-array
        /// </summary>
        /// <param name="fileInfo">FileInfo of file to get</param>
        /// <returns>Returns binary content data</returns>
        public byte[] GetFileAsByteArray(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                using (FileStream fs = File.OpenRead(fileInfo.FullName))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    return bytes;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the whole file content into a string
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        /// <remarks>MaG, 2013-07-01: Erstellt</remarks>
        public String GetFileContentAsString(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                return File.ReadAllText(fileInfo.FullName);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Ermittelt das letzte Schreibdatum. Existiert die Datei gar nicht, wird DateTime.MinValue zurückgegeben
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public DateTime GetLastModified(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                return File.GetLastWriteTime(fileInfo.FullName);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Writes the whole Filecontent-Stream into a file with the given Filename.
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="Filecontent"></param>
        /// <param name="AppendToFileIfItExists">If set to true -> append if file exists and create if not; if set to false -> overwrite file if it exists and create if not</param>
        /// <returns></returns>
        /// <remarks>MaG, 2013-07-01: Erstellt</remarks>
        public bool WriteFile(String Filename, Stream Filecontent, bool AppendToFileIfItExists)
        {
            var path = Path.Combine(_connectedPath, Filename);

            var filemode = (AppendToFileIfItExists ? FileMode.OpenOrCreate : FileMode.Create);

            using (var filestream = new FileStream(path, filemode, FileAccess.Write))
            {
                Filecontent.CopyTo(filestream);
            }

            return true;
        }

        /// <summary>
        /// deletes files from smb
        /// </summary>
        /// <param name="files">fileinfos of deletable files</param>
        public bool DeleteFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    FileInfo fi = new FileInfo(string.Concat(_connectedPath, @"\", file));
                    if (fi.Exists) fi.Delete();
                }
                catch (SecurityException)
                {
                    // no access
                    throw;
                }
                catch (UnauthorizedAccessException)
                {
                    // file is directory
                    throw;
                }
                catch (IOException)
                {
                    // there's a handle on the file
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public void Dispose()
        {
            PinvokeWindowsNetworking.disconnectRemote(_connectedPath);
        }

    }
}