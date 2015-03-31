using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarMate.DAL
{
    public class PlacemarksRepository:IPlacemarksRepository,IDisposable
    {
        private CarMateEntities context;
        public PlacemarksRepository(CarMateEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Placemarks> GetPlacemarks()
        {
            return context.Placemarks.ToList();
        }
        public Placemarks GetPlacemarkByID(int id)
        {
            return context.Placemarks.Find(id);
        }
        public void InsertPlacemarkByID(Placemarks placemark)
        {
            context.Placemarks.Add(placemark);
        }
        public void UpdatePlaceMark(Placemarks placemark)
        {
            context.Entry(placemark).State = EntityState.Modified;
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
