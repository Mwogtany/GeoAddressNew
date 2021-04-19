using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class MyPageViewModel
    {
    }
    public class TopSummaryViewModel
    {
        [Display(Name = "Farmers")]
        public int Farmers { get; set; }
        [Display(Name = "Schools")]
        public int Schools { get; set; }
        [Display(Name = "Households")]
        public int Households { get; set; }
        [Display(Name = "Health Centers")]
        public int Hospitals { get; set; }
        [Display(Name = "Businesses")]
        public int Businesses { get; set; }
    }
    public class MyRecordsViewModel
    {
        [Display(Name = "Farmers")]
        public int Farmers { get; set; }
        [Display(Name = "Schools")]
        public int Schools { get; set; }
        [Display(Name = "Households")]
        public int Households { get; set; }
        [Display(Name = "Health Centers")]
        public int Hospitals { get; set; }
        [Display(Name = "Businesses")]
        public int Businesses { get; set; }
        //public int FarmersCurService { get; set; }
        public List<vw_Farmer_Service_Requests> FarmersCurService { get; set; }
        public int FarmersPrevService { get; set; }
        public int ActiveProvidedService { get; set; }
        public int InActiveProvidedService { get; set; }

    }

    public class ServiceViewModel
    {
        [Display(Name = "Farmers")]
        public int Farmers { get; set; }
        [Display(Name = "Schools")]
        public int Schools { get; set; }
        [Display(Name = "Households")]
        public int Households { get; set; }
        [Display(Name = "Health Centers")]
        public int Hospitals { get; set; }
    }


    public class NewServiceViewModel
    {
        public int BaseID { get; set; }
        public int ServiceRunID { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [Display(Name = "Service")]
        public int ServiceID { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public IList<SelectListItem> CategoryList { get; set; }
        public IList<SelectListItem> ServiceList { get; set; }

        public List<vw_Farmer_Service_Requests> FarmerCurServiceList { get; set; }
    }
    public class ServicesViewModel
    {
        public int BaseID { get; set; }
        public int ServiceRunID { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [Display(Name = "Service")]
        public int ServiceID { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<bool> Published { get; set; }
        public Nullable<bool> ServiceSolved { get; set; }
        public string UserID { get; set; }
        public string ServiceDescription { get; set; }
        public string CategoryDescription { get; set; }

    }

    public class ProvideServiceViewModel
    {
        [Display(Name = "Category")]
        public Nullable<int> category_id { get; set; }
        public int ProviderID { get; set; }
        public int userid { get; set; }
        [Display(Name = "Service")]
        public int ServiceID { get; set; }
        public string Profession { get; set; }
        public string Qualification { get; set; }
        [Display(Name = "Service Description")]
        [DataType(DataType.MultilineText)]
        public string ServiceDescription { get; set; }
        public string Status { get; set; }
        public string StatusChangedBy { get; set; }
        public Nullable<System.DateTime> StatusChangedOn { get; set; }

        public IList<SelectListItem> CategoryList { get; set; }
        public IList<SelectListItem> ServiceList { get; set; }

        public List<vw_Service_Provider> ServicesProvidedList { get; set; }
    }
}