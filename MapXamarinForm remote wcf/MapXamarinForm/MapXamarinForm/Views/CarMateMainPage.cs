using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MapXamarinForm.Data;
using MapXamarinForm.Models;
using WcfServiceDb;
using MapXamarinForm.Service;
using MapXamarinForm.DataTemplateViews;


namespace MapXamarinForm.Views
{
    public class CarMateMainPage:ContentPage
    {
        ListView listView;
        StackLayout stackLayout;
        public CountryProxy CurrentCountry { set; get; }
        CountryTakerPage countryTakerPage;
        
        public CarMateMainPage()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            
           
            Label header = new Label 
            {
                Text="Check Country",
                FontSize=30,
                FontAttributes=FontAttributes.Bold,
                HorizontalOptions=LayoutOptions.Center
            };           
            listView= new ListView ();
            listView.ItemTemplate = new DataTemplate(typeof(CountryListTemplate));
            stackLayout = new StackLayout
            {
                Children = 
                {
                    header,
                    listView 
                }
            };
            this.Content = stackLayout;
            listView.ItemSelected += (sender,e)=>
            {
                CurrentCountry = (CountryProxy)e.SelectedItem;
                countryTakerPage = new CountryTakerPage(CurrentCountry);
                NavigationPage parent = (NavigationPage)this.Parent;
                RootPage root = (RootPage)parent.Parent;
                root.Detail = new NavigationPage(countryTakerPage);
            };
        }        
        async protected override void OnAppearing()
        {
            base.OnAppearing();
           
            //get data from server
            var task = DataBaseServiceAgent.DoGetCountries();
            var res = await task;
           
            listView.ItemsSource = res;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
