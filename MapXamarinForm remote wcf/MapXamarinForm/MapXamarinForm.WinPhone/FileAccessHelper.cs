using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm.WinPhone
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            string path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string dbPath = Path.Combine(path, filename);           
            return dbPath;
        }
    }
}
