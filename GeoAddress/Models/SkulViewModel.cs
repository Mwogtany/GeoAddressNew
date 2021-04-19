using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class SkulViewModel
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
        [Display(Name = "NEMIS Code")]
        public string NEMIS_CODE { get; set; }
        [Required]
        [Display(Name = "Institution Name")]
        public string INSTITUTION_NAME { get; set; }
        [Required]
        [Display(Name = "Level")]
        public string Level_Code { get; set; }
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

        public IList<SelectListItem> Levels { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
    public class SkulViewModel2
    {
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string Pluscode { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "NEMIS Code")]
        public string NEMIS_CODE { get; set; }
        [Required]
        [Display(Name = "Institution Name")]
        public string INSTITUTION_NAME { get; set; }
        [Required]
        [Display(Name = "Level")]
        public string Level_Code { get; set; }
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

        public IList<SelectListItem> Levels { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
}