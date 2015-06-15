using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.DAL
{
    public class RepEventTypes : BaseRep<EventTypes>
    {
        public RepEventTypes(CarMateEntities db)
            : base(db)
        {
            //_db = db;
        }

        public IQueryable<EventTypes> Select(int languageId)
        {
            var res = Db.EventTypes.Where(x => x.LanguageId == languageId);
            return res;
        }
    }
}