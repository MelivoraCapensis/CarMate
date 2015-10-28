using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace MapXamarinForm.ViewModel
{
    public class UserFuelEventDetail:INotifyPropertyChanged
    {
        string fileCategoryEvent;
        int categoryIdEvent;
        int fuelCategoryId;
        string eventCost;
        string detailEvent;
        string lastMounthCost;
        string currentMounthCost;
        string lastEventDate;
        string lastEventOdometer;
        string fuelcount;
        string carModel;
        string commentEvent;
        Poi currentPoi;
        string pricePerLitr;
        string eventAmount;
        Dictionary<string, int> categoryToEvent = new Dictionary<string, int>();
        Dictionary<string, int> fuelcategoryToEvent = new Dictionary<string, int>();
        Command saveEventCommand;
        public const string SaveEventCommandPropertyName = "SaveEventCommand";
        public event PropertyChangedEventHandler PropertyChanged;
        //icon
        public string Filecategoryevent
        {
            get { return fileCategoryEvent; }
            set
            {
                if (fileCategoryEvent != value)
                {
                    fileCategoryEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Filecategoryevent"));
                    }
                }
            }
        }
        public int FuelCategoryId
        {
            get { return fuelCategoryId; }
            set
            {
                if (fuelCategoryId != value)
                {
                    fuelCategoryId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FuelCategoryId"));
                    }
                }
            }
        }
        public int CategoryIdEvent
        {
            get { return categoryIdEvent; }
            set
            {
                if (categoryIdEvent != value)
                {
                    categoryIdEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CategoryIdEvent"));
                    }
                }
            }
        }
        public string EventAmount
        {
            get { return eventAmount; }
            set
            {
                if (eventAmount != value)
                {
                    eventAmount = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EventAmount"));
                    }
                }
            }
        }
        //last fuel event detailI(vendor,priceperlitr,fuelcategory)
        public string DetailEvent
        {
            get { return detailEvent; }
            set
            {
                if (detailEvent != value)
                {
                    detailEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DetailEvent"));
                    }
                }
            }
        }
        public Poi CurrentPoi
        {
            get { return currentPoi; }
            set {
                if (currentPoi != value)
                {
                    currentPoi = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrentPoi"));
                    }
                }
            }
        }
        public string PricePerLitr
        {
            get { return pricePerLitr; }
            set
            {
                if (pricePerLitr != value)
                {
                    pricePerLitr = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PricePerLitr"));
                    }
                }
            }
        }
        //last event cost
        public string EventCost
        {
            get { return eventCost; }
            set
            {
                if (eventCost != value)
                {
                    eventCost = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EventCost"));
                    }
                }
            }
        }
        //last event date
        public string LastEventDate
        {
            get { return lastEventDate; }
            set
            {
                if (lastEventDate != value)
                {
                    lastEventDate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastEventDate"));
                    }
                }
            }
        }
        //last event date
        public string LastEventOdometer
        {
            get { return lastEventOdometer; }
            set
            {
                if (lastEventOdometer != value)
                {
                    lastEventOdometer = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastEventOdometer"));
                    }
                }
            }
        }
        //total cost by event fuel last mounth
        public string LastMounthCost
        {
            get { return lastMounthCost; }
            set
            {
                if (lastMounthCost != value)
                {
                    lastMounthCost = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastMounthCost"));
                    }
                }
            }
        }
        //total cost by event fuel current mounth
        public string CurrentMounthCost
        {
            get { return currentMounthCost; }
            set
            {
                if (currentMounthCost != value)
                {
                    currentMounthCost = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrentMounthCost"));
                    }
                }
            }
        }
        public string FuelCount
        {
            get { return fuelcount; }
            set
            {
                if (fuelcount != value)
                {
                    fuelcount = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FuelCount"));
                    }
                }
            }
        }
        public string CarModel
        {
            get { return carModel; }
            set
            {
                if (carModel != value)
                {
                    carModel = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CarModel"));
                    }
                }
            }
        }
        

        public string  CommentEvent
        {
            get { return commentEvent; }
            set
            {
                if (commentEvent != value)
                {
                    commentEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CommentEvent"));
                    }
                }
            }
        }
        public Dictionary<string, int> CategoryToEvent
        {
            get { return categoryToEvent; }
            set
            {
                if (categoryToEvent != value)
                {
                    categoryToEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CategoryToEvent"));
                    }
                }
            }
        }
        public Dictionary<string, int> FuelcategoryToEvent
        {
            get { return fuelcategoryToEvent; }
            set
            {
                if (fuelcategoryToEvent != value)
                {
                    fuelcategoryToEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FuelcategoryToEvent"));
                    }
                }
            }
        }
        public Command SaveEventCommand
        {
            get
            {
                return saveEventCommand ?? (saveEventCommand = new Command(async () => await ExecuteSaveEventCommand()));
            }
        }
        public async Task<bool> GetLastEvent(int carId)
        {
            CarEvent lastEvent;
            List<CarEvent> lcarEvent;
            try 
            {
                var task1 = App.Database.GetLastEventCar(carId).ConfigureAwait(true);
                var res1 = await task1;
                if (res1 != null)
                {
                    lastEvent = res1;
                    this.EventCost = lastEvent.CostTotal.ToString()+"\r\n"+lastEvent.PricePerLitr.ToString();
                    this.LastEventDate = lastEvent.DateCreate.ToString();
                    this.LastEventOdometer = lastEvent.Odometer.ToString();

                    var task3 = GetListEventbyFuelbyEventType(lastEvent).ConfigureAwait(true);
                    var res3 = await task3;
                    lcarEvent = res3;
                    if (lcarEvent != null)
                    {
                        List<CarEvent> lastmounth = lcarEvent.Where(x => x.DateEvent.Month == lastEvent.DateEvent.Month - 1).Select(x => x).ToList();
                        decimal lastmCost = lastmounth.Sum(x => x.CostTotal);
                        this.LastMounthCost = lastmCost.ToString();

                        List<CarEvent> currentmounth = lcarEvent.Where(x => x.DateEvent.Month == lastEvent.DateEvent.Month).Select(x => x).ToList();
                        decimal currentmCost = currentmounth.Sum(x => x.CostTotal);
                        this.CurrentMounthCost = currentmCost.ToString();
                        var task2 = App.Database.GetPlacemarkByCurrentEventWithChildren(lastEvent).ConfigureAwait(true);
                        var res2 = await task2;
                        string vendor = "";
                        if (res2 != null)
                        {
                            vendor = res2.Vendor.VendorName;
                        }

                        string fuelcategory = lastEvent.FuelCategory.FuelCategoryName;
                        string fuelcount = lastEvent.FuelCount.ToString();
                        this.FuelCount = fuelcount;
                        this.DetailEvent = vendor + "\r\n" + fuelcategory + "\r\n" + fuelcount + "\r\n";
                    }
                   
                }
                
                return true;
            }
            catch (Exception) { return false; }
        }
        //список ивентов по категории топлива
        public async Task<List<CarEvent>> GetListEventbyFuelbyEventType(CarEvent lastCarEvent)
        {
            List<CarEvent> lCarEventbyEventTypeFuelCategory;
            try
            {
                var carId = lastCarEvent.CarId;
                var task1 = App.Database.GetCarEvent(carId);
                var res1 = await task1;
                if (res1 != null)
                {
                    int lastCarEventId = lastCarEvent.Id;
                    int servEventTypeId = lastCarEvent.ServEventTypeId;
                    int servFuelCategoryId = lastCarEvent.ServFuelCategoryId;
                    this.FuelCategoryId = servFuelCategoryId;
                    //все ивенты по категории и категории топлива
                    lCarEventbyEventTypeFuelCategory = res1.Where(x => x.ServEventTypeId == servEventTypeId && x.ServFuelCategoryId == servFuelCategoryId).ToList();
                    return lCarEventbyEventTypeFuelCategory;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        
        public async Task InitCarEventDetail()
        {
            await GetLastEvent(App.UserViewModel.Current_car.Car_id);
        }
        public async Task InitCategoryToEvent()
        {
            List<EventType> event_types = await App.Database.GetEventTypes();
            if (event_types != null)
            {
                foreach (var e in event_types)
                {

                    this.CategoryToEvent.Add(e.Name, e.EventTypeId);
                }
            }
        }
        public async Task InitFuelCategoryToEvent()
        {
            List<FuelCategory> fuel_categories = await App.Database.GetFuelCategoryByCountry(1);
            if (fuel_categories != null)
            {
                foreach (var fc in fuel_categories)
                {
                    if(CurrentPoi.fuelcetegory.Contains(fc.FuelCategoryName))
                        this.FuelcategoryToEvent.Add(fc.FuelCategoryName, fc.FuelCategoryId);
                }
            }
        }
        public async Task ExecuteSaveEventCommand()
        {
            int serv_carEventTypeId = this.CategoryIdEvent;
            int serv_fuelCategoryId = this.FuelCategoryId;
            int carId = App.UserViewModel.Current_car.Car_id;
            int odometr;
            decimal cost_total,price_per_litr,fuel_count,lat,lng;
            Int32.TryParse(this.LastEventOdometer,out odometr);
            Decimal.TryParse(this.EventCost,out cost_total);
            Decimal.TryParse(this.EventAmount, out fuel_count);
            Decimal.TryParse(this.PricePerLitr, out price_per_litr);
            Decimal.TryParse(this.CurrentPoi.geometry.location.lat.ToString(), out lat);
            Decimal.TryParse(this.CurrentPoi.geometry.location.lng.ToString(), out lng);
            CarEvent new_carEvent = new CarEvent 
            {
                Odometer=odometr,
                CostTotal = cost_total,
                DateEvent=DateTime.Now,
                Comment=this.CommentEvent,
                FuelCount = fuel_count,
                PricePerLitr = price_per_litr,
                Latitute=lat,
                Longitute=lng,
                IsFullTank=0,
                IsMissedFilling=0,               
                DateCreate = DateTime.Now,
                State=1,
                CarId = carId,
                ServEventTypeId = serv_carEventTypeId,
                ServFuelCategoryId = serv_fuelCategoryId
            };
            await App.Database.AddNewCarEvent(new_carEvent);
            
        }
    }
}
