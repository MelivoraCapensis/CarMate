using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarMate.DAL
{
    public class CategoryRepository:ICategoryRepository,IDisposable
    {
        private CarMateEntities context;
        public CategoryRepository(CarMateEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Categories> GetCategories()
        {
            return context.Categories.ToList();
        }
        public Categories GetCategoryByID(int id)
        {
            return context.Categories.Find(id);
        }
        public void InsertCategoryByID(Categories category)
        {
            context.Categories.Add(category);
        }
        public void UpdateCategory(Categories category)
        {
            context.Entry(category).State = EntityState.Modified;
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
