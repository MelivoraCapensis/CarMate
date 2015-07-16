using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.DAL
{
    public class RepCarNodes : BaseRep<CarNodes>
    {
        public RepCarNodes(CarMateEntities db)
            : base(db)
        {
        }

        public IQueryable<CarNodes> Select(int languageId)
        {
            List<CarNodes> carNodesList = new List<CarNodes>();
            var carNodesLang = Db.CarNodesLang.Where(x => x.LanguageId == languageId).ToList();
            for (int i = 0; i < carNodesLang.Count(); i++)
            {
                carNodesList.Add(new CarNodes()
                {
                    Name = carNodesLang[i].Name,
                    Id = carNodesLang[i].CarNodeId
                });
            }

            return carNodesList.AsQueryable();
        }
    }
}