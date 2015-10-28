using MapXamarinForm.ViewModel;
using System;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class UserDetailPage:ContentPage
    {
        UserSessionViewModel model;
        static readonly string dateFormat = "D";
        public UserDetailPage(UserSessionViewModel model)
        {
            this.SetValue(Page.TitleProperty, "UserDetail");
            this.model = model;
            this.BindingContext = model;
            Label header = new Label
            {
                Text = "Acount",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            EntryCell fnentryCell = new EntryCell()
            {
                Placeholder = "FirstName",
                Label = "FirstName"
            };
            fnentryCell.SetBinding(EntryCell.TextProperty, "FirstName");


            EntryCell lnentryCell = new EntryCell()
            {
                Placeholder = "LastName",
                Label = "LastName"
            };
            lnentryCell.SetBinding(EntryCell.TextProperty, "LastName");

            EntryCell nickentryCell = new EntryCell()
            {
                Placeholder = "Login",
                Label = "Login"
            };
            nickentryCell.SetBinding(EntryCell.TextProperty, "Nickname");


            EntryCell passentryCell = new EntryCell()
            {
                Placeholder = "Password",
                Label = "Password"
            };
            passentryCell.SetBinding(EntryCell.TextProperty, "Password");
            EntryCell ementryCell = new EntryCell()
            {
                Placeholder = "Email",
                Label = "Email"
            };
            ementryCell.SetBinding(EntryCell.TextProperty, "UserEmail");

            TextCell regtextCell = new TextCell() { Detail = "DateRegister", DetailColor = Color.Aqua };
            regtextCell.SetBinding(TextCell.TextProperty, "DateRegister");
            ImageCell acimage = new ImageCell
            {
                ImageSource = Device.OnPlatform(
                ImageSource.FromUri(new Uri("http://xamarin.com/images/index/ide-xamarin-studio.png")),
                ImageSource.FromFile("ide_xamarin_studio.png"),
                ImageSource.FromFile("Images/ide-xamarin-studio.png")),
                Text = "Image",
            };
            acimage.SetBinding(ImageCell.DetailProperty, "NickName");
            DatePicker register_datePicker = new DatePicker { HorizontalOptions = LayoutOptions.FillAndExpand, Format = dateFormat,IsEnabled=false };
            register_datePicker.SetBinding(DatePicker.DateProperty, "DateRegister");
            TableView tableView = new TableView
            {
                HeightRequest = 1200,
                Intent = TableIntent.Form,
                BindingContext = model,
                Root = new TableRoot("User Data") 
                {
                    new TableSection("Acoount Detail")
                    {
                        fnentryCell,
                        lnentryCell,
                        acimage,                        
                        
                        nickentryCell,
                        passentryCell,
                        ementryCell
                        
                    }
                   
                }
            };
            ToolbarItem addItem = new ToolbarItem
            {
                Name = "Add User",
                Icon = Device.OnPlatform("add.png", "ic_action_add.png", "Images/add.png"),
                Order = ToolbarItemOrder.Primary
            };
            addItem.Activated += async (sender, e) => 
            {
                if (!String.IsNullOrWhiteSpace(model.FirstName) ||
                              !String.IsNullOrWhiteSpace(model.LastName) ||
                              !String.IsNullOrWhiteSpace(model.UserEmail) ||
                              !String.IsNullOrWhiteSpace(model.Password))
                {
                    await model.SaveUserAccountDetail();
                    await App.UserViewModel.IsExistsUserAsync();
                }
            };
            this.ToolbarItems.Add(addItem);
            this.Content = new StackLayout
            {
                Children =                 
                    {
                       
                        tableView,
                        new StackLayout
                        { 
                            Orientation=StackOrientation.Horizontal,
                            Children=
                            {
                                new Label{Text="Data register"},
                                register_datePicker
                            }
                        }                       
                    }
            };
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
