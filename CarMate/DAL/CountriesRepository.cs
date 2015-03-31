using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace CarMate.DAL
{
    public class CountriesRepository:ICountriesRepository,IDisposable
    {
        private CarMateEntities context;
        public CountriesRepository(CarMateEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Countries> GetCountries()
        {
            return context.Countries.ToList();
        }
        public Countries GetCountryByID(int id)
        {
            return context.Countries.Find(id);
        }
        public void InsertCountry(Countries country)
        {
            context.Countries.Add(country);
        }
        public void UpdatePlaceMark(Countries country)
        {
            context.Entry(country).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
