using MapXamarinForm.ViewModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace MapXamarinForm.Views
{
    public class CarsDetailPage:ContentPage
    {
        UserSessionViewModel model;
        public CarsDetailPage(UserSessionViewModel model)
        {
            this.SetValue(Page.TitleProperty, "CarsDetail");
            this.model = model;
            this.BindingContext = model;
            ListView carListView = new ListView
            {
                ItemsSource = App.UserViewModel.Cars,
                ItemTemplate = new DataTemplate(typeof(CarListTemplate))
            };
            carListView.ItemSelected += (sender, e) => 
            {
                var cars =(ObservableCollection<CarViewModel>) carListView.ItemsSource;
                foreach (var c in cars)
                {
                    c.IsCheked = false;
                }
                var carChecked = (CarViewModel)e.SelectedItem;
                carChecked.IsCheked = true;
                App.UserViewModel.Current_car = carChecked;
            };
            Button addCar = new Button
            {
                Text = "Add Car"
               
            };
            addCar.SetBinding(Button.IsEnabledProperty, "User_exist");
            addCar.Clicked += async (sender, e) =>
            {
                ManegerGarageViewModel mg = new ManegerGarageViewModel();
                await mg.InitFuelCategoryToCar();
                await mg.InitTypeCar();
                AccountManagerPage masterpage = (AccountManagerPage)this.Parent;
                NavigationPage parent = (NavigationPage)masterpage.Parent;
                RootPage root = (RootPage)parent.Parent;
                root.Detail = new NavigationPage(new ManegerGaragePage(mg));
            };
            this.Content = new StackLayout
            {
                Children =                 
                    {
                       
                        carListView,
                        addCar                       
                    }
            };
        }
    }
}
