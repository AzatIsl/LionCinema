using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.IO;
using System.Reflection;

namespace Kursach_Ras
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "Users.db";
        public static UserRepository database;
        public static UserRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new UserRepository( Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }

 

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new MainPage();



            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.s3db");
            // если база данных не существует (еще не скопирована)
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            // берем из нее ресурс базы данных и создаем из него поток
            using (Stream stream = assembly.GetManifestResourceStream($"Kursach_Ras.{"db.s3db"}"))
            {
                using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(fs);  // копируем файл базы данных в нужное нам место
                    fs.Flush();
                }
            }


            var usersDB = new UserRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db")); 
            var moviesDB = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.s3db"));
            var movies = moviesDB.Table<Movie>();

            usersDB.database.Execute("DELETE FROM sqlite_sequence WHERE name = 'Movie'");
            usersDB.database.Execute("DELETE FROM Movie");
            usersDB.database.Commit();
                        
            foreach(Movie movie in movies)
            {
                usersDB.database.Insert(movie);
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
