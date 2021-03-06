﻿using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapXamarinForm.Views
{
    public partial class CurrentEventPage : ContentPage
    {
        UserFuelEventDetail model;
        public CurrentEventPage(UserFuelEventDetail model)
        {
            InitializeComponent();
            this.model = model;
            model.PricePerLitr = "";
            this.BindingContext = model;
            priceLitr.SetBinding(Entry.TextProperty, "PricePerLitr");
            eventAmount.SetBinding(Entry.TextProperty, "EventAmount");
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
            eventCost.SetBinding(Entry.TextProperty, "EventCost");
            lastEventOdometer.SetBinding(Entry.TextProperty, "LastEventOdometer");
            var map_position = new Position(model.CurrentPoi.geometry.location.lat, model.CurrentPoi.geometry.location.lng);
            
            //currentmap = new Map(MapSpan.FromCenterAndRadius(map_position, Distance.FromKilometers(1)))
            //{
            //    IsShowingUser = true,
            //    HeightRequest = 100,
            //    WidthRequest = 100,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            currentmap.MoveToRegion(MapSpan.FromCenterAndRadius(map_position, Distance.FromKilometers(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = map_position,
                Label = model.CurrentPoi.vendor
            };
            currentmap.Pins.Add(pin);
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
                        section_gas_event.IsVisible = true;

                    }
                    else
                    {
                        if (section_gas_event.Children.Count > 0)
                        {
                            section_gas_event.IsVisible = false;
                        }
                    }
                }
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
            button_ok.Clicked += async (sender, e) =>
            {
                if (priceLitrValidator.IsValid 
                    && amountValidator.IsValid 
                    && costValidator.IsValid
                    && eventOdometrValidator.IsValid)
                {
                    await model.ExecuteSaveEventCommand();
                    NavigationPage parent = (NavigationPage)this.Parent;
                    RootPage root = (RootPage)parent.Parent;
                    root.Detail = new NavigationPage(new CarMateEventsPage(new UserFuelEventDetail()));

                }           
            };
        }  
    }
}
