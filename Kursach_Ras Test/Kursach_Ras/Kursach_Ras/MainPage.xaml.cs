using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static SQLite.SQLite3;

namespace Kursach_Ras
{
    public partial class MainPage : ContentPage
    {
        public static string login1;
        public static int userId;
        public static string name1;
        public MainPage()
        {
            InitializeComponent();
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegistrPage();
        }

        public void SetUserData(User user)
        {
            LoginEntry.Text = user.Login;
            PasswordEntry.Text = user.Password;
        }

        private async void Auth_Clicked(object sender, EventArgs e)
        {
            // Получаем логин и пароль из соответствующих полей ввода
            string login = LoginEntry.Text;
            string password = PasswordEntry.Text;

            // Создаем путь к локальной базе данных SQLite
            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db");
            
            // Создаем асинхронное соединение с базой данных
            var db = new SQLiteAsyncConnection(dbPath);
           
            // Получаем все записи из таблицы User в базе данных
            var data = await db.Table<User>().ToListAsync();
           
            // Фильтруем список записей, чтобы найти запись, где логин и пароль соответствуют введенным значениям
            var query = data.Where(x => x.Login == login && x.Password == password);
            var result = query.FirstOrDefault();


            if (string.IsNullOrEmpty(login))
            {
                //await DisplayAlert("Ошибка", "Логин остался пустым :(", "OK");
                Toaster.ShowShort("Логин остался пустым :(");
            }
            else
            {
                if (string.IsNullOrEmpty(password))
                {
                    //await DisplayAlert("Ошибка", "Пароль остался пустым :(", "OK");
                    Toaster.ShowShort("Пароль остался пустым :(");
                }
                else
                {
                    if (result != null)
                    {
                        //await DisplayAlert("Успешно!", $"Вы авторизованы!", "Ок");
                        login1 = result.Login;
                        name1 = result.Name;
                        userId = result.Id;
                        Application.Current.MainPage = new CustomShellPage();
                    }
                    else
                    {
                        //await DisplayAlert("Ошибка", $"Пользователь не найден.", "Ок");
                        Toaster.ShowShort("Пользователь не найден :(");
                    }
                }
            } 
        }
    }
}
