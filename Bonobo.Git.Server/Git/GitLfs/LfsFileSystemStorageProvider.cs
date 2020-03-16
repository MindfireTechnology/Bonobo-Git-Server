using Bonobo.Git.Server.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bonobo.Git.Server.Git.GitLfs
{
    public class LfsFileSystemStorageProvider : ILfsDataStorageProvider
    {
        private string DetermineRootDataPath()
        {
            string lfsPath;
            if (!string.IsNullOrEmpty(UserConfiguration.Current.LfsPath))
                lfsPath = UserConfiguration.Current.LfsPath;
            else
                lfsPath = UserConfiguration.Current.RepositoryPath;

            if (!Directory.Exists(lfsPath))
                Directory.CreateDirectory(lfsPath);

            if (Directory.Exists(lfsPath))
                return lfsPath;

            throw new DirectoryNotFoundException("Unable to determine LfsPath.");
        }

        private string DetermineFilename(string repositoryName, string oid)
        {
            string appDataPath = DetermineRootDataPath();
            string firstTwo = (oid + "xx").Substring(0, 2).ToLower();
            string secondTwo = (oid + "xxyy").Substring(2, 2).ToLower();
            var filename = Path.Combine(appDataPath, repositoryName, "lfs", "objects", firstTwo, secondTwo, oid);
            return filename;
        }

        public Stream GetWriteStream(string operation, string repositoryName, string oid)
        {

            string filename = DetermineFilename(repositoryName, oid);
            string directoryName = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            return new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        public Stream GetReadStream(string operation, string repositoryName, string oid)
        {
            string filename = DetermineFilename(repositoryName, oid);

            if (File.Exists(filename))
                return new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            else
                throw new FileNotFoundException("File not found.", filename);
        }

        public bool Exists(string repositoryName, string oid)
        {
            string filename = DetermineFilename(repositoryName, oid);

            return File.Exists(filename);
        }

        public bool SufficientSpace(long requiredSpace)
        {
            var path = DetermineRootDataPath();
            var di = new DriveInfo(path);
            return di.AvailableFreeSpace > requiredSpace;
        }
    }
}