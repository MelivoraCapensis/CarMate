using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.Classes;

namespace CarMate.DAL
{
    public class RepCars : BaseRep<RepCars>
    {
        public RepCars(CarMateEntities db)
            : base(db)
        {
        }

        public IQueryable<Cars> SelectAll(bool showDeleted = false)
        {
            // Получаем все маши всех пользователей
            var res = Db.Cars as IQueryable<Cars>;
            // Если флаг false (получить только не удаленные машины)
            if (!showDeleted)
            {
                res = res.Where(x => x.State != Consts.StateDelete);
            }
            return res;
        }

        public IQueryable<Cars> Select(int userId, bool includeDeleted = false)
        {
            // Получаем все машиys пользователя
            var res = Db.Cars.Where(x => x.UserId == userId) as IQueryable<Cars>;
            // Если флаг false (получить только не удаленные машины)
            if (!includeDeleted)
            {
                res = res.Where(x => x.State != Consts.StateDelete);
            }
            return res;
        }

        public Cars FindById(int carId, bool includeDeleted = false)
        {
            // Ищем автомобиль по id
            var res = Db.Cars.Find(carId);
            if (res != null)
            {
                // Если флаг false (искать только не удаленных)
                if (!includeDeleted)
                {
                    // Если автомоибль помечен, как удаленный
                    if (res.State == Consts.StateDelete)
                        res = null;
                }
            }
            return res;
        }

        public Cars GetNewWithDefaultInitialization(int userId)
        {
            Cars car = new Cars
            {
                UserId = userId,
                DateBuy = DateTime.Now,
                Rating = 0,
                State = Consts.StateNew
            };
            return car;
        }

        public void Add(Cars car)
        {
            Db.Cars.Add(car);
            Db.SaveChanges();
        }

        //public void RemoveById(int carId)
        //{
        //    var car = FindById(carId);

        //    var carEvents = Db.CarEvents.Where(x => x.CarId == carId);
        //    foreach (var carEvent in carEvents)
        //    {
        //        Db.CarEvents.Remove(carEvent);
        //    }

        //    Db.Cars.Remove(car);
        //    Db.SaveChanges();
        //}

        public void Remove(Cars car)
        {
            var carEvents = Db.CarEvents.Where(x => x.CarId == car.Id);
            foreach (var carEvent in carEvents)
            {
                //Db.CarEvents.Remove(carEvent);
                carEvent.State = Consts.StateDelete;
                carEvent.DateCreate = DateTime.Now;
            }

            //Db.Cars.Remove(car);
            car.State = Consts.StateDelete;
            car.DateCreate = DateTime.Now;
            Db.SaveChanges();
        }
    }
}