using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    class LoginPage : ContentPage
    {
        Entry loginEntry;
        Entry passwordEntry;
        Label label;
        StackLayout stackLayout;
        UserSessionViewModel viewmodel;
        CarMateMainPage carmateMainePage;
       
        public LoginPage(UserSessionViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
            var preview = new Label
            {
                Text = "Connect with Your Data",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                XAlign = TextAlignment.Center, // Center the text in the blue box.
                YAlign = TextAlignment.Center, // Center the text in the blue box.
            };
            loginEntry = new Entry 
            {
                Placeholder = "Login"
            };
            passwordEntry = new Entry
            {
                Placeholder = "Password",
                IsPassword=true
            };
            label = new Label {  };
            this.BindingContext = viewmodel;
            loginEntry.SetBinding(Entry.TextProperty, "Nickname");
            passwordEntry.SetBinding(Entry.TextProperty, "Password");
            stackLayout = new StackLayout
            {
                Children = 
                {
                    preview,
                    loginEntry,                   
                    passwordEntry,
                    label
                }
            };
            ToolbarItem cancelItem = new ToolbarItem
            {
                Name = "Cancel",
                Icon = Device.OnPlatform("cancel.png", "ic_action_cancel.png", "Images/cancel.png"),
                Order = ToolbarItemOrder.Primary
            };
            cancelItem.Activated += async (sender, args) =>
            {
                bool confirm = await this.DisplayAlert("Login", "Cancel?", "Yes", "No");
                if (confirm)
                {

                    NavigationPage parent = (NavigationPage)this.Parent;
                    RootPage root = (RootPage)parent.Parent;
                    root.Detail = new NavigationPage(new NavigationFirstPage(App.UserViewModel));
                }
            };
            this.ToolbarItems.Add(cancelItem);
            ToolbarItem addItem = new ToolbarItem
            {
                Name = "Add",
                Icon = Device.OnPlatform("new.png", "ic_action_new.png", "Images/add.png"),
                Order = ToolbarItemOrder.Primary
            };
            addItem.Activated += async (sender, args) =>
            {
                bool confirm = await this.DisplayAlert("Synchronize need Internet connection", "Continue?", "Yes", "No");
                if (confirm)
                {
                    label.Text = "Please wait....\r\n";
                    var task1 = viewmodel.IsServerRegisterAsync();
                    var res1 = await task1;
                    if (res1 == true)
                    {
                        var task2 = viewmodel.GetExistsUserbyServerAsync();
                        var res2 = await task2;
                        var task3 = viewmodel.SynchronizeByUser(res2);
                        var res3 = await task3;
                        label.Text = res3;
                        carmateMainePage = new CarMateMainPage();
                        NavigationPage parent = (NavigationPage)this.Parent;
                        RootPage root = (RootPage)parent.Parent;
                        root.Detail = new NavigationPage(carmateMainePage);
                    }
                    else
                    {

                    }

                }
                else
                {
                    await this.Navigation.PopAsync();
                }
            };
            this.ToolbarItems.Add(addItem);
            this.Content = stackLayout;
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
