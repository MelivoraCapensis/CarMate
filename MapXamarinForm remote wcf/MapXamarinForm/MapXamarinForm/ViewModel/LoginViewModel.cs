using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using MapXamarinForm.Service;
using MapXamarinForm.Models;
using WcfServiceDb;

namespace MapXamarinForm.ViewModel
{
    public class LoginViewModel:INotifyPropertyChanged
    {
        string login,password;
        bool dbexist;
        public event PropertyChangedEventHandler PropertyChanged;
        public LoginViewModel(string filename)
        {
            this.Filename = filename;
        }
        public string Filename { get; set; }
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
        public bool Dbexist
        {
            get { return dbexist; }
            set
            {
                if (dbexist != value)
                {
                    dbexist = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Dbexist"));
                    }
                }
            }
        }
        public string Login
        {
            get { return login; }
            set
            {
                if (login != value)
                {
                    login = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Login"));
                    }
                }
            }
        }
        public async Task ExistsDbAsync()
        {
            var task1=FileHelper.ExistsAsync(this.Filename);
            var res1 = await task1;
           
        }
        public async Task<bool> IsRegisterAsync()
        {
            string name = this.Login;
            string password = this.Password;
            var task2 = DataBaseServiceAgent.DoIsRegister(name, password);
            var res2 = await task2;
            return res2;
        }
        public async Task<UserProxy> GetExistsUserAsync()
        {
            string name = this.Login;
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
        public async Task IsUserExists()
        {
            var task1 = await App.Database.GetUserAsync();
            if(task1!=null)
                this.Dbexist = true;
            else
                this.Dbexist = false;
        }
    }
}
