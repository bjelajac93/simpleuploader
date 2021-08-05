using Microsoft.AspNetCore.StaticFiles;

namespace SimpleUploaderAPI.Helper
{
    public static class FileContentType
    {
        public static string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}