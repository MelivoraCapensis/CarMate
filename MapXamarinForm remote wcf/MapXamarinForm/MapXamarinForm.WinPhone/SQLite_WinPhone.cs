using System;
using System.IO;
using MapXamarinForm;
using MapXamarinForm.WinPhone;
using Xamarin.Forms;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WindowsPhone8;
using System.IO.IsolatedStorage;
using System.Windows;
using SQLite.Net.Async;

[assembly:Dependency(typeof(SQLite_WinPhone))]
namespace MapXamarinForm.WinPhone
{
    public class SQLite_WinPhone:ISQLite
    {
        SQLiteAsyncConnection conn;
        public SQLite_WinPhone()
        { 
        }
        public SQLiteAsyncConnection GetConnection()
        {
            var sqliteFilename = "carmate.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            if (conn == null)
            {
                var connectionFunction = new Func<SQLiteConnectionWithLock>(() =>
                    new SQLiteConnectionWithLock
                        (
                            new SQLitePlatformWP8(),
                            
                            new SQLiteConnectionString(path,storeDateTimeAsTicks:false)
                        ));
                conn = new SQLiteAsyncConnection(connectionFunction);
            }          
            return conn;
        }
        public static void CopyDatabaseIfNotExists(string dbPath)
        {
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storageFile.FileExists(dbPath))
            {
                using (var resourceStream = System.Windows.Application.GetResourceStream(new Uri("carmate.db3", UriKind.Relative)).Stream)
                {
                    using (var fileStream = storageFile.CreateFile(dbPath))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytes = -1;

                        while ((bytes = resourceStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            fileStream.Write(readBuffer, 0, bytes);
                        }
                    }
                }
            }
        }
    }
}
