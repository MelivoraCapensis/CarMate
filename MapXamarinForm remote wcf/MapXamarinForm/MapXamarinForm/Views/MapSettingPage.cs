using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class MapSettingPage:ContentPage
    {
        PlaceViewModel model;
        public MapSettingPage(PlaceViewModel model)
        {
            this.SetValue(TitleProperty, "Settings");
            this.model=model;
            
            this.BindingContext = model;

            Label header = new Label
            {
                Text = "Settings",
                FontSize = 50
            };
            Label cafeText = new Label
            {
                Text = "Display cafe,\r\n require an internet connection",
                FontSize=30
            };
            Switch cafechoiser = new Switch();
            cafechoiser.SetBinding(Switch.IsToggledProperty, "IsCafeDisplay");
            Label gasText = new Label
            {
                Text = "Display gas",
                FontSize = 30
            };
            Switch gaschoiser = new Switch() { IsToggled=true};
            gaschoiser.SetBinding(Switch.IsToggledProperty, "IsGasDisplay");
            Label repairText = new Label
            {
                Text = "Display repair",
                FontSize = 30
            };
            Switch repairchoiser = new Switch() { IsEnabled=false};
            repairchoiser.SetBinding(Switch.IsToggledProperty, "IsRepairDisplay");
            ListView fuelMarkers = new ListView
            {
                ItemsSource = model.FuelMarkers,
                ItemTemplate = new DataTemplate(typeof(CustomCollectionViewCell))
            };
           
            Picker radius_picker = new Picker
            {
                Title = "Set the Radius",
                VerticalOptions = LayoutOptions.Center
            };
            foreach (string r in model.RadiusToEvent.Keys)
            {
                radius_picker.Items.Add(r);
            }
            radius_picker.SelectedIndexChanged += (sender, e) =>
            {
                if (radius_picker.SelectedIndex == -1)
                    model.Radius = 1;
                else
                {
                    string radiuskey = radius_picker.Items[radius_picker.SelectedIndex];
                    model.Radius = model.RadiusToEvent[radiuskey];
                }
            };
           // radius_picker.SetBinding(Picker.SelectedIndexProperty, "Radius");
            ScrollView scroll = new ScrollView 
            {
                Content=new StackLayout
                {
                    Children = { 
                        header,
                        cafeText,
                        cafechoiser,
                        gasText,
                        gaschoiser,
                        repairText,
                        repairchoiser,
                        radius_picker,
                        fuelMarkers
                    }
                }
            };
            this.Content = scroll;
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
