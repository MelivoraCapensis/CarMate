using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServiceDb;
using Xamarin.Forms;
using MapXamarinForm.Service;
using MapXamarinForm.Models;
using MapXamarinForm.Data;
using System.Threading;

namespace MapXamarinForm.Views
{
    class CountryTakerPage:ContentPage 
    {
        Label label;
        DateTime begin;
        CountryProxy countryProxy;
        StackLayout stackLayout;
        public event EventHandler CountryTakerPageSucceded;
        public CountryTakerPage(CountryProxy countryProxy)
        {
            this.countryProxy = countryProxy;
            begin = DateTime.Now;
            label = new Label { Text = "Please wait....\r\n" };
            this.BindingContext = countryProxy;
            this.SetBinding(ContentPage.TitleProperty, "CountryName");
           
            this.BindingContext = countryProxy;
            ActivityIndicator activityIndicator = new ActivityIndicator
            {
                Color = Device.OnPlatform(Color.Black, Color.Default, Color.Default),
                IsRunning = true,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            stackLayout = new StackLayout
            {
                Children = 
                {                      
                    label,
                    activityIndicator
                }
            };
            this.Content = new ScrollView 
            { 
                Content=stackLayout
            };
                

        }
        private void OnCountryTakerSucceeded()
        {
            if (CountryTakerPageSucceded != null)
            {
                CountryTakerPageSucceded(this, EventArgs.Empty);
            }
        }
        //after Navigation.PushAsync
        async protected override void OnAppearing()
        {
            var task1 = SynchronizationHelper.SynchronizeBy();
            var res1 = await task1;
            label.Text += res1;
           
            var task2 = SynchronizationHelper.SynchronizeByCountry(countryProxy);
            var res2 = await task2;
            label.Text += res2;
            await this.DisplayAlert("Synchronization success", "Show Account Settings", "OK");
            await App.UserViewModel.LoadUserDataAsync();
            await App.UserViewModel.LoadUserCarsAsync();
            await App.PlaceViewModel.InitFuelMarker();
            NavigationPage parent = (NavigationPage)this.Parent;
            RootPage root = (RootPage)parent.Parent;
            root.Detail = new NavigationPage(new AccountManagerPage(App.UserViewModel));

            //bool confirm = await this.DisplayAlert("Synchronization success", "Show Account Settings", "Yes", "No");
            //if (confirm)
            //{
            //    await App.UserViewModel.LoadUserDataAsync();
            //    await App.UserViewModel.LoadUserCarsAsync();
            //    await App.PlaceViewModel.InitFuelMarker();
            //    NavigationPage parent = (NavigationPage)this.Parent;
            //    RootPage root = (RootPage)parent.Parent;
            //    root.Detail = new NavigationPage(new AccountManagerPage(App.UserViewModel));

            //}
            //else
            //{
            //    await App.UserViewModel.LoadUserDataAsync();
            //    await App.UserViewModel.LoadUserCarsAsync();
            //    await App.PlaceViewModel.InitFuelMarker();
            //    NavigationPage parent = (NavigationPage)this.Parent;
            //    RootPage root = (RootPage)parent.Parent;
            //    root.Detail = new NavigationPage(new AccountManagerPage(App.UserViewModel));
            //}
            
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            
            base.OnDisappearing();
        }
        
    }
}
