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






    //public class RepCatalogHotel : BaseRep<CatalogHotel>
    //{
    //    //private readonly Entities _db = new Entities();

    //    public RepCatalogHotel(Entities db)
    //        : base(db)
    //    {
    //        //_db = db;
    //    }

    //    public CatalogHotel GetNewWithDefaultInitialization()
    //    {
    //        var res = new CatalogHotel
    //        {
    //            Id = Guids.GetCombGuid(),
    //            PublicationStateId = new Guid(UtilsConst.ConstPublicationStateIdDraft)
    //        };
    //        return res;
    //    }

    //    public CatalogHotel FindById(Guid id, bool includeDeleted)
    //    {
    //        var res = FindById(id);
    //        if (res != null)
    //        {
    //            if (!includeDeleted)
    //            {
    //                if (res.SysBaseObject.IsDeleted) res = null;
    //            }
    //        }
    //        return res;
    //    }

    //    public CatalogHotel FindByIdOrWebAlias(string idOrWebAlias, bool includeDeleted = true)
    //    {
    //        return
    //            Guids.IsGuid(idOrWebAlias)
    //                ? FindById(new Guid(idOrWebAlias), includeDeleted)
    //                : FindByWebAlias(idOrWebAlias, includeDeleted);
    //    }


    //    public CatalogHotel GetByIdOrWebAlias(string idOrWebAlias, bool includeDeleted = true)
    //    {
    //        var res = FindByIdOrWebAlias(idOrWebAlias, includeDeleted);
    //        if (res == null)
    //        {
    //            throw new ApplicationException("Не удалось получить ОР по Id или алиасу " + idOrWebAlias + "!");
    //        }
    //        return res;
    //    }



    //    public CatalogHotel FindByWebAlias(string webAlias, bool includeDeleted = true)
    //    {
    //        var q = _db.CatalogHotel.Where(i => i.WebAlias == webAlias);
    //        if (!includeDeleted)
    //        {
    //            q = q.Where(i => !i.SysBaseObject.IsDeleted);
    //        }
    //        var res = q.FirstOrDefault();//ToList();
    //        //if (q != null)
    //        //{
    //        //    if (!includeDeleted)
    //        //    {
    //        //        if (q.SysBaseObject.IsDeleted)
    //        //        {
    //        //            q = null;
    //        //        }
    //        //    }
    //        //}
    //        return res;
    //    }

    //    public CatalogHotel GetByWebAlias(string webAlias)
    //    {
    //        var res = FindByWebAlias(webAlias);
    //        if (res == null)
    //        {
    //            throw new ApplicationException("Не удалось получить ОР по алиасу " + webAlias + "!");
    //        }
    //        return res;
    //        //return _db.CatalogHotel.FirstOrDefault(i => i.WebAlias == webAlias);
    //    }


    //    public IQueryable<CatalogHotel> Select(
    //        bool showDeleted = false)
    //    {
    //        var res = _db.CatalogHotel as IQueryable<CatalogHotel>;
    //        if (!showDeleted)
    //        {
    //            res = res.Where(i => !i.SysBaseObject.IsDeleted);
    //        }
    //        return res;
    //    }

    //    public IQueryable<CatalogHotel> SelectOnlyPublished(
    //        bool showDeleted = false)
    //    {
    //        var q = Select(showDeleted);
    //        q = q.Where(i => i.PublicationStateId == new Guid(UtilsConst.ConstPublicationStateIdPublished));
    //        return q;
    //    }

    //    public void Add(CatalogHotel catalogHotel, Guid userId)
    //    {
    //        var sbo = new RepSysBaseObject(_db);
    //        catalogHotel.SysBaseObject = sbo.Create(
    //                catalogHotel.Id,
    //                new Guid(UtilsConst.SysObjectDefinitionIdCatalogHotel),
    //                userId
    //                );
    //        DbSet.Add(catalogHotel);
    //    }

    //    public void Update(CatalogHotel catalogHotel, Guid userId)
    //    {
    //        var sbo = new RepSysBaseObject(_db);
    //        Update(catalogHotel);
    //        sbo.Edit(catalogHotel.Id, UtilsConst.SysUserIdAdmin);
    //    }

    //    public void AddOrUpdate(CatalogHotel hotel, Guid userId)
    //    {
    //        if (IsDetached(hotel))
    //        {
    //            Add(hotel, userId);
    //        }
    //        else
    //        {
    //            Update(hotel, userId);
    //        }
    //    }
    //}

}



