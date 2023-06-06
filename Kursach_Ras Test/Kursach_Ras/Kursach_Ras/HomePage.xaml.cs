using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
    public class ImageModel
    {
        public string ImageSource { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            List<ImageModel> images = new List<ImageModel>
            {
                new ImageModel {ImageSource = "Filmfour.jpg"},
                new ImageModel {ImageSource = "Filmtwo.jpg"},
                new ImageModel {ImageSource = "Filmthree.jpg"},

            };

            carouselView.ItemsSource = images;

        }
    }
}