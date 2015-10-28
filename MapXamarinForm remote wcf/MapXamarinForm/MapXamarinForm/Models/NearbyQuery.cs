using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace MapXamarinForm.Models
{
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
    }

    public class OpeningHours
    {
        public bool open_now { get; set; }
        public List<object> weekday_text { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public List<object> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class Place
    {
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
        public List<Photo> photos { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public List<string> types { get; set; }
        public string vicinity { get; set; }
    }

    public class NearbyQuery
    {

        public List<object> html_attributions { get; set; }
        public string next_page_token { get; set; }
        [JsonProperty(PropertyName = "results")]
        public List<Place> Places { get; set; }
        public string status { get; set; }
    }
    public class FuelMarker
    {
       
        bool isChecked;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Fuelname { set; get; }
        public int FuelId { set; get; }
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                    }
                }
            }
        }
    }
    public class NearbyQueryPoi
    {    
        public List<Poi> Pois { get; set; }   
    }
    public class Poi:IEquatable<Poi>
    {
        public Geometry geometry { get; set; }
        public string vendor { get; set; }
        public List<string> fuelcetegory { get; set; }
        public List<double> prices { get; set; }
        public bool Equals(Poi other)
        {
            if (this.geometry.location.lat == other.geometry.location.lat &
                this.geometry.location.lng == other.geometry.location.lng)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }    
}
