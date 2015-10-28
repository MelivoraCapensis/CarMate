using MapXamarinForm.Models;
using MapXamarinForm.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServiceDb;

namespace MapXamarinForm.ViewModel
{
    public class UserSessionViewModel : INotifyPropertyChanged
    {
        string firstname, lastname, nickname, password, useremail;
        int local_user_id;
        bool user_exist;
        DateTime datecreate,dateRegister;
        public ObservableCollection<CarViewModel> Cars = new ObservableCollection<CarViewModel>();
        CarViewModel current_car;
        public event PropertyChangedEventHandler PropertyChanged;
        public string FirstName
        {
            get { return firstname; }
            set
            {
                if (firstname != value)
                {
                    firstname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                }
            }
        }
        public string LastName
        {
            get { return lastname; }
            set
            {
                if (lastname != value)
                {
                    lastname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                }
            }
        }
        public string UserEmail
        {
            get { return useremail; }
            set
            {
                if (useremail != value)
                {
                    useremail = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UserEmail"));
                    }
                }
            }
        }
        public DateTime DateRegister
        {
            get { return dateRegister; }
            set
            {
                if (dateRegister != value)
                {
                    dateRegister = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateRegister"));
                    }
                }
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (nickname != value)
                {
                    nickname = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Nickname"));
                    }
                }
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                    }
                }
            }
        }
        public CarViewModel Current_car
        {
            get { return current_car; }
            set
            {
                if (current_car != value)
                {
                    current_car = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Current_car"));
                    }
                }
            }
        }
        
        public int Local_user_id
        {
            get { return local_user_id; }
            set
            {
                if (local_user_id != value)
                {
                    local_user_id = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Local_user_id"));
                    }
                }
            }
        }
        public DateTime Datecreate
        {
            get { return datecreate; }
            set
            {
                if (datecreate != value)
                {
                    datecreate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Datecreate"));
                    }
                }
            }
        }
        public bool User_exist
        { 
            get { return user_exist; }
            set
            {
                if (user_exist != value)
                {
                    user_exist = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("User_exist"));
                    }
                }
            }
        }
        public async Task IsExistsUserAsync()
        {
            var task1 =await App.Database.IsExistsUserAsync();
            User_exist = task1;
            if (User_exist == false)
                this.DateRegister = DateTime.Now;
        }
        public async Task<bool> LoadUserDataAsync()
        {
            try {
                var task1 = await App.Database.GetUserWithChildren();
                this.LastName = task1.LastName;
                this.FirstName = task1.FirstName;
                this.UserEmail = task1.UserEmail;
                this.Nickname = task1.Nickname;
                this.Password = task1.UserPassword;
                this.DateRegister = task1.DateRegister;
                this.Local_user_id = task1.Id;
                this.User_exist = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
        public async Task<bool> LoadUserCarsAsync()
        {
            try {
                var task1 = await App.Database.GetCarByLocalUserId(Local_user_id);
                if (task1 != null)
                {
                    Cars.Clear();
                    foreach (var c in task1)
                    {
                        var task2 = await App.Database.GetCarModel(c.ServModelId);
                        var task3 = await App.Database.GetCarBrand(task2.ServBrandId);
                        var task4 = await App.Database.GetCarModification(c.ServModificationId);
                        var task5 = await App.Database.GetFuelCategory(c.ServFuelCategoryId);
                        CarViewModel car = new CarViewModel
                        {
                            Car_id=c.Id,
                            Serv_fuelcategory_id=c.ServFuelCategoryId,
                            Model=task2.model,
                            Brand=task3.Brand,
                            Modification=task4.Modification,
                            Fuelcategory=task5.FuelCategoryName,
                            Odometr=c.Odometer.ToString(),
                            Tank=c.Tank.ToString(),
                            Datebuy=c.DateBuy.Date.ToString("dd/MM/yyyy HH:mm"),
                            IsCheked=false
                        };
                        Cars.Add(car);
                    }
                    if (Current_car == null)
                    {
                        Cars.First().IsCheked = true;
                        Current_car = Cars.First();
                    }                   
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }                 
        }
        public async Task<bool> IsServerRegisterAsync()
        {
            string name = this.Nickname;
            string password = this.Password;
            var task2 = DataBaseServiceAgent.DoIsRegister(name, password);
            var res2 = await task2;
            return res2;
        }
        public async Task<UserProxy> GetExistsUserbyServerAsync()
        {
            string name = this.Nickname;
            string password = this.Password;
            var task3 = DataBaseServiceAgent.DoGetUser(name, password);
            var res3 = await task3;
            return res3;
        }
        public async Task<string> SynchronizeByUser(UserProxy userProxy)
        {
            var task1 = SynchronizationHelper.SynchronizeByUser(userProxy);
            var res1 = await task1;
            return res1;
        }
        public async Task SaveUserAccountDetail()
        {
            User register_user = new User
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Nickname=this.Nickname,
                UserEmail = this.UserEmail,
                UserPassword = this.Password,
                DateCreate = DateTime.Now,
                DateRegister = this.DateRegister,
                State = 1
            };
            if(User_exist)
            {
                await App.Database.UpdateUser(register_user);
            }
            else
            {
                
                await App.Database.InsertUser(register_user);
            }  
        }
    }   
}
