using System;

using R5T.Sparta;

using R5T.T0064;


namespace R5T.Gretna.LocalFileSystem
{
    /// <summary>
    /// Provides the root directory path for local file system storage.
    /// </summary>
    [ServiceDefinitionMarker]
    public interface IRootDirectoryPathProvider : IServiceDefinition
    {
        DirectoryPath GetRootDirectoryPath();
    }
}
