using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.DataTemplateViews
{
    class KeyValueCell:ViewCell
    {
        public KeyValueCell(string key, string value)
        {
            View = new StackLayout()
            {
                Padding = new Thickness(15, 10),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Children = {                    
                    new Label(){               
                        Text=key,
                        TextColor=Color.White,
                        HorizontalOptions=LayoutOptions.StartAndExpand,
                    },                                               
                    new Label(){
                        Text=value,
                        TextColor=Color.Lime,
                        HorizontalOptions=LayoutOptions.EndAndExpand,
                    }
                }
            };
        }
    }
}
