
using System;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class CarListTemplate:ViewCell
    {
        static Image carImage;
        static Label modelLabel;
        static Label brandLabel;
        static Label modificationLabel;
        static Label odometrLabel;
        static Label tankLabel;
        static Label fuelLabel;
        static Label datebuyLabel;
        static Image iSchecked;
        public CarListTemplate()
        {
            carImage = new Image 
            {                
                Source = Device.OnPlatform(
                           ImageSource.FromUri(new Uri("http://xamarin.com/images/index/ide-xamarin-studio.png")),
                           ImageSource.FromFile("ide_xamarin_studio.png"),
                           ImageSource.FromFile("Images/ide-xamarin-studio.png"))
            };

            modelLabel = new Label { FontAttributes = FontAttributes.Bold };
            modelLabel.SetBinding(Label.TextProperty, "Model");
            brandLabel = new Label { FontAttributes = FontAttributes.Bold };
            brandLabel.SetBinding(Label.TextProperty, "Brand");
            modificationLabel = new Label { FontAttributes = FontAttributes.Bold };
            modificationLabel.SetBinding(Label.TextProperty, "Modification");
            odometrLabel = new Label { FontAttributes = FontAttributes.Bold };
            odometrLabel.SetBinding(Label.TextProperty, "Odometr");
            tankLabel = new Label { FontAttributes = FontAttributes.Bold };
            tankLabel.SetBinding(Label.TextProperty, "Tank");
            fuelLabel = new Label { FontAttributes = FontAttributes.Bold };
            fuelLabel.SetBinding(Label.TextProperty, "Fuelcategory");
            datebuyLabel = new Label { FontAttributes = FontAttributes.Bold };
            datebuyLabel.SetBinding(Label.TextProperty, "Datebuy");
            iSchecked = new Image
            {
                Source = Device.OnPlatform(
               ImageSource.FromFile("check.png"),
               ImageSource.FromFile("check.png"),
               ImageSource.FromFile("Images/check.png"))
            };
            iSchecked.SetBinding(Image.IsVisibleProperty, "IsCheked");
            var viewLayout = CreateLayout();
            View = viewLayout;
            
        }
        static Grid CreateLayout()
        {
            var newLayout = new GridWithCustomCarView();
            newLayout.Children.Add(carImage, 0, 1, 0, 6);
            newLayout.Children.Add(new Label { Text = "Model", FontAttributes = FontAttributes.None }, 1, 0);
            newLayout.Children.Add(new Label { Text = "Brand", FontAttributes = FontAttributes.None }, 1, 1);
            newLayout.Children.Add(new Label { Text = "Modification", FontAttributes = FontAttributes.None }, 1, 2);
            newLayout.Children.Add(new Label { Text = "Odometr", FontAttributes = FontAttributes.None }, 1, 3);
            newLayout.Children.Add(new Label { Text = "Tank", FontAttributes = FontAttributes.None }, 1, 4);
            newLayout.Children.Add(new Label { Text = "Fuel", FontAttributes = FontAttributes.None }, 1, 5);
            newLayout.Children.Add(new Label { Text = "Date Buy", FontAttributes = FontAttributes.None }, 1, 6);
            newLayout.Children.Add(modelLabel, 2, 0);
            newLayout.Children.Add(brandLabel, 2, 1);
            newLayout.Children.Add(modificationLabel, 2, 2);
            newLayout.Children.Add(odometrLabel, 2, 3);
            newLayout.Children.Add(tankLabel, 2, 4);
            newLayout.Children.Add(fuelLabel, 2, 5);
            newLayout.Children.Add(datebuyLabel, 2, 6);
            newLayout.Children.Add(iSchecked, 3, 0);
            return newLayout;
        }
    }
    public class GridWithCustomCarView : Grid
    {
        public GridWithCustomCarView()
        { 
            HeightRequest = 200;
            RowSpacing = 2;
            ColumnSpacing = 5;
            RowDefinitions = new RowDefinitionCollection 
            { 
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition(),
            };
            ColumnDefinitions = new ColumnDefinitionCollection 
            {
                new ColumnDefinition{ Width = new GridLength(70) },
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition{Width = new GridLength(50)},
            };
        }
    }
}
