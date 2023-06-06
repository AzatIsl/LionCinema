
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionPage : ContentPage
    {
        public CollectionPage()
        {
            InitializeComponent();
            LoadMovies();
        }



        public void LoadMovies()
        {
            collectionST.Children.Clear();


            var _usersDB = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db"));
            var collection = _usersDB.Table<Collection>();
            var movies = _usersDB.Table<Movie>();

            var data = collection.Where(x => x.UserId == MainPage.userId);


            foreach (Collection c in data)
            {
                Movie f = movies.Where(x => x.Id == c.MovieId).FirstOrDefault();
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
                var button = new Button() { Text = "Удалить из избранного", BackgroundColor = Color.Transparent};
                button.Clicked += (s, e) =>
                {
                    var __usersDB = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db"));
                    __usersDB.Delete(c);

                    LoadMovies();
                };
                // Создаем вертикальный StackLayout, в котором будут располагаться элементы Label и ProgressBar
                var innerStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };
                innerStackLayout.Children.Add(labelName);
                innerStackLayout.Children.Add(labelDescription);
                innerStackLayout.Children.Add(button);

                // Добавляем элемент изображения и вертикальный StackLayout на горизонтальный StackLayout
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(innerStackLayout);

                collectionST.Children.Add(stackLayout);
            }

        }
    }
}