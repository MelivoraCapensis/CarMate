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
    
    public partial class VendorAliases
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int AliasId { get; set; }
    
        public virtual Aliases Aliases { get; set; }
        public virtual Vendors Vendors { get; set; }
    }
}