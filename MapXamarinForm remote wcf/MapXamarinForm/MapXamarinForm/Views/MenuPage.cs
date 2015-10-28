using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class MenuPage:ContentPage
    {
        readonly List<OptionItem> OptionItems = new List<OptionItem>();
        public ListView Menu { get; set; }
        public MenuPage()
        {                 
            OptionItems.Add(new AccountOptionItem());
            OptionItems.Add(new CarMateOptionItem());
            OptionItems.Add(new MapOptionItem());            
            OptionItems.Add(new MapSettingOptionItem());
            OptionItems.Add(new SynchronizeManagerOptionItem());
            BackgroundColor = Color.FromHex("333333");

            var layout = new StackLayout { Spacing = 0, VerticalOptions = LayoutOptions.FillAndExpand };

            var label = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Xamarin.Forms.Label
                {
                    TextColor = Color.FromHex("AAAAAA"),
                    Text = "MENU",
                }
            };

            Label lbl = (Xamarin.Forms.Label)label.Content;
            Device.OnPlatform(
                iOS: () => lbl.FontSize = Device.GetNamedSize(NamedSize.Micro, lbl),
                Android: () => lbl.FontSize = Device.GetNamedSize(NamedSize.Medium, lbl)
            );

            layout.Children.Add(label);

            Menu = new ListView
            {
                ItemsSource = OptionItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };

            var cell = new DataTemplate(typeof(DarkTextCell));
            cell.SetBinding(TextCell.TextProperty, "Title");           
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
            cell.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);

            Menu.ItemTemplate = cell;

            layout.Children.Add(Menu);

            Content = layout;
        }
    }
}
