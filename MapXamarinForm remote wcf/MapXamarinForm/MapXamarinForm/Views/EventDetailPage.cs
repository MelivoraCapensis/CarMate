using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapXamarinForm.Views
{
    public class EventDetailPage:ContentPage
    {
        Entry priceLitr;//
        Entry eventAmount;//
        Entry eventCost;//
        Entry lastEventOdometer;//
        Entry commentEvent;//
        StackLayout section_gas_event;//
        Map map;//
        Picker category_picker;//
        Picker checked_fuel_category;//
        UserFuelEventDetail model;
        Button button_ok;
       
        public EventDetailPage(UserFuelEventDetail model)
        {
            this.model = model;
            model.PricePerLitr = "";
            this.BindingContext = model;

            priceLitr = new Entry 
            {
                Placeholder="Price per litr"
            };
            priceLitr.SetBinding(Entry.TextProperty, "PricePerLitr");
            
            eventAmount = new Entry 
            {
                Placeholder="Amount litr"
            };
            eventAmount.SetBinding(Entry.TextProperty, "EventAmount");
            commentEvent = new Entry 
            {
                Placeholder="Comment"
            };
            commentEvent.SetBinding(Entry.TextProperty, "CommentEvent");
            eventAmount.TextChanged += (sender, e) => 
            {
                if (this.model.PricePerLitr != null)
                {
                    double price_per_litr;
                    Double.TryParse(this.model.PricePerLitr, out price_per_litr);
                    double amount_litr;
                    Double.TryParse(this.model.EventAmount, out amount_litr);
                    double event_cost = price_per_litr * amount_litr;
                    this.model.EventCost = event_cost.ToString();
                }
            };
            eventCost = new Entry 
            {
                Placeholder="Event Cost"
            };
            eventCost.SetBinding(Entry.TextProperty, "EventCost");
            lastEventOdometer = new Entry 
            {
                Placeholder="Odometr"
            };
            lastEventOdometer.SetBinding(Entry.TextProperty, "LastEventOdometer");
            //fuel_category = new Entry 
            //{
            //    Placeholder="Fuel Category"
            //};
            //fuel_category.SetBinding(Entry.TextProperty, "Filecategoryevent");
            var map_position=new Position(model.CurrentPoi.geometry.location.lat,model.CurrentPoi.geometry.location.lng);
            map=new Map(MapSpan.FromCenterAndRadius(map_position,Distance.FromKilometers(1)))
            {
                IsShowingUser=true,
                HeightRequest=100,
                WidthRequest=100,
                VerticalOptions=LayoutOptions.FillAndExpand
            };
            var pin =new Pin
            {
                Type=PinType.Place,
                Position=map_position,
                Label=model.CurrentPoi.vendor
            };
            map.Pins.Add(pin);
            section_gas_event = new StackLayout { };
            category_picker = new Picker
            {
                Title = "Event Category",
                VerticalOptions = LayoutOptions.Center
            };
            foreach (string category in model.CategoryToEvent.Keys)
            {
                category_picker.Items.Add(category);
            }
            category_picker.SelectedIndexChanged += (sender, e) =>
            {
                if (category_picker.SelectedIndex == -1)
                {
                    model.Filecategoryevent = "1";
                    category_picker.SelectedIndex = 1;
                }
                else
                {
                    string fuelName = category_picker.Items[category_picker.SelectedIndex];
                    model.CategoryIdEvent = model.CategoryToEvent[fuelName];
                    if (model.CategoryIdEvent == 2 || model.CategoryIdEvent == 11)
                    {
                        section_gas_event.Children.Add(checked_fuel_category);
                        section_gas_event.Children.Add(priceLitr);                        
                        section_gas_event.Children.Add(eventAmount);
                        
                    }
                    else
                    {
                        if (section_gas_event.Children.Count > 0)
                        {
                            section_gas_event.Children.RemoveAt(2);
                            section_gas_event.Children.RemoveAt(1);
                            section_gas_event.Children.RemoveAt(0);
                        }
                    }
                }
            };
            checked_fuel_category = new Picker 
            {
                Title="Checked Fuel Category (MapSetting)",
                VerticalOptions = LayoutOptions.Center
            };
            foreach (string fc in model.FuelcategoryToEvent.Keys)
            {
                checked_fuel_category.Items.Add(fc);

            }
            checked_fuel_category.SelectedIndexChanged += (sender, e) =>
            {
                if (checked_fuel_category.SelectedIndex != -1)
                {
                    string fuelcategorykey = checked_fuel_category.Items[checked_fuel_category.SelectedIndex];
                    model.FuelCategoryId = model.FuelcategoryToEvent[fuelcategorykey];
                    model.PricePerLitr = model.CurrentPoi.prices[checked_fuel_category.SelectedIndex].ToString();
                    if (this.model.PricePerLitr != null)
                    {
                        double price_per_litr;
                        Double.TryParse(this.model.PricePerLitr, out price_per_litr);
                        double amount_litr;
                        Double.TryParse(this.model.EventAmount, out amount_litr);
                        double event_cost = price_per_litr * amount_litr;
                        this.model.EventCost = event_cost.ToString();
                    }
                }
                
            };
            button_ok = new Button { Text = "Save", TextColor = Color.White };

           // button_ok.SetBinding(Button.CommandProperty, UserFuelEventDetail.SaveEventCommandPropertyName);

            button_ok.Clicked +=async (sender, e) => {
                await model.ExecuteSaveEventCommand();
                NavigationPage parent = (NavigationPage)this.Parent;
                RootPage root = (RootPage)parent.Parent;
                root.Detail=new NavigationPage(new CarMateEventsPage(new UserFuelEventDetail()));
                //await Navigation.PushAsync(new CarMateEventsPage(new UserFuelEventDetail()));            
            };
            ScrollView scroll = new ScrollView 
            {
                Content=new StackLayout
                {
                    Children={
                        new Label
                        {
                            Text="New Event",
                            FontSize=50,
                            FontAttributes=FontAttributes.Bold,
                            HorizontalOptions=LayoutOptions.Center
                        },
                        map,
                        category_picker,  
                        section_gas_event,                   
                        eventCost,
                        lastEventOdometer,
                        commentEvent,
                        button_ok
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
