using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
namespace MapXamarinForm.DataTemplateViews
{
    public class CountryListTemplate:ViewCell
    {
        static Label countryLabel;
        public CountryListTemplate()
        {
            countryLabel = new Label();
            countryLabel.SetBinding(Label.TextProperty, "CountryName");
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
                    countryLabel
                }
            };
            return newLayout;
        }
    }
}
