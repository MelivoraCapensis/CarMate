using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarMate.Classes;
using WebGrease.Css.Extensions;

namespace CarMate.DAL
{
    public class RepUsers : BaseRep<RepUsers>
    {
        public RepUsers(CarMateEntities db)
            : base(db)
        {
        }

        public IQueryable<Users> SelectAll(bool showDeleted = false)
        {
            // Получаем всех пользователей
            var users = Db.Users as IQueryable<Users>;
            // Если флаг false (получить только не удаленных пользователей)
            if (!showDeleted)
            {
                users = users.Where(x => x.State != Consts.StateDelete);
                users.ForEach(x=>x.Cars = x.Cars.Where(y=>y.State != Consts.StateDelete).ToList());
                // Загружаем только не удаленные автомобили
                //RepositoryProvider repProvider = new RepositoryProvider(new CarMateEntities());
                //foreach (Users t in usersList)
                //{
                //    var user = t;
                //    //usersList[i].Cars = repProvider.Cars.Select(usersList[i].Id).ToList();
                //    t.Cars =
                //        Db.Cars.Where(x => x.UserId == user.Id && x.State != Consts.StateDelete).ToList();
                //}
                //users = usersList.AsQueryable();
            }
            
            return users;
        }

        public Users FindById(int userId, bool includeDeleted = false)
        {
            var res = Db.Users.Find(userId);
            if (res != null)
            {
                // Если флаг false (искать только не удаленных)
                if (!includeDeleted)
                {
                    //res.Cars = res.Cars.Where(y => y.State != Consts.StateDelete).ToList();
                    if (res.State == Consts.StateDelete)
                        res = null;
                }
            }
            return res;
        }

        public Users FindByName(string userName, bool includeDeleted = false)
        {
            if (userName == null)
                return null;
            var res = Db.Users
                .FirstOrDefault(x => x.Nickname.Equals(userName, StringComparison.OrdinalIgnoreCase));
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

        public Users GetNewWithDefaultInitialization()
        {
            Users user = new Users
            {
                State = Consts.StateNew
            };
            return user;
        }

        public void Add(Users user)
        {
            Db.Users.Add(user);
            Db.SaveChanges();
        }
    }
}