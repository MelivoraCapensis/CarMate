using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
[assembly:Dependency(typeof(MapXamarinForm.WinPhone.FileHelper))]
namespace MapXamarinForm.WinPhone
{
    class FileHelper:IFileHelper
    {
        public async Task<bool> ExistsAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                await localFolder.GetFileAsync(filename);
            }
            catch
            {
                return false;
            }
            return true;

        }
    }
}
