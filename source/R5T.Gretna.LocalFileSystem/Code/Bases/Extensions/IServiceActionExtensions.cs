using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Dufftown;
using R5T.Lockerbie;
using R5T.Lombardy;

using R5T.T0062;
using R5T.T0063;


namespace R5T.Gretna.LocalFileSystem
{
    public static class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ConfigurationBasedRootDirectoryPathProvider"/> implementation of <see cref="IRootDirectoryPathProvider"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IRootDirectoryPathProvider> AddConfigurationBasedRootDirectoryPathProviderAction(this IServiceAction _,
            IServiceAction<IConfiguration> configurationAction)
        {
            var serviceAction = _.New<IRootDirectoryPathProvider>(services => services.AddConfigurationBasedRootDirectoryPathProvider(
                configurationAction));

            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="LocalFileSystemImageFileRepository"/> implementation of <see cref="IImageFileRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IImageFileRepository> AddLocalFileSystemImageFileRepositoryAction(this IServiceAction _,
            IServiceAction<ILocalFileInfoRepository> localFileInfoRepositoryAction,
            IServiceAction<IOriginalFileNameMappingRepository> originalFileNameMappingRepositoryAction,
            IServiceAction<IRootDirectoryPathProvider> rootDirectoryPathProviderAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = _.New<IImageFileRepository>(services => services.AddLocalFileSystemImageFileRepository(
                localFileInfoRepositoryAction,
                originalFileNameMappingRepositoryAction,
                rootDirectoryPathProviderAction,
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }
    }
}
