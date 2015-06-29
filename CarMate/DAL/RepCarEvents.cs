using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.Classes;

namespace CarMate.DAL
{
    public class RepCarEvents : BaseRep<RepCarEvents>
    {
        public RepCarEvents(CarMateEntities db)
            : base(db)
        {
        }

        public IQueryable<CarEvents> Select(int carId, bool includeDeleted = false)
        {
            // Получаем все события автомобиля
            var res = Db.CarEvents.Where(x => x.CarId == carId) as IQueryable<CarEvents>;
            // Если флаг false (получить только не удаленные события)
            if (!includeDeleted)
            {
                res = res.Where(x => x.State != Consts.StateDelete);
            }
            return res;
        }

        public CarEvents FindById(int eventId, bool includeDeleted = false)
        {
            var res = Db.CarEvents.Find(eventId);
            if (res != null)
            {
                // Если флаг false (искать только не удаленных)
                if (!includeDeleted)
                {
                    if (res.State == Consts.StateDelete)
                        res = null;
                }
            }
            return res;
        }

        public void Add(CarEvents carEvent)
        {
            Db.CarEvents.Add(carEvent);
            Db.SaveChanges();
        }

        public void Remove(CarEvents carEvent)
        {
            carEvent.State = Consts.StateDelete;
            carEvent.DateCreate = DateTime.Now;
            Db.SaveChanges();
        }

        public CarEvents GetNewWithDefaultInitialization(int carId)
        {
            CarEvents carEvents = new CarEvents
            {
                CarId = carId,
                DateEvent = DateTime.Now,
                State = Consts.StateNew
            };
            return carEvents;
        }
    }
}