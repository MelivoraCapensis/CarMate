using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace CarMate.DAL
{
    public class RegionRepository:IRegionRepository,IDisposable
    {
        private CarMateEntities context;
        public RegionRepository(CarMateEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Regions> GetRegions()
        {
            return context.Regions.ToList();
        }
        public Regions GetRegionByID(int id)
        {
            return context.Regions.Find(id);
        }
        public void InsertRegionByID(Regions region)
        {
            context.Regions.Add(region);
        }
        public void UpdateRegion(Regions region)
        {
            context.Entry(region).State = EntityState.Modified;
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
