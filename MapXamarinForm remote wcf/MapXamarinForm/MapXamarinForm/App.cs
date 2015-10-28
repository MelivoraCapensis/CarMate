using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MapXamarinForm.Data;
using MapXamarinForm.Views;
using SQLite.Net.Interop;
using Xamarin.Forms;
using MapXamarinForm.ViewModel;
using System.Threading.Tasks;
using MapXamarinForm.Service;
using WcfServiceDb;
using System.Threading;

namespace MapXamarinForm
{
    public class App : Application
    {
        public static GoogleService Service = new GoogleService();
        public static PoiService PService = new PoiService();
        public static GeolocatorService Locator = new GeolocatorService();
     
        public static CarMateDatabase Database
        {
            get;
            private set;
        }
        public static PlaceViewModel PlaceViewModel
        {
            get;
            private set;
        }
        public static UserSessionViewModel UserViewModel
        {
            get;
            private set;
        }
      
        public App(string dbPath, ISQLitePlatform sqlitePlatform)
        {
            PlaceViewModel = new PlaceViewModel();
            UserViewModel = new UserSessionViewModel();
            Database = new CarMateDatabase(sqlitePlatform, dbPath);
            //var mainNav = new NavigationPage(new NavigationFirstPage(UserViewModel));       
            var mainNav = new NavigationPage(new RootPage());
            MainPage = mainNav;
        }
        async protected override void OnStart()
        {
            // Handle when your app starts
            var task = await DataBaseServiceAgent.DoGetCountries();
           
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
