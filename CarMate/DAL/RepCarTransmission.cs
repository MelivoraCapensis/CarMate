using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;

namespace CarMate.DAL
{
    public class RepCarTransmission : BaseRep<CarTransmission>
    {
        public RepCarTransmission(CarMateEntities db)
            : base(db)
        {
            //_db = db;
        }

        public IQueryable<CarTransmission> Select(int languageId)
        {
            List<CarTransmission> carTransmissionList = new List<CarTransmission>();
            var carTransmissionLang = Db.CarTransmissionLang.Where(x => x.LanguageId == languageId).ToList();
            for (int i = 0; i < carTransmissionLang.Count(); i++)
            {
                carTransmissionList.Add(new CarTransmission()
                {
                    NameTransmission = carTransmissionLang[i].NameTransmission,
                    Id = carTransmissionLang[i].CarTransmissionId
                });
            }

            return carTransmissionList.AsQueryable();
        }
    }
}



