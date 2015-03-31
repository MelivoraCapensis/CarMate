using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMate.DAL
{
    public interface IRegionRepository
    {
        IEnumerable<Regions> GetRegions();
        Regions GetRegionByID(int regionId);
        void InsertRegionByID(Regions region);
        void UpdateRegion(Regions region);
        void Save();
    }
}
