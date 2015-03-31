using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMate.DAL
{
    public interface IPlacemarksRepository
    {
        IEnumerable<Placemarks> GetPlacemarks();
        Placemarks GetPlacemarkByID(int placemarkId);
        void InsertPlacemarkByID(Placemarks placemark);
        void UpdatePlaceMark(Placemarks placemark);
        void Save();
    }
}
