//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarMate
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventTypesLang
    {
        public int Id { get; set; }
        public int EventTypesId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
    
        public virtual EventTypes EventTypes { get; set; }
        public virtual Languages Languages { get; set; }
    }
}
