//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfServiceDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoryLog
    {
        public CategoryLog()
        {
            this.LogBook = new HashSet<LogBook>();
        }
    
        public int Id { get; set; }
        public string Category { get; set; }
    
        public virtual ICollection<LogBook> LogBook { get; set; }
    }
}
