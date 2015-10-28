using MapXamarinForm.Models;
using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class SynchronizeManagerPage:ContentPage
    {
        UserSessionViewModel model;
        StackLayout layout;
        Button senddata;
        public SynchronizeManagerPage(UserSessionViewModel model)
        {
            this.SetBinding(TitleProperty, "Synchronize Data Page");
            this.model = model;
            BackgroundColor = Color.Transparent;
            
            senddata = new Button 
            {
                Text="Send Data To Server"
            };
            senddata.Clicked += async(sender, e) => 
            {
                ActivityIndicator activityIndicator = new ActivityIndicator
                {
                    Color = Device.OnPlatform(Color.Black, Color.Default, Color.Default),
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                layout.Children.Add(activityIndicator);
                User current=await App.Database.GetUserAsync();
                if (current.UserId == 0)
                { 

                }
                else
                {

                }
            };
            layout = new StackLayout
            {
                Children = 
                {
                    
                    senddata

                }
            };
            this.Content = layout;
        }
    }
}
