using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMate.DAL
{
    public interface ICategoryRepository
    {
        IEnumerable<Categories> GetCategories();
        Categories GetCategoryByID(int categoryId);
        void InsertCategoryByID(Categories category);
        void UpdateCategory(Categories category);
        void Save();
    }
}
