using System;

using R5T.Francia;
using R5T.Lockerbie;
using R5T.Magyar.Extensions;
using R5T.Sparta;


namespace R5T.Gretna.LocalFileSystem
{
    public static class ImageFileIdentityExtensions
    {
        public static FileIdentity GetFileIdentity(this ImageFileIdentity imageFileIdentity)
        {
            var fileIdentity = FileIdentity.New(imageFileIdentity.Value);
            return fileIdentity;
        }

        public static string GetUniqueFileNameValue(this ImageFileIdentity imageFileIdentity)
        {
            var uniqueFileNameValue = imageFileIdentity.Value.ToStringStandard();
            return uniqueFileNameValue;
        }

        public static FileName GetUniqueFileName(this ImageFileIdentity imageFileIdentity)
        {
            var uniqueFileNameValue = imageFileIdentity.GetUniqueFileNameValue();

            var uniqueFileName = FileName.New(uniqueFileNameValue);
            return uniqueFileName;
        }
    }
}
