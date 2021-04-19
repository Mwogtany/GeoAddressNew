using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class HospViewModel
    {
        [Required]
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
        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
        [Display(Name = "Phone")]
        public string ContactPhone { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string Building { get; set; }
        [Required]
        [Display(Name = "Level")]
        public string Level_ID { get; set; }
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
    public class HospViewModel2
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
        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
        [Display(Name = "Phone")]
        public string ContactPhone { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string Building { get; set; }
        [Required]
        [Display(Name = "Level")]
        public string Level_ID { get; set; }
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