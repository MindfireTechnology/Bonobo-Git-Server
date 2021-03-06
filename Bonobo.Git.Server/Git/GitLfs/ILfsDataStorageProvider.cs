﻿using System;
using System.IO;

namespace Bonobo.Git.Server.Git.GitLfs
{
    ///<summary>Represents a method of storage for LFS objects.</summary>
    public interface ILfsDataStorageProvider
    {
        /// <summary>Creates a writable stream which represents a file to be created in LFS storage.</summary>
        Stream GetWriteStream(string operation, string repositoryName, string oid);
        /// <summary>Creates a readable stream which represents an existing file in LFS storage.</summary>
        Stream GetReadStream(string operation, string repositoryName, string oid);
        /// <summary>Indicates whether the specified LFS resource (file) exists in LFS storage.</summary>
        bool Exists(string repositoryName, string oid);
        /// <summary>Determines if there is sufficient storage space in LFS storage to accommodate the incoming resource(s).</summary>
        bool SufficientSpace(long requiredSpace);
    }
}