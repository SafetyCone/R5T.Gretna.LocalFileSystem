using System;

using R5T.Sparta;


namespace R5T.Gretna.LocalFileSystem
{
    /// <summary>
    /// Provides the root directory path for local file system storage.
    /// </summary>
    public interface IRootDirectoryPathProvider
    {
        DirectoryPath GetRootDirectoryPath();
    }
}
