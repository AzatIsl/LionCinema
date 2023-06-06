
using Microsoft.EntityFrameworkCore.Query;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class FilmPage : ContentPage
    {

        public FilmPage()
        {
            InitializeComponent();
            LoadMovies();
        }



        public void LoadMovies()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db");
            var db = new SQLiteConnection(dbPath); ;
            var data = db.Table<Movie>();


            foreach (Movie f in data)
            {
                var imageStream = new MemoryStream(f.Image);


                TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (s, e) =>
                {
                    Type pageType = Type.GetType($"{typeof(App).Namespace}.{f.Link}"); // получаем тип страницы
                    ContentPage page = (ContentPage)Activator.CreateInstance(pageType); // создаем экземпляр страницы
                    CustomShellPage.GetInstance().SetContent(page);
                };

                Image image = new Image()
                {
                    GestureRecognizers = {
                        recognizer
                    },
                    HeightRequest = 180,
                    WidthRequest = 128,
                    Source = ImageSource.FromStream(() => imageStream)
                };

                // Создаем горизонтальный StackLayout, в котором будут располагаться элементы пользовательского интерфейса
                var stackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(5, 10, 10, 10), };
                var labelName = new Label() { Text = f.Name, FontSize = 18, HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
                var labelDescription = new Label() { Text = f.Description, FontSize = 16, Margin = new Thickness(5, 0, 5, 0), HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
                var button = new Button() { Text = "Добавить в избранное", BackgroundColor = Color.Transparent};

                // Создаем вертикальный StackLayout, в котором будут располагаться элементы Label и ProgressBar
                var innerStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };
                innerStackLayout.Children.Add(labelName);
                innerStackLayout.Children.Add(labelDescription);


                if (db.Table<Collection>().Where(x => x.UserId == MainPage.userId && x.MovieId == f.Id).Count() == 0)
                {
                    button.Clicked += (s, e) =>
                    {

                        var usersDB = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db"));
                        usersDB.Insert(new Collection() { MovieId = f.Id, UserId = MainPage.userId});

                        button.IsEnabled = false;
                        button.Text = "Уже в избранном";
                    };
                }
                else
                {
                    button.IsEnabled = false;
                    button.Text = "Уже в избранном";
                }

                innerStackLayout.Children.Add(button);

                // Добавляем элемент изображения и вертикальный StackLayout на горизонтальный StackLayout
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(innerStackLayout);

                mainST.Children.Add(stackLayout);
            }

        }


    }
}