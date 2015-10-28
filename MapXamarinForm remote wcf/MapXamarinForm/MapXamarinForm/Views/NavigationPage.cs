using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    class NavigationFirstPage:ContentPage
    {
        UserSessionViewModel model;
        StackLayout layout;
        Button button1;
        Button button2;
        LoginPage loginPage;
        
        public NavigationFirstPage(UserSessionViewModel model)
        {
            this.model = model;
            BackgroundColor = Color.Transparent;
            Image img = new Image 
            {
                Source=ImageSource.FromFile(Device.OnPlatform("ApplicationIcon.png",
                "ApplicationIcon.png", "Images/ApplicationIcon.png")),
                BackgroundColor=Color.White

            };
            button1 = new Button
            {
                Text = "Go To Login page",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1
            };
            button1.Clicked += (sender, args) => {
                loginPage = new LoginPage(App.UserViewModel);
                NavigationPage parent = (NavigationPage)this.Parent;
                RootPage root = (RootPage)parent.Parent;
                root.Detail = new NavigationPage(loginPage);
               
            };
               
            button2 = new Button
            {
                Text = "Go To Registration page",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1
            };
            //button2.IsEnabled = false;
            button2.Clicked += (sender, args) =>
            {
                NavigationPage parent = (NavigationPage)this.Parent;
                RootPage root = (RootPage)parent.Parent;
                root.Detail = new NavigationPage(new CarMateMainPage());
            };
            layout=new StackLayout
            {
                Children = 
                {
                    img,
                    button1,
                    //button2
                   
                }
            };
            this.Content = layout;
        }
       
        protected override void OnAppearing()
        {
            base.OnAppearing();         
        }
        protected override void OnDisappearing()
        {

            base.OnDisappearing();
        }

    }
}
