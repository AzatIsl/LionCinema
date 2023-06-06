using System.Collections.Generic;
using SQLite;

namespace Kursach_Ras
{
    public class UserRepository
    {
        public SQLiteConnection database;
        public UserRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<User>();
            database.CreateTable<Movie>();
            database.CreateTable<Collection>();
        }
        public IEnumerable<User> GetItems()
        {
            return database.Table<User>().ToList();
        }
        public User GetItem(int id)
        {
            return database.Get<User>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<User>(id);
        }
        public int SaveItem(User item)
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
