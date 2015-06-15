using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace CarMate.DAL
{
    public class BaseRep<T> where T : class
    {
        protected readonly CarMateEntities Db;//= new Entities();

        protected DbSet<T> DbSet { get; set; }

        public BaseRep(CarMateEntities dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            Db = dbContext;
            DbSet = Db.Set<T>();

            //RepContext = repContext;
        }

        public T GetById(Guid id)
        {
            var res = DbSet.Find(id);
            if (res == null)
            {
                throw new ApplicationException("Не удалось получить объект " + typeof(T).Name + " по коду " + id.ToString() + "!");
            }
            return res;
        }

        public T FindById(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry entry = Db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public bool IsDetached(T entity)
        {
            DbEntityEntry entry = Db.Entry(entity);
            return entry.State == EntityState.Detached;
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = Db.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
            //if (entit)
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = Db.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
    }
}