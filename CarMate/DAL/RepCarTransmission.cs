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
            var res = Db.CarTransmissionLang.Where(x => x.LanguageId == languageId).ToList();
            for (int i = 0; i < res.Count(); i++)
            {
                carTransmissionList.Add(new CarTransmission()
                {
                    NameTransmission = res[i].NameTransmission,
                    Id = res[i].CarTransmissionId
                });
            }

            return carTransmissionList.AsQueryable();
        }
    }
}



