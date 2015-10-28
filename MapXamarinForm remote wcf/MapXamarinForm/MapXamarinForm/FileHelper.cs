using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm
{
    static class FileHelper
    {
        static IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
        public static Task<bool> ExistsAsync(string filename)
        {
            return fileHelper.ExistsAsync(filename);
        }
    }
}
