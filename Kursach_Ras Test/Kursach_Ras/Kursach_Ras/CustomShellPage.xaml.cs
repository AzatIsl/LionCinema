using Android.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kursach_Ras
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomShellPage : ContentPage
	{
        public static CustomShellPage Instance = null;

        public static CustomShellPage GetInstance()
        {
            if (Instance == null)
                return null;
            return Instance;
        }

        public void SetContent(ContentPage page)
        {
            contentView.Content = page.Content;
        }

		public CustomShellPage ()
		{
			InitializeComponent ();
            Instance = this;
            contentView.Content = new HomePage().Content;
        }

        void leftBarClick(object sender, EventArgs e)
        {
            leftBarParentStackLayout.IsVisible = true;
        }

        private void hideLeftBarTapped(object sender, EventArgs e)
        {
            leftBarParentStackLayout.IsVisible = false;
        }


        private void HomeClick(object sender, EventArgs e)
        {
            contentView.Content = new HomePage().Content;
            leftBarParentStackLayout.IsVisible = false;
        }

        private void CollectionClick(object sender, EventArgs e)
        {
            contentView.Content = new CollectionPage().Content;
            leftBarParentStackLayout.IsVisible = false;
        }


        private void FilmClick(object sender, EventArgs e)
        {
            contentView.Content = new FilmPage().Content;
            leftBarParentStackLayout.IsVisible = false;
        }

        private void ProfileClick(object sender, EventArgs e)
        {
            contentView.Content = new ProfilePage().Content;
            leftBarParentStackLayout.IsVisible = false;
        }

        private void LogoutClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}