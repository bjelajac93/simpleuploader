using Microsoft.AspNetCore.StaticFiles;

namespace SimpleUploaderAPI.Helper
{
    public static class FileContentType
    {
        private static string contentType;

        public static string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}