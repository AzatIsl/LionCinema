using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kursach_Ras
{
    class Toaster
    {
        public static void ShowLong(String message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }


        public static void ShowShort(String message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}
