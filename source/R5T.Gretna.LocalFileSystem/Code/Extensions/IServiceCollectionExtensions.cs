using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Dufftown;
using R5T.Lockerbie;
using R5T.Lombardy;

using R5T.T0063;


namespace R5T.Gretna.LocalFileSystem
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ConfigurationBasedRootDirectoryPathProvider"/> implementation of <see cref="IRootDirectoryPathProvider"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddConfigurationBasedRootDirectoryPathProvider(this IServiceCollection services,
            IServiceAction<IConfiguration> configurationAction)
        {
            services
                .Run(configurationAction)
                .AddSingleton<IRootDirectoryPathProvider, ConfigurationBasedRootDirectoryPathProvider>()
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="LocalFileSystemImageFileRepository"/> implementation of <see cref="IImageFileRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalFileSystemImageFileRepository(this IServiceCollection services,
            IServiceAction<ILocalFileInfoRepository> localFileInfoRepositoryAction,
            IServiceAction<IOriginalFileNameMappingRepository> originalFileNameMappingRepositoryAction,
            IServiceAction<IRootDirectoryPathProvider> rootDirectoryPathProviderAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            services
                .Run(localFileInfoRepositoryAction)
                .Run(originalFileNameMappingRepositoryAction)
                .Run(rootDirectoryPathProviderAction)
                .Run(stringlyTypedPathOperatorAction)
                .AddSingleton<IImageFileRepository, LocalFileSystemImageFileRepository>()
                ;

            return services;
        }
    }
}
