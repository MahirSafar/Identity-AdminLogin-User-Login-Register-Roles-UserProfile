using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Pustok.App.Extensions
{
    public static class FileManager
    {
        public static void DeleteFile(string folderPath, string fileName)
        {
            string filePath = Path.Combine("wwwroot/assets/image", folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string SaveFile(this IFormFile file, string folderPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/image", folderPath, fileName);

            using FileStream stream = new(path, FileMode.Create);
            file.CopyTo(stream);
            return fileName;
        }
        public static bool CheckFileSize(this IFormFile file, int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }
        public static bool CheckFileType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
    }
}
