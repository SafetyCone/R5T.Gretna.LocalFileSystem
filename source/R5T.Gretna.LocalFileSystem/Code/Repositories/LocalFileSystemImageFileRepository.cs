using System;
using System.IO;
using System.Threading.Tasks;

using R5T.Dufftown;
using R5T.Francia;
using R5T.Lockerbie;
using R5T.Lombardy;
using R5T.Magyar.IO;
using R5T.Philippi;
using R5T.Sparta;

using FileInfoAppType = R5T.Lockerbie.FileInfo;


namespace R5T.Gretna.LocalFileSystem
{
    public class LocalFileSystemImageFileRepository : IImageFileRepository
    {
        private IRootDirectoryPathProvider RootDirectoryPathProvider { get; }
        private ILocalFileInfoRepository LocalFileInfoRepository { get; }
        private IStringlyTypedPathOperator StringlyTypedPathOperator { get; }
        private IOriginalFileNameMappingRepository OriginalFileNameMappingRepository { get; }


        public LocalFileSystemImageFileRepository(
            IRootDirectoryPathProvider rootDirectoryPathProvider,
            ILocalFileInfoRepository localFileInfoRepository,
            IStringlyTypedPathOperator stringlyTypedPathOperator,
            IOriginalFileNameMappingRepository originalFileNameMappingRepository)
        {
            this.RootDirectoryPathProvider = rootDirectoryPathProvider;
            this.LocalFileInfoRepository = localFileInfoRepository;
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;
            this.OriginalFileNameMappingRepository = originalFileNameMappingRepository;

            this.Setup();
        }

        private void Setup()
        {
            var rootDirectoryPath = this.RootDirectoryPathProvider.GetRootDirectoryPath();

            DirectoryHelper.CreateDirectoryOkIfExists(rootDirectoryPath.Value);
        }

        public async Task<ImageFileIdentity> AddImage(Stream imageFileStream, FileName imageFileName, FileFormat fileFormat)
        {
            var imageFileIdentity = ImageFileIdentity.New();

            // Get unique file-name from image file identity.
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();

            // Get the storage file path of the image file, using a single directory path and a globally-unique file name.
            var uniqueImageFilePath = this.GetUniqueImageFilePath(uniqueImageFileName);

            // Write the image file from its input stream to its unique local file path.
            using (var file = File.Open(uniqueImageFilePath.Value, FileMode.CreateNew)) // Throw an exception if the file already exists by using create-new.
            {
                await imageFileStream.CopyToAsync(file);
            }

            // Add the file info to the repository.
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var fileInfo = new FileInfoAppType()
            {
                FileIdentity = fileIdentity,
                FilePath = uniqueImageFilePath,
                FileFormat = fileFormat,
            };

            await this.LocalFileInfoRepository.Add(fileInfo);

            // Now add the unique file-name to original file-name mapping.
            await this.OriginalFileNameMappingRepository.Add(uniqueImageFileName, imageFileName);

            // Return the image file identity.
            return imageFileIdentity;
        }

        private FilePath GetUniqueImageFilePath(FileName uniqueImageFileName)
        {
            var rootDirectoryPath = this.RootDirectoryPathProvider.GetRootDirectoryPath();

            var uniqueImageFilePathValue = this.StringlyTypedPathOperator.GetFilePath(rootDirectoryPath.Value, uniqueImageFileName.Value);

            var uniqueImageFilePath = FilePath.New(uniqueImageFilePathValue);
            return uniqueImageFilePath;
        }

        private FilePath GetUniqueImageFilePath(ImageFileIdentity imageFileIdentity)
        {
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();

            var uniqueImageFilePath = this.GetUniqueImageFilePath(uniqueImageFileName);
            return uniqueImageFilePath;
        }

        public async Task Delete(ImageFileIdentity imageFileIdentity)
        {
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();
            var uniqueImageFilePath = this.GetUniqueImageFilePath(uniqueImageFileName);

            // The multiple tasks are sequentially awaited to allow failures to preserve the sequence of events in case of failure.
            // (The alternative would have been to Task.WhenAll() the various tasks, which would be simultaneous, but would lead to situations where things might get out of whack in case of failure.)

            // Delete the image file.
            await FileHelper.DeleteAsync(uniqueImageFilePath.Value);

            // Remove the local file info.
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            await this.LocalFileInfoRepository.Delete(fileIdentity);

            // Delete the original image name mapping.

            await this.OriginalFileNameMappingRepository.Delete(uniqueImageFileName);
        }

        public async Task<bool> Exists(ImageFileIdentity imageFileIdentity)
        {
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var exists = await this.LocalFileInfoRepository.Exists(fileIdentity);
            return exists;
        }

        public async Task<FileFormat> GetImageFileFormat(ImageFileIdentity imageFileIdentity)
        {
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var fileFormat = await this.LocalFileInfoRepository.GetFileFormat(fileIdentity);
            return fileFormat;
        }

        /// <summary>
        /// This, in context, means the original image file name.
        /// </summary>
        public async Task<FileName> GetImageFileName(ImageFileIdentity imageFileIdentity)
        {
            // Can get the unique image file-name directly from the image file identity.
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();

            var originalImageFileName = await this.OriginalFileNameMappingRepository.GetOriginalImageFileName(uniqueImageFileName);
            return originalImageFileName;
        }

        public Task<Stream> GetImageFileStream(ImageFileIdentity imageFileIdentity)
        {
            var uniqueImageFilePath = this.GetUniqueImageFilePath(imageFileIdentity);

            var imageFileStream = File.OpenRead(uniqueImageFilePath.Value);
            return Task.FromResult<Stream>(imageFileStream);
        }

        public async Task<ImageFileReadInfo> GetReadInfo(ImageFileIdentity imageFileIdentity)
        {
            var getImageFileStream = this.GetImageFileStream(imageFileIdentity);
            var getImageFileName = this.GetImageFileName(imageFileIdentity);
            var getImageFileFormat = this.GetImageFileFormat(imageFileIdentity);

            var imageFileStream = await getImageFileStream;
            var originalImageFileName = await getImageFileName;
            var imageFileFormat = await getImageFileFormat;

            var readInfo = new ImageFileReadInfo()
            {
                Identity = imageFileIdentity,
                Stream = imageFileStream,
                FileName = originalImageFileName,
                FileFormat = imageFileFormat,
            };

            return readInfo;
        }
    }
}
