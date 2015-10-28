using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class CarMateEventsPage : ContentPage
    {
        UserFuelEventDetail model;
        public CarMateEventsPage(UserFuelEventDetail model)
        {
            this.model = model;
            this.BindingContext = model;
            Title = "Car";
            var odometr = new Image()
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("odometr.png"),
                ImageSource.FromFile("odometr.png"),
                ImageSource.FromFile("Images/odometr.png"))
            };
            Label lamountcurrentm = new Label
            {

                FontSize = 30,
                //BackgroundColor = Color.Lime,
                TextColor = Color.Aqua,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            lamountcurrentm.SetBinding(Label.TextProperty, "CurrentMounthCost");
            Label datafuelcount = new Label
            {

                FontSize = 25,
                TextColor = Color.Aqua,
                //BackgroundColor = Color.Pink,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand

            };
            datafuelcount.SetBinding(Label.TextProperty, "FuelCount");
            Label datafueladd = new Label
            {
                Text = "7",
                FontSize = 25,
                TextColor = Color.Aqua,
                //BackgroundColor = Color.Pink,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var odometrpart1 = new Image()
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("odpartial1.png"),
                ImageSource.FromFile("odpartial1.png"),
                ImageSource.FromFile("Images/odpartial1.png")),


            };
            var odometrpart2 = new Image()
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("odpartial2.png"),
                ImageSource.FromFile("odpartial2.png"),
                ImageSource.FromFile("Images/odpartial2.png")),

            };
            var odometrpart3 = new Image()
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("odpartial1.png"),
                ImageSource.FromFile("odpartial1.png"),
                ImageSource.FromFile("Images/odpartial1.png")),


            };
            var odometrpart4 = new Image()
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("odpartial2.png"),
                ImageSource.FromFile("odpartial2.png"),
                ImageSource.FromFile("Images/odpartial2.png")),

            };
            RelativeLayout relativeLayout = new RelativeLayout()
            {
                HeightRequest = 100,

            };
            relativeLayout.Children.Add(
                odometr,
                Constraint.Constant(0),//x
                Constraint.Constant(0),//y
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 2;//w 50
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 2;//h 50
                })
            );
            relativeLayout.Children.Add(
                lamountcurrentm,
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return parent.Width / 8 + 20;//x 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return parent.Width / 5;//y 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 2;//w 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 4;//h 50
                })
            );
            relativeLayout.Children.Add(
               odometrpart1,
                 Constraint.RelativeToParent((parent) =>
                 {
                     return parent.Width / 2;//x 50
                 }),
               Constraint.Constant(0),//y
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 2;//w 50
               }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 4;//h 25
               })
           );
            relativeLayout.Children.Add(datafuelcount,
                Constraint.RelativeToView(odometrpart1, (parent, sibling) =>
                {
                    return parent.Width / 2 + parent.Width / 4 - 20;//x 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return parent.Width / 8 + 10;//y 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 2;//w 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 4;//h 50
                }));
            relativeLayout.Children.Add(
               odometrpart2,
                 Constraint.RelativeToParent((parent) =>
                 {
                     return parent.Width / 2;//x
                 }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 8 - 10;//y
               }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 2;
               }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 4;
               })
           );
            relativeLayout.Children.Add(
               odometrpart3,
                 Constraint.RelativeToParent((parent) =>
                 {
                     return parent.Width / 2;
                 }),
                 Constraint.RelativeToParent((parent) =>
                 {
                     return parent.Width / 4 + 10;
                 }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 2;
               }),
               Constraint.RelativeToParent((parent) =>
               {
                   return parent.Width / 4;
               })
           );
            relativeLayout.Children.Add(datafueladd,
                Constraint.RelativeToView(odometrpart3, (parent, sibling) =>
                {
                    return parent.Width / 2 + parent.Width / 4 - 20;//x 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return parent.Width / 2 - 40;//y 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 2;//w 50
                }),
                Constraint.RelativeToView(odometr, (parent, sibling) =>
                {
                    return sibling.Width / 4;//h 50
                }));

            relativeLayout.Children.Add(
                odometrpart4,
                  Constraint.RelativeToParent((parent) =>
                  {
                      return parent.Width / 2;
                  }),
                  Constraint.RelativeToParent((parent) =>
                  {
                      return parent.Width / 4 + 60;
                  }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 2;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 4;
                })
            );

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = 
                {
                    new RowDefinition{ Height=GridLength.Auto},
                    new RowDefinition{ Height=GridLength.Auto},
                    new RowDefinition{ Height=GridLength.Auto},
                    new RowDefinition{ Height=GridLength.Auto},
                    new RowDefinition{ Height=new GridLength(100,GridUnitType.Absolute)},              
                },
                ColumnDefinitions = 
                {
                    new ColumnDefinition{Width=new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition{Width=new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition{Width=new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition{Width=new GridLength(1,GridUnitType.Star)}                   
                }
            };
            Image fuelIcon = new Image
            {
                Source = Device.OnPlatform(
                ImageSource.FromFile("fuel.png"),
                ImageSource.FromFile("fuel.png"),
                ImageSource.FromFile("Images/fuel.png")),
                BackgroundColor = Color.White

            };
            grid.Children.Add(
                fuelIcon
             , 0, 0);
            Label lfueldetail = new Label
            {
                // Text = "WOG\r\n 9.09l\r\n A-92",
                FontSize = 40,
                TextColor=Color.Black,
                HorizontalOptions = LayoutOptions.Center

            };
            lfueldetail.SetBinding(Label.TextProperty, "DetailEvent");
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                lfueldetail
                }
            }, 1, 3, 0, 1);
            Label leventcost = new Label
            {
                // Text = "4190\r\n 20.95",
                FontSize = 50,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center

            };
            leventcost.SetBinding(Label.TextProperty, "EventCost");
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    leventcost
                }
            }, 3, 5, 0, 1);

            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children ={
                    new Label
                    {
                        Text = "Last month",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor=Color.Black
                        //BackgroundColor = Color.Pink
                    }
                }
            }, 0, 3, 1, 2);
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                new Label
                {
                    Text = "Current month",
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor=Color.Black
                   // BackgroundColor = Color.Maroon
                }
                }
            }, 3, 5, 1, 2);
            Label lamountlastm = new Label
            {
                // Text = "1487 y.e.",
                FontSize = 40,
                // BackgroundColor = Color.Fuchsia,
                HorizontalOptions = LayoutOptions.Center,
                TextColor=Color.Black

            };
            lamountlastm.SetBinding(Label.TextProperty, "LastMounthCost");

            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    lamountlastm
                }
            }, 0, 3, 2, 3);
            Label currentm = new Label
            {

                FontSize = 30,
                //BackgroundColor = Color.Lime,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            currentm.SetBinding(Label.TextProperty, "CurrentMounthCost");
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    currentm
                }
            }, 3, 5, 2, 3);
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    new Label
                    {
                        Text = "Rate",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor=Color.Black
                      //  BackgroundColor = Color.Lime
                    }
                }
            }, 0, 3, 3, 4);
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    new Label
                    {
                        Text = "time event ",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor=Color.Black
                      //  BackgroundColor = Color.Fuchsia
                    }
                }
            }, 3, 5, 3, 4);
            Label ldetail = new Label
            {
                //Text = "c 12-11-2014 980 km",
                FontSize = 50,
                //   BackgroundColor = Color.Pink,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Black


            };
            ldetail.SetBinding(Label.TextProperty, "LastEventOdometer");
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    ldetail
                }
            }, 0, 3, 4, 5);
            Label lcardetail = new Label
            {
                //Text = "12-11-2014\r\n 14560 km",
                FontSize = 20,
                // BackgroundColor = Color.Gray,
                HorizontalOptions = LayoutOptions.Center,
                TextColor=Color.Black

            };
            lcardetail.SetBinding(Label.TextProperty, "LastEventDate");
            grid.Children.Add(new StackLayout
            {
                BackgroundColor = Color.White,
                Children = { 
                    lcardetail
                }
            }, 3, 5, 4, 5);

            relativeLayout.Children.Add(
                grid,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width / 2 + 5;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height;
                })
            );

            //ToolbarItem addevent = new ToolbarItem
            //{
            //    Name = "Add Event",
            //    Icon = Device.OnPlatform("new.png", "ic_action_new.png", "Images/add.png"),
            //    Order = ToolbarItemOrder.Primary
            //};

            //addevent.Activated += (sender, args) =>
            //{

            //    this.Navigation.PushAsync(new MapPage(App.PlaceViewModel));
                
            //};


            //this.ToolbarItems.Add(addevent);
           
            this.Content = relativeLayout;
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();

            //get data from server
            if (App.UserViewModel.Cars.Count > 0)
            {
                await model.InitCarEventDetail();
            }
            
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
   
}
