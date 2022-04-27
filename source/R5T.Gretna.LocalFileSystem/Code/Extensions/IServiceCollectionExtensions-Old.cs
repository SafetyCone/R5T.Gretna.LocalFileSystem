using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Dufftown;
using R5T.Lockerbie;
using R5T.Lombardy;


namespace R5T.Gretna.LocalFileSystem
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ConfigurationBasedRootDirectoryPathProvider"/> implementation of <see cref="IRootDirectoryPathProvider"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddConfigurationBasedRootDirectoryPathProvider_Old(this IServiceCollection services,
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
        public static IServiceAction<IRootDirectoryPathProvider> AddConfigurationBasedRootDirectoryPathProviderAction_Old(this IServiceCollection services,
            IServiceAction<IConfiguration> configurationAction)
        {
            var serviceAction = ServiceAction.New<IRootDirectoryPathProvider>(() => services.AddConfigurationBasedRootDirectoryPathProvider_Old(
                configurationAction));

            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="LocalFileSystemImageFileRepository"/> implementation of <see cref="IImageFileRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalFileSystemImageFileRepository_Old(this IServiceCollection services,
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
        public static IServiceAction<IImageFileRepository> AddLocalFileSystemImageFileRepositoryAction_Old(this IServiceCollection services,
            IServiceAction<ILocalFileInfoRepository> localFileInfoRepositoryAction,
            IServiceAction<IOriginalFileNameMappingRepository> originalFileNameMappingRepositoryAction,
            IServiceAction<IRootDirectoryPathProvider> rootDirectoryPathProviderAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = ServiceAction.New<IImageFileRepository>(() => services.AddLocalFileSystemImageFileRepository_Old(
                localFileInfoRepositoryAction,
                originalFileNameMappingRepositoryAction,
                rootDirectoryPathProviderAction,
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }
    }
}
