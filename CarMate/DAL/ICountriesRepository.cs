using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMate.DAL
{
    public interface ICountriesRepository
    {
        IEnumerable<Countries> GetCountries();
        Countries GetCountryByID(int countryId);
        void InsertCountry(Countries country);
        void UpdatePlaceMark(Countries country);
        void Save();
    }
}
