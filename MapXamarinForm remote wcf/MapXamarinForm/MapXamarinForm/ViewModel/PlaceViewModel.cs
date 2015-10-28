using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace MapXamarinForm.ViewModel
{
    public class PlaceViewModel:BaseViewModel,INotifyPropertyChanged
    {  
        public ObservableCollection<Place> Places = new ObservableCollection<Place>();
        public ObservableCollection<Poi> Pois = new ObservableCollection<Poi>();
        public ObservableCollection<FuelMarker> FuelMarkers = new ObservableCollection<FuelMarker>();
        public int FuelId { set; get; }
        public int CarId { set; get; }
        double radius;
        public bool isCafeDisplay;
        public bool isGasDisplay;
        public bool isRepairDisplay;
        public bool isParkingDisplay;
        Dictionary<string, double> radiusToEvent = new Dictionary<string, double>();
        public event PropertyChangedEventHandler PropertyChanged;
        public PlaceViewModel()
        {          
            InitRadiusToEvent();
        }
        public bool IsCafeDisplay
        {
            get { return isCafeDisplay; }
            set {
                if (isCafeDisplay != value)
                {
                    isCafeDisplay = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsCafeDisplay"));
                    }
                }
            }
        }
        public double Radius 
        {
            get { return radius; }
            set {
                if (radius != value)
                {
                    radius = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Radius"));
                    }
                }
            }
        }
        public bool IsGasDisplay
        {
            get { return isGasDisplay; }
            set
            {
                if (isGasDisplay != value)
                {
                    isGasDisplay = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsGasDisplay"));
                    }
                }
            }
        }
        public bool IsRepairDisplay
        {
            get { return isRepairDisplay; }
            set
            {
                if (isRepairDisplay != value)
                {
                    isRepairDisplay = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsRepairDisplay"));
                    }
                }
            }
        }
        public bool IsParkingDisplay
        {
            get { return isParkingDisplay; }
            set
            {
                if (isParkingDisplay != value)
                {
                    isParkingDisplay = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsParkingDisplay"));
                    }
                }
            }
        }
        public Dictionary<string, double> RadiusToEvent
        {
            get { return radiusToEvent; }
            set
            {
                if (radiusToEvent != value)
                {
                    radiusToEvent = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("RadiusToEvent"));
                    }
                }
            }
        }
        //Command property for loading places operation bound
        private Command loadPlacesCommand;
        public Command LoadPlacesCommand
        {
            get
            {
                return loadPlacesCommand ?? (loadPlacesCommand=new Command(async()=> await ExecuteLoadPlacesCommand(),()=>!IsBusy));
            }
        }
        private Command loadPoiCommand;
        public Command LoadPoiCommand
        {
            get
            {
                return loadPoiCommand ?? (loadPoiCommand = new Command(async () => await ExecuteLoadPoiCommand(), () => !IsBusy));
            }
        }
        private Command loadUserDataCommand;
        public Command LoadUserDataCommand
        {
            get
            {
                return loadUserDataCommand ?? (loadUserDataCommand = new Command(async () => await ExecuteLoadUserDataCommand(), () => !IsBusy));
            }
        }
       

        public async Task ExecuteLoadPlacesCommand()
        {
            //check if an operation is already in progress and if true then return
            if (IsBusy)
                return;
            //set IsBusy flag true to indicate that a load  operation is starting
            IsBusy = true;
            //notify abbor command state change (as it is bound to busy flag)
            LoadPlacesCommand.ChangeCanExecute();
            try
            {
                bool DidGetLocation = await App.Locator.GetLocation();
                if (DidGetLocation)
                {
                   
                    //execute load operation
                   
                    NearbyQuery query = await App.Service.GetPlacesForCoordinates(App.Locator.Latitude, App.Locator.Longitude);
                    //populate list with results
                    if (query.Places != null)
                    {
                        foreach (Place p in query.Places)
                        {
                            Places.Add(p);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading places:", ex.Message);
            }
            finally {
                IsBusy = false;
                LoadPlacesCommand.ChangeCanExecute();
            }
        }
        public async Task ExecuteLoadPoiCommand()
        {
            if (IsBusy)
                return;
            if(Pois.Count>0)
                Pois.Clear();
            IsBusy = true;
            LoadPoiCommand.ChangeCanExecute();
            try
            {
                bool DidGetLocation = await App.Locator.GetLocation();
                if (DidGetLocation)
                {
                    List<int> idfuel_categoryId = new List<int>();
                    foreach (var fm in FuelMarkers)
                    {
                        if (fm.IsChecked == true)
                            idfuel_categoryId.Add(fm.FuelId);
                    }
                    NearbyQueryPoi poiquery = new NearbyQueryPoi();
                    if(Radius==0.0)
                        Radius = 1;
                    poiquery.Pois = await App.PService.GetPoiForCoordinates(App.Locator.Latitude, App.Locator.Longitude, Radius, FuelId, idfuel_categoryId);
                    if (poiquery.Pois != null)
                    {
                        foreach (Poi p in poiquery.Pois)
                        {
                            Pois.Add(p);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading poi db:", ex.Message);
            }
            finally
            {
                IsBusy = false;
                LoadPoiCommand.ChangeCanExecute();
            }
        }
        public async Task ExecuteLoadUserDataCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            LoadUserDataCommand.ChangeCanExecute();
            try
            {
                CarUserDataAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading poi db:", ex.Message);
            }
            finally
            {
                IsBusy = false;
                LoadUserDataCommand.ChangeCanExecute();
            }
        }
        public void CarUserDataAsync()
        {
            //получаем машину
          //  await Task.Delay(600);
            try {
              
                this.FuelId =App.UserViewModel.Current_car.Serv_fuelcategory_id ;
                this.CarId = App.UserViewModel.Current_car.Car_id;
            }
            catch (Exception)
            { }
            
        }
        public void InitRadiusToEvent()
        {
           this.RadiusToEvent.Add("0.5", 0.5);
           this.RadiusToEvent.Add("1", 1);
           this.RadiusToEvent.Add("1.5", 1.5);
           this.RadiusToEvent.Add("2", 2.0);
           this.RadiusToEvent.Add("2.5", 2.5);
        }
        public async Task InitFuelMarker()
        {

            List<FuelCategory> fuelcategories = await App.Database.GetFuelCategoryByCountry(1);
            if (FuelMarkers.Count == 0) {
                foreach (var fc in fuelcategories)
                {
                    FuelMarker temp = new FuelMarker
                    {
                        FuelId = fc.FuelCategoryId,
                        Fuelname = fc.FuelCategoryName,
                        IsChecked = false
                    };
                    if (fc.FuelCategoryId == FuelId)
                        temp.IsChecked = true;
                    FuelMarkers.Add(temp);
                }
            }
            
        }
    }
}
