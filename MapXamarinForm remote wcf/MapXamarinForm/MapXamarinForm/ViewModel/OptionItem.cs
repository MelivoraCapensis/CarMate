using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapXamarinForm.ViewModel
{
    public class AccountOptionItem : OptionItem
    {
 
    }
    public class MapOptionItem : OptionItem
    {

    }
    public class CarMateOptionItem : OptionItem
    {

    }
    public class MapSettingOptionItem : OptionItem
    { 
    }
    public class CarEventOptionItem : OptionItem
    {
 
    }
    public class SynchronizeManagerOptionItem : OptionItem
    { 
    }
    public abstract class OptionItem
    {
        public virtual string Title { get { var n = GetType().Name; return n.Substring(0, n.Length - 10); } }
      
        public virtual bool Selected { get; set; }
        public virtual string Icon
        {
            get
            {
                return
                    Title.ToLower().TrimEnd('s') + ".png";
            }
        }
        public ImageSource IconSource { get { return ImageSource.FromFile(Icon); } }
    }
}
