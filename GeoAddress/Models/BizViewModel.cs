using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class BizViewModel
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
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
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

        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
    public class BizViewModel2
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
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
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
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
}