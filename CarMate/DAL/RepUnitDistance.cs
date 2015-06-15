using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.Classes;

namespace CarMate.DAL
{
    public class RepUnitDistance : BaseRep<UnitDistance>
    {
        public RepUnitDistance(CarMateEntities db)
            : base(db)
        {
            //_db = db;
        }

        public IQueryable<UnitDistance> Select(int languageId)
        {
            var unitDistanceList = Db.UnitDistance.ToList();
            var res = Db.UnitDistanceLang.Where(x => x.LanguageId == languageId).ToList();
            for (int i = 0; i < res.Count(); i++)
            {
                unitDistanceList.First(x => x.Id == res[i].UnitDistanceId).NameUnit = res[i].NameUnit;
                //{
                //    NameUnit = res[i].NameUnit,
                //    Id = res[i].UnitDistanceId
                //});
            }

            return unitDistanceList.AsQueryable();
        }

        public string SelectAnalogue(string unitName)
        {
            var unitDistanceLang = Db.UnitDistanceLang.First(x => x.NameUnit.Equals(unitName));
            int unitDistanceId = unitDistanceLang.UnitDistanceId;
            var res = Db.UnitDistanceLang.First(x => x.UnitDistanceId == unitDistanceId && x.LanguageId == Consts.RussianLanguage).NameUnit;

            return res;
        }
    }

}