using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Dufftown;
using R5T.Lockerbie;
using R5T.Lombardy;


namespace R5T.Gretna.LocalFileSystem
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ConfigurationBasedRootDirectoryPathProvider"/> implementation of <see cref="IRootDirectoryPathProvider"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddConfigurationBasedRootDirectoryPathProvider(this IServiceCollection services,
            IServiceAction<IConfiguration> configurationAction)
        {
            services
                .AddSingleton<IRootDirectoryPathProvider, ConfigurationBasedRootDirectoryPathProvider>()
                .Run(configurationAction)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="ConfigurationBasedRootDirectoryPathProvider"/> implementation of <see cref="IRootDirectoryPathProvider"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IRootDirectoryPathProvider> AddConfigurationBasedRootDirectoryPathProviderAction(this IServiceCollection services,
            IServiceAction<IConfiguration> configurationAction)
        {
            var serviceAction = ServiceAction.New<IRootDirectoryPathProvider>(() => services.AddConfigurationBasedRootDirectoryPathProvider(
                configurationAction));

            return serviceAction;
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
                .AddSingleton<IImageFileRepository, LocalFileSystemImageFileRepository>()
                .Run(localFileInfoRepositoryAction)
                .Run(originalFileNameMappingRepositoryAction)
                .Run(rootDirectoryPathProviderAction)
                .Run(stringlyTypedPathOperatorAction)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="LocalFileSystemImageFileRepository"/> implementation of <see cref="IImageFileRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IImageFileRepository> AddLocalFileSystemImageFileRepositoryAction(this IServiceCollection services,
            IServiceAction<ILocalFileInfoRepository> localFileInfoRepositoryAction,
            IServiceAction<IOriginalFileNameMappingRepository> originalFileNameMappingRepositoryAction,
            IServiceAction<IRootDirectoryPathProvider> rootDirectoryPathProviderAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = ServiceAction.New<IImageFileRepository>(() => services.AddLocalFileSystemImageFileRepository(
                localFileInfoRepositoryAction,
                originalFileNameMappingRepositoryAction,
                rootDirectoryPathProviderAction,
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }
    }
}
