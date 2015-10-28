using MapXamarinForm.DataTemplateViews;
using MapXamarinForm.ViewModel;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    class RegistrationPage:TabbedPage
    {
        public UserDetailPage UserDetail { get; private set; }
        public CarsDetailPage CarsDetail { get; private set; }
        //public ManegerGaragePage ManagerGarage { get; private set; }
        UserSessionViewModel model;
        //ManegerGarageViewModel mg = new ManegerGarageViewModel();
        public RegistrationPage(UserSessionViewModel model)
        {
           
            this.model = model;
            this.BindingContext = model;
            UserDetail = new UserDetailPage(model);
            CarsDetail = new CarsDetailPage(model);
            //ManagerGarage = new ManegerGaragePage(mg);
            
            this.Children.Add(CarsDetail);
            this.Children.Add(UserDetail);
            //this.Children.Add(ManagerGarage);


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            //get data from server
          
        }
        protected override void OnDisappearing()
        {
           
            base.OnDisappearing();
        }

    }
}
