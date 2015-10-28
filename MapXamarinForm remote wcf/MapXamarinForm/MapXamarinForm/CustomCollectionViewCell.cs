using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm
{
    public class CustomCollectionViewCell:ViewCell
    {
        static Label nameFuel;
        static Switch choiser;
      
        public CustomCollectionViewCell()
        {           
            nameFuel = new Label 
            {
                FontSize=30         
            };
            nameFuel.SetBinding(Label.TextProperty, "Fuelname");
            choiser = new Switch 
            {
                HorizontalOptions=LayoutOptions.EndAndExpand
            };
            choiser.SetBinding(Switch.IsToggledProperty, "IsChecked");
            var viewLayout = CreateLayout();
            View = viewLayout;

        }
        static StackLayout CreateLayout()
        {
            var newlayout = new StackLayout()
            {
                Padding = new Thickness(0, 5),
                Orientation = StackOrientation.Horizontal,
                Children = { 
                    nameFuel,
                    choiser
                }
            };
            return newlayout;            
        }
    }
}
