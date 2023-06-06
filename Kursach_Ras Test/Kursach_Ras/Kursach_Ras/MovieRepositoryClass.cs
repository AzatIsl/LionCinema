using System.Collections.Generic;
using SQLite;

namespace Kursach_Ras
{
    public class MovieRepository
    {
        SQLiteConnection database;
        public MovieRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Movie>();
        }

        public SQLiteConnection GetDatabase()
        {
            return database;
        }
        public IEnumerable<Movie> GetItems()
        {
            return database.Table<Movie>().ToList();
        }
        public Movie GetItem(int id)
        {
            return database.Get<Movie>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Movie>(id);
        }
        public int AddItem(Movie item)
        {
            return database.Insert(item);
        }
        public int SaveItem(Movie item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
