using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
using SQLite.Net.Platform.WindowsPhone8;
namespace MapXamarinForm.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
            string dbPath = FileAccessHelper.GetLocalFilePath("carmate.db3");

            LoadApplication(new MapXamarinForm.App(dbPath, new SQLitePlatformWP8()));
            
        }
    }
}
