using SQLite;
using System;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrPage : ContentPage
    {
        public RegistrPage()
        {
            InitializeComponent();

        }

        private void Auth_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        private void Registr_Clicked(object sender, EventArgs e)
        {
            string login = LoginEntry.Text;
            string password = PasswordEntry.Text;            

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && password.Equals(Password2Entry.Text))
            {
                App.Database.SaveItem(new User() { Login = login, Password = password });
                User user = new User() { Login = login, Password = password };
                //await DisplayAlert("Успешно!", "Вы зарегистрированы!", "Ок");
                Toaster.ShowShort("Вы зарегистрированы!");
                MainPage page = new MainPage();
                page.SetUserData(user);
                Application.Current.MainPage = page;
            }         
            else
            {
                if (string.IsNullOrEmpty(LoginEntry.Text))
                {
                    //await DisplayAlert("Ошибка", "Логин остался пустым :(", "Ок");
                    Toaster.ShowShort("Логин остался пустым :(");
                }
                else
                {
                    if (string.IsNullOrEmpty(PasswordEntry.Text))
                    {
                        //await DisplayAlert("Ошибка", "Пароль остался пустым :(", "Ок");
                        Toaster.ShowShort("Пароль остался пустым :(");
                    }
                    else
                    {
                        if (PasswordEntry.Text != Password2Entry.Text)
                        {
                            //await DisplayAlert("Ошибка", "Пароли не совпадают :(", "Ок");
                            Toaster.ShowShort("Пароли не совпадают :(");
                        }
                    }
                }
            }
        }
    }
}