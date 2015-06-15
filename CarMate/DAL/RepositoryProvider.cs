using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.DAL
{
    public class RepositoryProvider
    {
        private readonly CarMateEntities _db;
        public RepositoryProvider(CarMateEntities db)
        {
            _db = db;
        }

        private readonly Hashtable _h = new Hashtable();

        public T GetRep<T>() where T : class
        {
            var typename = typeof(T).FullName;
            if (!_h.ContainsKey(typename))
            {
                var rep = (T)Activator.CreateInstance(typeof(T), _db);
                _h[typename] = rep;

            }
            return (T)_h[typename];
        }


        public RepCarTransmission CarTransmission
        {
            get { return GetRep<RepCarTransmission>(); }
        }

        public RepUnitDistance UnitDistance
        {
            get { return GetRep<RepUnitDistance>(); }
        }

        public RepEventTypes EventTypes
        {
            get { return GetRep<RepEventTypes>(); }
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
            //throw new NotImplementedException();
        }


    }
}