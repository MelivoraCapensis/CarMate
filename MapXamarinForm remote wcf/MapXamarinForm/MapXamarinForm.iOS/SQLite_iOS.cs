using System;
using MapXamarinForm;
using Xamarin.Forms;
using MapXamarinForm.iOS;
using System.IO;
using Foundation;

[assembly:Dependency(typeof(SQLite_iOS))]
namespace MapXamarinForm.iOS
{
    public class SQLite_iOS:ISQLite
    {
        public SQLite_iOS()
        { 
        }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "carmate.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }
            var path = Path.Combine(libraryPath, sqliteFilename);
            CopyDatabaseIfNotExists(path);

            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
        private static void CopyDatabaseIfNotExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("carmate", "db3");
                File.Copy(existingDb, dbPath);
            }
        }
    }
}