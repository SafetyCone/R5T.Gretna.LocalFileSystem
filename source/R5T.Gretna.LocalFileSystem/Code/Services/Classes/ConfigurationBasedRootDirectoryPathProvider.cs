using System;

using Microsoft.Extensions.Configuration;

using R5T.Sparta;


namespace R5T.Gretna.LocalFileSystem
{
    public class ConfigurationBasedRootDirectoryPathProvider : IRootDirectoryPathProvider
    {
        public const string ConfigurationPath = "R5T.Gretna.LocalFileSystem:RootDirectoryPath";


        private IConfiguration Configuration { get; }


        public ConfigurationBasedRootDirectoryPathProvider(IConfiguration configuration)
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
