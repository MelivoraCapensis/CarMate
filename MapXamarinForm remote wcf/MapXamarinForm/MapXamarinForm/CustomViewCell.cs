using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm
{
    public class CustomViewCell:ViewCell
    {
        static Label textlable;
        static Switch choiser;
        public CustomViewCell(string text, string path)
        {
            textlable = new Label 
            {
                FontSize=30,
                Text = text
            };
            choiser = new Switch();
            choiser.SetBinding(Switch.IsToggledProperty, path);
            var viewLayout = CreateLayout();
            View = viewLayout;

        }
        static StackLayout CreateLayout()
        {
            var newLayout = new StackLayout()
            {
                Padding = new Thickness(0, 5),
                Orientation = StackOrientation.Horizontal,
                Children = 
                {
                    textlable,
                    choiser
                }
            };
            return newLayout;
        }
    }
}
