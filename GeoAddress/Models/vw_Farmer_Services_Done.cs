//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeoAddress.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_Farmer_Services_Done
    {
        public int ServiceRunID { get; set; }
        public int BaseID { get; set; }
        public int ServiceID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Published { get; set; }
        public Nullable<bool> ServiceSolved { get; set; }
        public string UserID { get; set; }
        public string ServiceDescription { get; set; }
        public string CategoryDescription { get; set; }
        public int CategoryID { get; set; }
    }
}