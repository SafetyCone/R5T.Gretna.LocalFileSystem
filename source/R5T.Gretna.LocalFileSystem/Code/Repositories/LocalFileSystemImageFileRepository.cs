using System;
using System.IO;

using R5T.Dufftown;
using R5T.Francia;
using R5T.Lockerbie;
using R5T.Lombardy;
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
        }

        public ImageFileIdentity AddImage(Stream imageFileStream, FileName imageFileName, FileFormat fileFormat)
        {
            var imageFileIdentity = ImageFileIdentity.New();

            // Get unique file-name from image file identity.
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();

            // Get the storage file path of the image file, using a single directory path and a globally-unique file name.
            var uniqueImageFilePath = this.GetUniqueImageFilePath(uniqueImageFileName);

            // Write the image file from its input stream to its unique local file path.
            using (var file = File.Open(uniqueImageFilePath.Value, FileMode.CreateNew)) // Throw an exception if the file already exists by using create-new.
            {
                imageFileStream.CopyTo(file);
            }

            // Add the file info to the repository.
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var fileInfo = new FileInfoAppType()
            {
                FileIdentity = fileIdentity,
                FilePath = uniqueImageFilePath,
                FileFormat = fileFormat,
            };

            this.LocalFileInfoRepository.Add(fileInfo);

            // Now add the unique file-name to original file-name mapping.
            this.OriginalFileNameMappingRepository.Add(uniqueImageFileName, imageFileName);

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

        public void Delete(ImageFileIdentity imageFileIdentity)
        {
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();
            var uniqueImageFilePath = this.GetUniqueImageFilePath(uniqueImageFileName);

            // Delete the image file.
            File.Delete(uniqueImageFilePath.Value);

            // Remove the local file info.
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            this.LocalFileInfoRepository.Delete(fileIdentity);

            // Delete the original image name mapping.

            this.OriginalFileNameMappingRepository.Delete(uniqueImageFileName);
        }

        public bool Exists(ImageFileIdentity imageFileIdentity)
        {
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var exists = this.LocalFileInfoRepository.Exists(fileIdentity);
            return exists;
        }

        public FileFormat GetImageFileFormat(ImageFileIdentity imageFileIdentity)
        {
            var fileIdentity = imageFileIdentity.GetFileIdentity();

            var fileFormat = this.LocalFileInfoRepository.GetFileFormat(fileIdentity);
            return fileFormat;
        }

        /// <summary>
        /// This, in context, means the original image file name.
        /// </summary>
        public FileName GetImageFileName(ImageFileIdentity imageFileIdentity)
        {
            // Can get the unique image file-name directly from the image file identity.
            var uniqueImageFileName = imageFileIdentity.GetUniqueFileName();

            var originalImageFileName = this.OriginalFileNameMappingRepository.GetOriginalImageFileName(uniqueImageFileName);
            return originalImageFileName;
        }

        public Stream GetImageFileStream(ImageFileIdentity imageFileIdentity)
        {
            var uniqueImageFilePath = this.GetUniqueImageFilePath(imageFileIdentity);

            var imageFileStream = File.OpenRead(uniqueImageFilePath.Value);
            return imageFileStream;
        }

        public ImageFileReadInfo GetReadInfo(ImageFileIdentity imageFileIdentity)
        {
            var imageFileStream = this.GetImageFileStream(imageFileIdentity);
            var originalImageFileName = this.GetImageFileName(imageFileIdentity);
            var imageFileFormat = this.GetImageFileFormat(imageFileIdentity);

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
