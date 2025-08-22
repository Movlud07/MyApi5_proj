using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Helpers
{
    public class Helper
    {
        public static void DeleteFile(string wwwpath,string middlePath,string imageName)
        {
            if (imageName ==null)
            {
                return;
            }
            string fullPath = Path.Combine(wwwpath,middlePath,imageName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
