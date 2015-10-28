using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class RootPage:MasterDetailPage
    {
        OptionItem previousItem;
        NavigationFirstPage page;
        public RootPage()
        {
            var optionsPage = new MenuPage { Icon = "Images/settings.png", Title = "menu" };
            optionsPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as OptionItem);
            Master = optionsPage;
            NavigateTo(optionsPage.Menu.ItemsSource.Cast<OptionItem>().First());
           
        }
        async void ShowLoginDialog()
        {
            await App.UserViewModel.IsExistsUserAsync();
            if (App.UserViewModel.User_exist == true)
            {
               
                await App.UserViewModel.LoadUserDataAsync();
               
                await App.UserViewModel.LoadUserCarsAsync();
                await App.PlaceViewModel.InitFuelMarker();
                
            }
            if (App.UserViewModel.User_exist == false)
            {
                page = new NavigationFirstPage(App.UserViewModel);
                this.Detail = new NavigationPage(page);
            }
            
        }
        public void NavigateTo(OptionItem option)
        {
            if (previousItem != null)
                previousItem.Selected = false;

            option.Selected = true;
            previousItem = option;
            var displayPage = PageForOption(option);

#if WINDOWS_PHONE
            Detail = new ContentPage();//work around to clear current page.
#endif
            var color = Helpers.Color.Blue.ToFormsColor();
            Detail = new NavigationPage(displayPage)
            {
                BarBackgroundColor = color,
                BarTextColor = color
            };

            IsPresented = false;

        }
        Page PageForOption(OptionItem option)
        {
            if (option.Title == "Account")
                return new AccountManagerPage(App.UserViewModel);
            if (option.Title == "Map")
                return new MapPage(App.PlaceViewModel);
            if (option.Title == "CarMate")
                return new CarMateEventsPage(new UserFuelEventDetail());
            if (option.Title == "MapSetting")             
                return new MapSettingPage(App.PlaceViewModel);
            if (option.Title == "SynchronizeManager")
                return new SynchronizeManagerPage(App.UserViewModel);
            throw new NotImplementedException(string.Format("Unknown menu option: {0}", option.Title));
        }
       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowLoginDialog();

        }
        protected override void OnDisappearing()
        {

            base.OnDisappearing();
        }
    }
}
