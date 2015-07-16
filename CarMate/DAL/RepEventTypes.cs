using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.Classes;

namespace CarMate.DAL
{
    public class RepEventTypes : BaseRep<EventTypes>
    {
        public RepEventTypes(CarMateEntities db)
            : base(db)
        {
            //_db = db;
        }

        //public IQueryable<EventTypes> Select(int languageId)
        //{
        //    var res = Db.EventTypes.Where(x => x.LanguageId == languageId);
        //    return res;
        //}

        public IQueryable<EventTypes> Select(int languageId)
        {
            List<EventTypes> eventTypesList = new List<EventTypes>();
            var eventTypesLang = Db.EventTypesLang.Where(x => x.LanguageId == languageId).ToList();
            for (int i = 0; i < eventTypesLang.Count(); i++)
            {
                eventTypesList.Add(new EventTypes()
                {
                    Name = eventTypesLang[i].Name,
                    Id = eventTypesLang[i].EventTypesId
                });
            }

            return eventTypesList.AsQueryable();
        }

        public string SelectAnalogue(string eventTypeName)
        {
            var eventTypesLang = Db.EventTypesLang.First(x => x.Name.Equals(eventTypeName));
            int eventTypesId = eventTypesLang.EventTypesId;
            var res = Db.EventTypesLang.First(x => x.EventTypesId == eventTypesId && x.Languages.Code.Equals("ru")).Name;

            return res;
        }

        public string SelectAnalogueFromLanguage(string eventTypeName, string language)
        {
            var eventTypesLang = Db.EventTypesLang.First(x => x.Name.Equals(eventTypeName));
            int eventTypesId = eventTypesLang.EventTypesId;
            var res = Db.EventTypesLang.First(x => x.EventTypesId == eventTypesId && x.Languages.Code.Equals(language)).Name;

            return res;
        }
    }
}