using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.Classes;
using WebGrease.Css.Extensions;

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
            var carEvents = Db.CarEvents.Where(x => x.CarId == carId) as IQueryable<CarEvents>;
            // Если флаг false (получить только не удаленные события)
            if (!includeDeleted)
            {
                carEvents = carEvents.Where(x => x.State != Consts.StateDelete);
            }
            var carEventsList = carEvents.ToList();
            foreach (var carEvent in carEventsList)
            {
                var firstOrDefault = carEvent.EventTypes.EventTypesLang.FirstOrDefault(y => y.EventTypesId == carEvent.EventTypes.Id && y.Languages.Code.Equals("ru", StringComparison.OrdinalIgnoreCase));
                if (firstOrDefault != null)
                {
                    carEvent.EventTypes.Name = firstOrDefault.Name;
                }

            }
            //carEventsList.ForEach(x => x.EventTypes.Name = x.EventTypes.EventTypesLang.FirstOrDefault(y => y.EventTypesId == x.EventTypes.Id && y.Languages.Code.Equals("ru", StringComparison.OrdinalIgnoreCase)).Name);

            //carEvents.ForEach(x => x. Cars = x.Cars.Where(y => y.State != Consts.StateDelete).ToList());
            return carEventsList.AsQueryable();
        }

        public CarEvents FindById(int eventId, bool includeDeleted = false)
        {
            var carEvent = Db.CarEvents.Find(eventId);
            if (carEvent != null)
            {

                var eventTypesLang = carEvent.EventTypes.EventTypesLang
                    .FirstOrDefault(y => y.EventTypesId == carEvent.EventTypes.Id &&
                                         y.Languages.Code.Equals("ru", StringComparison.OrdinalIgnoreCase));
                if (eventTypesLang != null)
                {
                    carEvent.EventTypes.Name = eventTypesLang.Name;
                }

                // Если флаг false (искать только не удаленных)
                if (!includeDeleted)
                {
                    if (carEvent.State == Consts.StateDelete)
                        carEvent = null;
                }
            }
            

            return carEvent;
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