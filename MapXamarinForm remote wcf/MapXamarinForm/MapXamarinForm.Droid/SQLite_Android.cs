using System;
using MapXamarinForm;
using Xamarin.Forms;
using MapXamarinForm.Droid;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using Android.App;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;



[assembly: Dependency(typeof(SQLite_Android))]
namespace MapXamarinForm.Droid
{
    public class SQLite_Android:ISQLite
    {
        SQLiteAsyncConnection conn;
        public SQLite_Android()
        { 
        }
        public SQLiteAsyncConnection GetConnection()
        {
            var sqliteFilename = "carmate.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);           
            var path = Path.Combine(documentsPath, sqliteFilename);
            //CopyDatabaseIfNotExists(path);
            if (conn == null)
            {
                var connectFunction=new Func<SQLiteConnectionWithLock>(()=>
                    new SQLiteConnectionWithLock
                        (
                            new SQLitePlatformAndroid(),
                            new SQLiteConnectionString(path,storeDateTimeAsTicks:false)
                        ));
                conn = new SQLiteAsyncConnection(connectFunction);
            }           
            //var conn = new SQLiteConnection(new SQLitePlatformAndroid(),path);
            return conn;
        }
        private static void CopyDatabaseIfNotExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open("carmate.db3")))
                {
                    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }
        }
    }
}