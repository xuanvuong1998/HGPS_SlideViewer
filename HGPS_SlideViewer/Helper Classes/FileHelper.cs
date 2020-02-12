using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public class FileHelper
    {
        public static string DropboxPath { get; set; }
        public static string BasePath { get; set; }

        static FileHelper()
        {
            //BasePath = GetGoogleDriveDirectory() + @"\Lessons\";
            DropboxPath = GetDropboxDirectory();
            BasePath = DropboxPath + @"\Lessons\";
        }
        public static string GetGoogleDriveDirectory()
        {
            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Drive\\sync_config.db");
            if (!File.Exists(dbFilePath))
                dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Drive\\user_default\\sync_config.db");

            string csGdrive = @"Data Source=" + dbFilePath + ";Version=3;New=False;Compress=True;";
            SQLiteConnection con = new SQLiteConnection(csGdrive);
            con.Open();
            SQLiteCommand sqLitecmd = new SQLiteCommand(con);

            sqLitecmd.CommandText = "select * from data where entry_key='local_sync_root_path'";

            SQLiteDataReader reader = sqLitecmd.ExecuteReader();
            reader.Read();
            var path = reader["data_value"].ToString().Substring(4);
            con.Dispose();

            return path;
        }
        public static string GetDropboxDirectory()
        {
            var infoPath = @"Dropbox\info.json";

            var jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), infoPath);

            if (!File.Exists(jsonPath)) jsonPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), infoPath);

            if (!File.Exists(jsonPath)) throw new Exception("Dropbox could not be found!");

            var dropboxPath = File.ReadAllText(jsonPath).Split('\"')[5].Replace(@"\\", @"\");

            return dropboxPath;
        }
    }
}
