using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;

[assembly:Dependency(typeof(MapXamarinForm.iOS.FileHelper))]
namespace MapXamarinForm.iOS
{
    class FileHelper : IFileHelper
    {
        public Task<bool> ExistsAsync(string filename)
        {
            bool exists = File.Exists(GetFilePath(filename));
            return Task<bool>.FromResult(exists);
        }
        string GetFilePath(string filename)
        {

            return Path.Combine(GetDocsFolder(), filename);
        }
        string GetDocsFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}