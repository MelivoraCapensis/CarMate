using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm
{
    public static class Constants
    {
        public const string APIKey = "AIzaSyDDtXg27XRV-i7-1S83jo9CQePbHI9tf6o";//YOUR_API_KEY
        public const string PlacesQueryUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?types=cafe&location={0},{1}&opennow=true&rankby=distance&key={2}";
    }
}
