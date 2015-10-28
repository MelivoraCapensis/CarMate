using System;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

[assembly: Dependency(typeof(MapXamarinForm.Droid.FileHelper))]
namespace MapXamarinForm.Droid
{
    class FileHelper : IFileHelper
    {
        public Task<bool> ExistsAsync(string filename)
        {
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);
            bool exists = File.Exists(filepath);
            return Task<bool>.FromResult(exists);
        }
    }
}