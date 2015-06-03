using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.DAL;
using System.Net;
using CarMate.Classes;
using CarMate.Models;


namespace CarMate.Controllers
{
    public class MapController : Controller
    {
        private CarMateEntities db = new CarMateEntities();
       
        string formatted_address = "";
        //
        // GET: /Map/
        static double  PIx = 3.141592;
        static double RADIO = 6378.16;
        /// Convert degrees to Radians    
        ///  
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }
        /// Calculate the distance between two places.
        public  static double DistanceBetweenPlaces(
           double lon1,
           double lat1,
           double lon2,
           double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (angle * RADIO) ;//distance in miles
        }
        //запрос к API геокодирования 
        public string GetforrmatedAdress(double lat,double lng)
        {
            
            string url = string.Format(
                "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=true_or_false&language=ru",
                lat.ToString().Replace(",", "."), lng.ToString().Replace(",", "."));
            //выполняем запрос к универсальному коду ресурса (URI)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Proxy = MyProxy.CreateProxy();
            request.Method = "GET";
            //получаем ответ от интернет-ресурса
            WebResponse response = request.GetResponse();
            //чтение ресурса
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            //документ xml

            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.LoadXml(responsereader);
            string city = "";
            string region = "";

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                formatted_address =
                    xmldoc.SelectNodes("//formatted_address").Item(0).InnerText.ToString();
                string[] words = formatted_address.Split(',');
                city = xmldoc.GetElementsByTagName("short_name")[3].ChildNodes[0].InnerText;
                region = xmldoc.GetElementsByTagName("short_name")[2].ChildNodes[0].InnerText;

            }
            return formatted_address;             
        }

        private IPlacemarksRepository placemarksRepository;
        private ICategoryRepository categoryRepository;
        private ICountriesRepository countriesRepository;
        private IRegionRepository regionRepository;

        public MapController()
        {
            this.placemarksRepository = new PlacemarksRepository(new CarMateEntities());
            this.categoryRepository = new CategoryRepository(new CarMateEntities());
            this.countriesRepository = new CountriesRepository(new CarMateEntities());
            this.regionRepository = new RegionRepository(new CarMateEntities());
        }

        public MapController(IPlacemarksRepository placemarksRepository,
            ICategoryRepository categoryRepository,
            ICountriesRepository countriesRepository,
            IRegionRepository regionRepository)
        {
            this.placemarksRepository = placemarksRepository;
            this.categoryRepository = categoryRepository;
            this.countriesRepository = countriesRepository;
            this.regionRepository = regionRepository;
        }
        //
        // GET: /Map/Details/5

       
        public ActionResult GetMap()
        {
            Squirrel s = new Squirrel();
            return View(s);
        }
        
        [HttpPost]
        public JsonResult PostSquirrel(Squirrel poi)
        {
            List<Squirrel> markers = new List<Squirrel>();          
            int categoryId;
            Int32.TryParse(poi.Category, out categoryId);
            double radius;
            string r, l1, l2;
            List<int> fuelcategoryid=new List<int>();
            //r = poi.Radius.Replace('.', ',');
            //l1 = poi.Lat.Replace('.', ',');
            //l2 = poi.Long.Replace('.', ',');
            r = poi.Radius;
            l1 = poi.Lat;
            l2 = poi.Long;   
            Double.TryParse(r, out radius);
            double latitude;
            Double.TryParse(l1, out latitude);
            double longitude;
            Double.TryParse(l2, out longitude);
            if (poi.FuelCategories == null)
            {            
                var placemarks = from p in placemarksRepository.GetPlacemarks()
                             select p;            
                placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();                  
                foreach (var p in placemarks)
                {
                    if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                    {
                        List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();//выбор цен по точке
                        List<double?> pricesSquirrel = new List<double?>();
                        List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();//выбор категорий по стране
                        foreach (var v in prices)
                        {
                            pricesSquirrel.Add(v);
                        }
                        formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                        markers.Add(new Squirrel()
                        {
                            Lat = p.Latitude.ToString(),
                            Long = p.Longitude.ToString(),
                            Vendore = p.Description,
                            Adress = formatted_address,
                            Prices = pricesSquirrel.ToArray(),//формируем ответ по ценам
                            FuelCategories = fuelcatsSquirrel.ToArray()//формируем ответ по категориям 
                        });
                    }
                }
                
            }
            else if (poi.FuelCategories != null && poi.Gascheaper==null)
            {
                if (poi.FuelCategories.Length > 0)
                {
                    for (int i = 0; i < poi.FuelCategories.Length; i++)
                    {
                        int fcid;
                        Int32.TryParse(poi.FuelCategories[i], out fcid);
                        fuelcategoryid.Add(fcid);
                    }

                }
                var placemarks = from p in placemarksRepository.GetPlacemarks()
                                 select p;
                placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();
                foreach (var p in placemarks)
                {
                    if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                    {
                        
                            List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();//выбор цен по точке
                            if (prices.Count == 0)
                                continue;
                            for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                            { 
                                int tempId=fuelcategoryid[i];
                                if (prices[tempId-1] != 0)
                                {
                                    List<double?> pricesSquirrel = new List<double?>();
                                    List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();//выбор категорий по стране
                                    foreach (var v in prices)
                                    {
                                        pricesSquirrel.Add(v);
                                    }
                                    formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                                    Squirrel temp = new Squirrel()
                                    {
                                        Lat = p.Latitude.ToString(),
                                        Long = p.Longitude.ToString(),
                                        Vendore = p.Description,
                                        Adress = formatted_address,
                                        Prices = pricesSquirrel.ToArray(),//формируем ответ по ценам
                                        FuelCategories = fuelcatsSquirrel.ToArray()//формируем ответ по категориям
                                    };
                                    if (!markers.Contains(temp))
                                        markers.Add(temp);//проверяем чтобы точки заправок по разным категориям не дублировались
                                }
                            }                 
                    }
                }           
            }
            else if (poi.FuelCategories != null && poi.Gascheaper != null)
            {
                if (poi.FuelCategories.Length > 0)
                {
                    for (int i = 0; i < poi.FuelCategories.Length; i++)
                    {
                        int fcid;
                        Int32.TryParse(poi.FuelCategories[i], out fcid);
                        fuelcategoryid.Add(fcid);
                    }

                }
                var placemarks = from p in placemarksRepository.GetPlacemarks()
                                 select p;
                placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();
                foreach (var p in placemarks)
                {
                    if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                    {

                        List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();//выбор цен по точке
                        if (prices.Count == 0)
                            continue;
                        for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                        {
                            int tempId = fuelcategoryid[i];
                            if (prices[tempId - 1] != 0)
                            {
                                List<double?> pricesSquirrel = new List<double?>();
                                List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();//выбор категорий по стране
                                foreach (var v in prices)
                                {
                                    pricesSquirrel.Add(v);
                                }
                                formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                                Squirrel temp = new Squirrel()
                                {
                                    Lat = p.Latitude.ToString(),
                                    Long = p.Longitude.ToString(),
                                    Vendore = p.Description,
                                    Adress = formatted_address,
                                    Prices = pricesSquirrel.ToArray(),//формируем ответ по ценам
                                    FuelCategories = fuelcatsSquirrel.ToArray()//формируем ответ по категориям
                                };
                                if (!markers.Contains(temp))
                                    markers.Add(temp);//проверяем чтобы точки заправок по разным категориям не дублировались
                            }
                        }
                    }
                }
                List<VendorItem> vendors = new List<VendorItem>();
                foreach (var m in markers)
                {
                    VendorItem vtemp = new VendorItem()
                    {
                        Name = m.Vendore,
                        Categories = m.FuelCategories,
                        Prices = m.Prices
                    };
                    if (!vendors.Contains(vtemp) & vtemp.Prices != null)
                    {
                        vendors.Add(vtemp);
                    }
                }
                List<CheaperFuelItem> cheaperfuels = new List<CheaperFuelItem>();
                List<Squirrel> tempmarkers = new List<Squirrel>();
                List<Squirrel> tempmarkers2 = new List<Squirrel>();  
                for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                {
                    foreach (var v in vendors)
                    {
                        int tempId = fuelcategoryid[i];//выбраная категория топлива
                        CheaperFuelItem fuelitem = new CheaperFuelItem()
                        {
                            Name = v.Name,
                            Price = Convert.ToDouble(v.Prices[tempId])
                        };
                        cheaperfuels.Add(fuelitem);
                    }
                    cheaperfuels.Sort();
                    tempmarkers=markers.Where(x => x.Vendore == cheaperfuels[0].Name).Select(x => x).ToList();
                    tempmarkers2.AddRange(tempmarkers);
                }
                markers = tempmarkers2;
            }
            return Json(markers, JsonRequestBehavior.AllowGet);
        }
       
       //выбор точек по маршруту
        [HttpPost]
        public JsonResult PostlstSquirrel(List<Squirrel> incominglstSquirrel)
        {

            #region получаем категорию и радиус
            string r, l1, l2;
            Squirrel item = incominglstSquirrel[0];
            int categoryId;
            Int32.TryParse(item.Category, out categoryId);
            double radius;
            //r = item.Radius.Replace('.', ',');
            r = item.Radius;
            Double.TryParse(r, out radius);
            #endregion 
            List<Squirrel> markers = new List<Squirrel>();//список маркеров по путевым точкам
            List<int> fuelcategoryid = new List<int>();
            if (item.FuelCategories == null)
            {

                // выбор из базы данных пои точек по категории и отбор по радиусу

                var placemarks = from p in placemarksRepository.GetPlacemarks()
                                 select p;
                placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();


                foreach (var poisqurrel in incominglstSquirrel)
                {
                    double latitude;
                    //l1 = poisqurrel.Lat.Replace('.', ',');
                    //l2 = poisqurrel.Long.Replace('.', ',');
                    l1 = poisqurrel.Lat;
                    l2 = poisqurrel.Long;
                    Double.TryParse(l1, out latitude);
                    double longitude;
                    Double.TryParse(l2, out longitude);
                    foreach (var p in placemarks)
                    {

                        if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                        {

                            List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();
                            List<double?> pricesSquirrel = new List<double?>();
                            List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();
                            foreach (var v in prices)
                            {
                                pricesSquirrel.Add(v);
                            }
                            formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                            Squirrel temp = new Squirrel()
                            {
                                Lat = p.Latitude.ToString(),
                                Long = p.Longitude.ToString(),
                                Vendore = p.Description,
                                Adress = formatted_address,
                                Prices = pricesSquirrel.ToArray(),
                                FuelCategories = fuelcatsSquirrel.ToArray()
                            };
                            if (!markers.Contains(temp))
                                markers.Add(temp);
                        }

                    }
                }
            }
            else if (item.FuelCategories != null && item.Gascheaper==null)
            {

                if (item.FuelCategories.Length > 0)
                {
                    for (int i = 0; i < item.FuelCategories.Length; i++)
                    {
                        int fcid;
                        Int32.TryParse(item.FuelCategories[i], out fcid);
                        fuelcategoryid.Add(fcid);
                    }
                    // выбор из базы данных пои точек по категории и отбор по радиусу

                    var placemarks = from p in placemarksRepository.GetPlacemarks()
                                     select p;
                    placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();


                    foreach (var poisqurrel in incominglstSquirrel)
                    {
                        double latitude;
                        //l1 = poisqurrel.Lat.Replace('.', ',');
                        //l2 = poisqurrel.Long.Replace('.', ',');
                        l1 = poisqurrel.Lat;
                        l2 = poisqurrel.Long;
                        Double.TryParse(l1, out latitude);
                        double longitude;
                        Double.TryParse(l2, out longitude);
                        foreach (var p in placemarks)
                        {

                            if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                            {

                                List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();
                                if (prices.Count == 0)
                                    continue;

                                for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                                {
                                    int tempId = fuelcategoryid[i];
                                    if (prices[tempId - 1] != 0)
                                    {
                                        List<double?> pricesSquirrel = new List<double?>();
                                        List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();
                                        foreach (var v in prices)
                                        {
                                            pricesSquirrel.Add(v);
                                        }
                                        formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                                        Squirrel temp = new Squirrel()
                                        {
                                            Lat = p.Latitude.ToString(),
                                            Long = p.Longitude.ToString(),
                                            Vendore = p.Description,
                                            Adress = formatted_address,
                                            Prices = pricesSquirrel.ToArray(),
                                            FuelCategories = fuelcatsSquirrel.ToArray()
                                        };
                                        if (!markers.Contains(temp))
                                            markers.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
            
            }
            else if (item.FuelCategories != null && item.Gascheaper != null)
            {
                if (item.FuelCategories.Length > 0)
                {
                    for (int i = 0; i < item.FuelCategories.Length; i++)
                    {
                        int fcid;
                        Int32.TryParse(item.FuelCategories[i], out fcid);
                        fuelcategoryid.Add(fcid);
                    }
                    // выбор из базы данных пои точек по категории и отбор по радиусу

                    var placemarks = from p in placemarksRepository.GetPlacemarks()
                                     select p;
                    placemarks = placemarks.Where(x => x.CategoryId == categoryId).ToList();


                    foreach (var poisqurrel in incominglstSquirrel)
                    {
                        double latitude;
                        //l1 = poisqurrel.Lat.Replace('.', ',');
                        //l2 = poisqurrel.Long.Replace('.', ',');
                        l1 = poisqurrel.Lat;
                        l2 = poisqurrel.Long;
                        Double.TryParse(l1, out latitude);
                        double longitude;
                        Double.TryParse(l2, out longitude);
                        foreach (var p in placemarks)
                        {

                            if (DistanceBetweenPlaces(p.Latitude, p.Longitude, latitude, longitude) <= radius)
                            {

                                List<double?> prices = db.Prices.Where(x => x.PlacemarkId == p.Id).Select(x => x.Price).ToList();
                                if (prices.Count == 0)
                                    continue;

                                for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                                {
                                    int tempId = fuelcategoryid[i];
                                    if (prices[tempId - 1] != 0)
                                    {
                                        List<double?> pricesSquirrel = new List<double?>();
                                        List<string> fuelcatsSquirrel = db.FuelCategories.Where(x => x.CountryId == p.CountryId).Select(x => x.Category).ToList();
                                        foreach (var v in prices)
                                        {
                                            pricesSquirrel.Add(v);
                                        }
                                        formatted_address = GetforrmatedAdress(p.Latitude, p.Longitude);
                                        Squirrel temp = new Squirrel()
                                        {
                                            Lat = p.Latitude.ToString(),
                                            Long = p.Longitude.ToString(),
                                            Vendore = p.Description,
                                            Adress = formatted_address,
                                            Prices = pricesSquirrel.ToArray(),
                                            FuelCategories = fuelcatsSquirrel.ToArray()
                                        };
                                        if (!markers.Contains(temp))
                                            markers.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
                List<VendorItem> vendors = new List<VendorItem>();
                foreach (var m in markers)
                {
                    VendorItem vtemp = new VendorItem()
                    {
                        Name = m.Vendore,
                        Categories = m.FuelCategories,
                        Prices = m.Prices
                    };
                    if (!vendors.Contains(vtemp) & vtemp.Prices != null)
                    {
                        vendors.Add(vtemp);
                    }
                }
                List<CheaperFuelItem> cheaperfuels = new List<CheaperFuelItem>();
                List<Squirrel> tempmarkers = new List<Squirrel>();
                List<Squirrel> tempmarkers2 = new List<Squirrel>();
                for (int i = 0; i < fuelcategoryid.Count; i++)//перебираeм id-шники fuelсаtegory    
                {
                    foreach (var v in vendors)
                    {
                        int tempId = fuelcategoryid[i];//выбраная категория топлива
                        CheaperFuelItem fuelitem = new CheaperFuelItem()
                        {
                            Name = v.Name,
                            Price = Convert.ToDouble(v.Prices[tempId])
                        };
                        cheaperfuels.Add(fuelitem);
                    }
                    cheaperfuels.Sort();
                    tempmarkers = markers.Where(x => x.Vendore == cheaperfuels[0].Name).Select(x => x).ToList();
                    tempmarkers2.AddRange(tempmarkers);
                }
                markers = tempmarkers2;
            }
            return Json(markers, JsonRequestBehavior.AllowGet);
        }
        //vendors пои на карте
        [HttpPost]
        public JsonResult PostlstSVendor(List<Squirrel> incominglstSquirrel)
        {
            List<VendorItem> vendors = new List<VendorItem>();
            foreach (var v in incominglstSquirrel) 
            {
                VendorItem vtemp=new VendorItem(){
                    Name=v.Vendore,
                    Categories=v.FuelCategories,
                    Prices=v.Prices
                };
                if (!vendors.Contains(vtemp) & vtemp.Prices!=null)
                {
                    vendors.Add(vtemp);
                }
            }
            List<Squirrel> outlstSquirrel = new List<Squirrel>();
            foreach (var v in vendors)
            {
                Squirrel temp = new Squirrel() {
                    Vendore = v.Name,
                    FuelCategories=v.Categories,
                    Prices=v.Prices
                };
                outlstSquirrel.Add(temp);
            }
            return Json(outlstSquirrel, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}