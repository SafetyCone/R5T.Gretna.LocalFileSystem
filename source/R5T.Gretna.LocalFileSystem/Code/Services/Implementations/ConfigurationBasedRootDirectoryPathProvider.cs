using System;

using Microsoft.Extensions.Configuration;

using R5T.Sparta;

using R5T.T0064;


namespace R5T.Gretna.LocalFileSystem
{
    [ServiceImplementationMarker]
    public class ConfigurationBasedRootDirectoryPathProvider : IRootDirectoryPathProvider, IServiceImplementation
    {
        public const string ConfigurationPath = "R5T.Gretna.LocalFileSystem:RootDirectoryPath";


        private IConfiguration Configuration { get; }


        public ConfigurationBasedRootDirectoryPathProvider(
            IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public DirectoryPath GetRootDirectoryPath()
        {
            var rootDirectoryPathValue = this.Configuration[ConfigurationBasedRootDirectoryPathProvider.ConfigurationPath];

            var output = DirectoryPath.New(rootDirectoryPathValue);
            return output;
        }
    }
}
