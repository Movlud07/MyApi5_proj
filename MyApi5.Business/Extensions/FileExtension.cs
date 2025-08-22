using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Extensions
{
    public static class FileExtension
    {
        public static bool FileSize(this IFormFile file)
        {
            if (file.Length> 2 * 1024 * 1024)
            {
                return false;
            }
            return true;
        }
        public static bool IsImage(this IFormFile file)
        {
            string[] ext = { ".png", ".jpg", ".jpeg" };
            string extension = Path.GetExtension(file.FileName);
            return ext.Any(x => x.Contains(extension));
        }
        public static string PutPlace(this IFormFile file, IWebHostEnvironment env)
        {
            string imageName = Guid.NewGuid() + file.FileName;
            string fullPath = Path.Combine(env.WebRootPath, "uploads/img", imageName);
            using(FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }
            return imageName;
        }

    }
}
