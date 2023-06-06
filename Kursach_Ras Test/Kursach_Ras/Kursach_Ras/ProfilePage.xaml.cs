using SQLite;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        public User result;
        public SQLiteAsyncConnection db;
        public ProfilePage ()
		{
			InitializeComponent ();

            FromDBMethod();
        }

        private async void FromDBMethod()
        {
            try
            {
                string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db");

                // Создаем асинхронное соединение с базой данных
                db = new SQLiteAsyncConnection(dbPath);

                // Получаем все записи из таблицы User в базе данных
                var data = await db.Table<User>().ToListAsync();

                // Фильтруем список записей, чтобы найти запись, где логин и пароль соответствуют введенным значениям
                var query = data.Where(x => x.Login == MainPage.login1);
                result = query.FirstOrDefault();
                NameEdit.Text = result.Name;
                LoginEdit.Text = result.Login;
                PasswordEdit.Text = result.Password;
            }
            catch
            {
                NameEdit.Text = "";
            }
            
        }


        private async void ButtonEdit_Clicked(object sender, EventArgs e)
        {

            if(NameEdit.IsReadOnly)
            {
                NameEdit.IsReadOnly = false;
                LoginEdit.IsReadOnly = false;
                PasswordEdit.IsReadOnly = false;
                ButtonEdit.Text = "Нажимте, чтобы принять изменения";
            }
            else
            {
                string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db");

                // Создаем асинхронное соединение с базой данных
                db = new SQLiteAsyncConnection(dbPath);

                // Получаем все записи из таблицы User в базе данных
                var data = await db.Table<User>().ToListAsync();

                // Фильтруем список записей, чтобы найти запись, где логин и пароль соответствуют введенным значениям
                var query = data.Where(x => x.Login == MainPage.login1);
                result = query.FirstOrDefault();

                result.Name = NameEdit.Text;
                result.Login = LoginEdit.Text;
                result.Password = PasswordEdit.Text;
                await db.UpdateAsync(result);
                NameEdit.IsReadOnly = true;
                LoginEdit.IsReadOnly = true;
                PasswordEdit.IsReadOnly = true;
                Toaster.ShowLong("Ваши данные успешно обновлены " + result.Name);
                ButtonEdit.Text = "Нажмите, чтобы изменить";

            }
            
        }
    }
}