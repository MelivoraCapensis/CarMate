using MapXamarinForm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.Views
{
    public class MapMasterPage:TabbedPage
    {
        public MapPage Map { get; set; }
        public MapSettingPage MapSetting { get; set; }
        public MapMasterPage()
        {
            var viewModel = App.PlaceViewModel;
            BindingContext = viewModel;
            
            Map = new MapPage(viewModel);
            MapSetting = new MapSettingPage(viewModel);
            
            this.Children.Add(Map);
            this.Children.Add(MapSetting);
        }
    }
}
