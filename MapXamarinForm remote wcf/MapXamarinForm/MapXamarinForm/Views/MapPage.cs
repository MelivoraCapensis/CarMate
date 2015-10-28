using MapXamarinForm.Models;
using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapXamarinForm.Views
{
    public class MapPage:ContentPage
    {
        Map map;
        Slider slider;
        private readonly StackLayout stack;
        private PlaceViewModel ViewModel
        {
           get{ return BindingContext as PlaceViewModel;}
        }
        public MapPage(PlaceViewModel model)
        {
            this.SetValue(TitleProperty, "Map");
            BindingContext = model;
            model.IsGasDisplay = true;
            //model.Radius = 2;
            // put the page together
            stack = new StackLayout { Spacing = 0 };           
            Content = stack;
            SetupMap();


            // add the slider
            slider = new Slider(1, 18, 1);
            slider.ValueChanged += (sender, e) =>
            {
                var zoomLevel = e.NewValue; // between 1 and 18
                var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
                Debug.WriteLine(zoomLevel + " -> " + latlongdegrees);
                if (map.VisibleRegion != null)
                    map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
            };
            
          
        }
        private async void SetupMap()
        {
           //execute load places through our viewmodel (which also gets current posotion)
           
            await ViewModel.ExecuteLoadUserDataCommand();
            if(ViewModel.IsCafeDisplay==true)
                await ViewModel.ExecuteLoadPlacesCommand();    
            if(ViewModel.IsGasDisplay==true)
                await ViewModel.ExecuteLoadPoiCommand();
           //create a new Position object with our current coordinates
            var my_position = new Position(App.Locator.Latitude, App.Locator.Longitude);
            map = new Map(MapSpan.FromCenterAndRadius(my_position, Distance.FromKilometers(1)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            if (map.Pins.Count > 0)
                map.Pins.Clear();
            // for debugging output only
            map.PropertyChanged += (sender, e) =>
            {
                Debug.WriteLine(e.PropertyName + " just changed!");
                if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
                    CalculateBoundingCoordinates(map.VisibleRegion);
            };
           
            //loop on the places list and create pins
            foreach (Place p in ViewModel.Places)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(p.geometry.location.lat, p.geometry.location.lng),
                    Label = p.name,
                    Address = p.vicinity
                };

                //ad place pin to map
                map.Pins.Add(pin);
            }
            foreach (Poi p in ViewModel.Pois)
            {
                string fuelcategory = "";
                if (p.fuelcetegory != null)
                {
                    foreach (var ftemp in p.fuelcetegory)
                    {
                        fuelcategory += ftemp + " "; 
                        
                    }
                }
                string temp = "";
                if (p.prices != null)
                {
                    foreach (var ptemp in p.prices)
                    {
                        temp += ptemp + " ";
                    }
                }

                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(p.geometry.location.lat, p.geometry.location.lng),
                    Label = p.vendor+"\r\n"+fuelcategory+"\r\n"+temp,
                    Address = temp
                };
                pin.Clicked+=async(sender,e)=>
                    {
                        //await DisplayAlert("Tapped!", "Pin was tapped", "OK"); 
                        NavigationPage parent = (NavigationPage)this.Parent;                       
                        RootPage root = (RootPage) parent.Parent;

                       
                        Pin current_pin = (Pin)sender;
                        UserFuelEventDetail model = new UserFuelEventDetail();
                        await model.InitCategoryToEvent();                       
                        model.CurrentPoi = new Poi();
                        model.CurrentPoi.geometry = new Geometry();
                        model.CurrentPoi.geometry.location = new Location();
                        model.CurrentPoi.geometry.location.lat = current_pin.Position.Latitude;
                        model.CurrentPoi.geometry.location.lng = current_pin.Position.Longitude;
                        model.CurrentPoi.vendor = current_pin.Label;
                        //model.CarId = this.ViewModel.CarId;
                        model.FuelCategoryId=this.ViewModel.FuelId;
                        string[] pin_data = current_pin.Label.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (pin_data.Count() > 1)
                        {
                            model.PricePerLitr = pin_data[2];
                            string[] pin_fuelcategories = pin_data[1].Split(new string[] { " " }, StringSplitOptions.None);
                            string[] pin_prices = pin_data[2].Split(new string[] { " " }, StringSplitOptions.None);
                            model.CurrentPoi.fuelcetegory = pin_fuelcategories.ToList();
                            model.CurrentPoi.prices = new List<double>();
                            foreach(var pin_price in pin_prices.ToList())
                            {
                                double convert_price;
                                Double.TryParse(pin_price, out convert_price);
                                model.CurrentPoi.prices.Add(convert_price);
                            }  
                        }
                        await model.InitFuelCategoryToEvent();
                        root.Detail = new NavigationPage(new CurrentEventPage(model));
                        

                    };
                map.Pins.Add(pin);
            }
            stack.Children.Add(map);
            stack.Children.Add(slider);

        }
        /// <summary>
        /// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
        /// Useful if you need to send the bounds to a web service or otherwise calculate what
        /// pins might need to be drawn inside the currently visible viewport.
        /// </summary>
        static void CalculateBoundingCoordinates(MapSpan region)
        {
            // WARNING: I haven't tested the correctness of this exhaustively!
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            Debug.WriteLine("Bounding box:");
            Debug.WriteLine("                    " + top);
            Debug.WriteLine("  " + left + "                " + right);
            Debug.WriteLine("                    " + bottom);
       }
    }
}
