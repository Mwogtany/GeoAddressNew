using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class HseViewModel
    {
        public int BaseID { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string Pluscode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Household Name")]
        public string HouseHoldName { get; set; }
        [Required]
        [Display(Name = "Household Type")]
        public string HouseHoldTypeId { get; set; }
        [Display(Name = "Phone")]
        [Required]
        public string ContactPhone { get; set; }
        [Display(Name = "E-mail")]
        [EmailAddress]
        [Required]
        public string ContactEmail { get; set; }
        [Display(Name = "Website")]
        [Url]
        public string Website { get; set; }
        public string Building { get; set; }
        [Display(Name = "County")]
        [Required]
        public string County_Code { get; set; }
        [Display(Name = "Constituency")]
        [Required]
        public string Constituency_Code { get; set; }
        [Display(Name = "Sub-County")]
        [Required]
        public string Sub_County_Code { get; set; }
        [Display(Name = "Ward")]
        [Required]
        public string Ward_Code { get; set; }
        [Display(Name = "Village")]
        [Required]
        public string Village { get; set; }
        [Display(Name = "Land Title/Ownership")]
        public string LandRegistration { get; set; }
        [Display(Name = "No of Workers")]
        public int NoWorkers { get; set; }


        public IList<SelectListItem> HseTypes { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
    public class HseViewModel2
    {
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string Pluscode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Household Name")]
        public string HouseHoldName { get; set; }
        [Required]
        [Display(Name = "Household Type")]
        public int HouseHoldTypeId { get; set; }
        [Display(Name = "Phone")]
        [Required]
        public string ContactPhone { get; set; }
        [Display(Name = "E-mail")]
        [EmailAddress]
        [Required]
        public string ContactEmail { get; set; }
        [Display(Name = "Website")]
        [Url]
        public string Website { get; set; }
        public string Building { get; set; }
        [Display(Name = "County")]
        [Required]
        public string County_Code { get; set; }
        [Display(Name = "Constituency")]
        [Required]
        public string Constituency_Code { get; set; }
        [Display(Name = "Sub-County")]
        [Required]
        public string Sub_County_Code { get; set; }
        [Display(Name = "Ward")]
        [Required]
        public string Ward_Code { get; set; }
        [Display(Name = "Village")]
        [Required]
        public string Village { get; set; }
        [Display(Name = "Land Title/Ownership")]
        public string LandRegistration { get; set; }
        [Display(Name = "No of Workers")]
        public int NoWorkers { get; set; }


        public IList<SelectListItem> HseTypes { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
}