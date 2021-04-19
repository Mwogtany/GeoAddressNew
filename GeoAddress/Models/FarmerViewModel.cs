using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GeoAddress.Models
{
    public class FarmerViewModel
    {
        //HouseHold Details
        [Key]
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
        public string LandRegistrationDesc { get; set; }
        [Display(Name = "No of Workers")]
        public int NoWorkers { get; set; }
        // Farmer Specific
        [Display(Name = "Category")]
        [Required]
        public string CategoryID { get; set; }
        [Display(Name = "Agriculture Type")]
        [Required]
        public string TypeID { get; set; }
        [Display(Name = "Agriculture Practice")]
        [Required]
        public string ItemID { get; set; }

        [Display(Name = "Who Supports the Farm")]
        public string SupportedBy { get; set; }
        [Display(Name = "Which Help is Required")]
        public string SupportSort { get; set; }
        public string Challenges { get; set; }

        //Drop Down List
        public IList<SelectListItem> AgriCategory { get; set; }
        public IList<SelectListItem> AgriType { get; set; }
        public IList<SelectListItem> AgriItem { get; set; }
        public IList<SelectListItem> HseTypes { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
    }
    public class FarmerViewModel2
    {
        //HouseHold Details
        [Key]
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
        [Display(Name = "Farm/Household Name")]
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
        [Display(Name = "Land Registration")]
        public string LandRegistration { get; set; }
        public string LandRegistrationDesc { get; set; }
        [Display(Name = "No of Workers")]
        public int NoWorkers { get; set; }
        // Farmer Specific
        [Display(Name = "Category")]
        [Required]
        public Nullable<int> CategoryID { get; set; }
        [Display(Name = "Agriculture Type")]
        [Required]
        public Nullable<int> TypeID { get; set; }
        [Display(Name = "Agriculture Practice")]
        [Required]
        public Nullable<int> ItemID { get; set; }

        [Display(Name = "Who Supports the Farm")]
        public string SupportedBy { get; set; }
        [Display(Name = "Which Help is Required")]
        public string SupportSort { get; set; }
        public string Challenges { get; set; }

        [Display(Name = "Surname")]
        public string ContactSurname { get; set; }
        [Display(Name = "First Name")]
        public string ContactFirstname { get; set; }
        [Display(Name = "Other Name")]
        public string ContactOthername { get; set; }
        [Display(Name = "Gender")]
        public string ContactGender { get; set; }
        [Display(Name = "Mariral Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FarmerDateOfBirth { get; set; }
        [Display(Name = "Formal Training in Agriculture")]
        [UIHint("YesNo")]
        public Nullable<bool> FormalTraining { get; set; }
        [Display(Name = "Highest Education Level")]
        public string FormalHighestEducation { get; set; }

        //Drop Down List
        public IList<SelectListItem> AgriCategory { get; set; }
        public IList<SelectListItem> AgriType { get; set; }
        public IList<SelectListItem> AgriItem { get; set; }
        public IList<SelectListItem> HseTypes { get; set; }
        public IList<SelectListItem> CountyNames { get; set; }
        public IList<SelectListItem> SubCountyNames { get; set; }
        public IList<SelectListItem> Constituencies { get; set; }
        public IList<SelectListItem> Wards { get; set; }
        public IList<SelectListItem> Maritals { get; set; }
        public IList<SelectListItem> Academics { get; set; }
        public IList<SelectListItem> LandOwner { get; set; }
        public IList<SelectListItem> Genders { get; set; }
    }

    public class FarmerViewModel3
    {
        //HouseHold Details
        [Key]
        public int BaseID { get; set; }
        [Display(Name = "Total Land Acreage (Convert to Acres)")]
        public Nullable<decimal> LandAcre { get; set; }
        [Display(Name = "Group Category (CIG/VMG/PO/Coop)")]
        public string Group_CIG_VGM_PO { get; set; }
        [Display(Name = "Name of Group Category")]
        public string Group_Name { get; set; }
        [Display(Name = "Number of Years for Leasing Land")]
        public Nullable<decimal> LeaseYears { get; set; }

        [Display(Name = "Legal Status Holding")]
        public string LegalStatusHolding { get; set; }
        [Display(Name = "LR/Cert. No/Agreement no")]
        public string LRNo { get; set; }
        [Display(Name = "Do you have another farm elsewhere?")]
        public Nullable<bool> OtherFarm { get; set; }

        [Display(Name = "Growing crops for subsistence?")]
        public Nullable<bool> CropSubsistence { get; set; }
        [Display(Name = "Growing crops for sale?")]
        public Nullable<bool> CropSale { get; set; }
        [Display(Name = "Rearing livestock for subsistence?")]
        public Nullable<bool> LivestockSubsistence { get; set; }
        [Display(Name = "Rearing livestock for sale?")]
        public Nullable<bool> LivestockSale { get; set; }
        [Display(Name = "Aquaculture for subsistence?")]
        public Nullable<bool> AquacultureSubsistence { get; set; }
        [Display(Name = "Aquaculture for sale?")]
        public Nullable<bool> AquacultureSale { get; set; }
        [Display(Name = "Tree farming?")]
        public Nullable<bool> TreeFarming { get; set; }

        public IList<SelectListItem> Groups { get; set; }
        public IList<SelectListItem> Holdings { get; set; }
    }

    //Crop Management
    public class FarmerViewModel4
    {
        [Key]
        public int BaseID { get; set; }
        //Multi_select Feature (search how to implement this)
        [Display(Name = "What production system do you use?")]
        public string CropProductionSystem { get; set; }

        [Display(Name = "Acreage under crop Value Chain:")]
        public Nullable<decimal> CropAcreage { get; set; }
        [Display(Name = "Average Yield per acre (Compute average yield per acre for the last 2 seasons):")]
        public Nullable<decimal> CropAverageAcreYield { get; set; }
        [Display(Name = "When did you plant your crop (this year : Date of Planting)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateCropdPlanted { get; set; }
        [Display(Name = "Variety of Crop Planted:")]
        public string VarietyOfCrop { get; set; }

        [Display(Name = "Do you use fertilizer as part of your crop production?")]
        public Nullable<bool> UseFertilizer { get; set; }
        //Rather than type, pick a multiselect feature; Where is fertilizer list?
        [Display(Name = "Which fertilizer(s) do you use in the production of crop (user CTRL Key for Multi-Select?")]
        public string FertilizerUse { get; set; }
        [Display(Name = "Average quantity of fertilizer used per acre in 2020 (in kgs):")]
        public Nullable<decimal> AverageFertilizerUse { get; set; }
        [Display(Name = "What Distance from the Farm the source of fertilizer for crop production (Kms)?")]
        public Nullable<decimal> FertilizerSourceDistince { get; set; }
        [Display(Name = "Name of place where fertilizer is sourced from:")]
        public string PlaceFertilizerSourced { get; set; }
        [Display(Name = "Do you grow other crops in this farm other than the KCSAP supported crop Value Chains?")]
        public Nullable<bool> SupportOtherCrops { get; set; }
        //Multi-select option
        [Display(Name = "Which other chemical interventions do you use on the crops?")]
        public string CropChemicalUse { get; set; }

        public IList<SelectListItem> CropProdSystemList { get; set; }
        public string[] SelCropProdSystemList { get; set; }

        public IList<SelectListItem> CropChemicalUseList { get; set; }
        public string[] SelCropChemicalUseList { get; set; }

        public IList<SelectListItem> FertilizerList { get; set; }
        public string[] SelFertilizerList { get; set; }

        //View Captured Crops Here
        public List<vw_Farmer_Crops> Crops { get; set; }

    }
    public class FarmerCropViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Crop")]
        public int CropID { get; set; }
        [Display(Name = "Acreage")]
        public Nullable<decimal> Acreage { get; set; }
        public string Purpose { get; set; }
        [Display(Name = "Water Source")]
        public Nullable<int> WaterSourceID { get; set; }
        public string Seeds { get; set; }
        
        public IList<SelectListItem> WaterSource { get; set; }
        public IList<SelectListItem> Crops { get; set; }

        public List<vw_Farmer_Crops> CropsList { get; set; }

    }
    //Livestock Management
    public class FarmerViewModel5
    {
        [Key]
        public int BaseID { get; set; }
        //Multi-select
        [Display(Name = "What are your main livestock feed items? (For each specify quantity)?")]
        public string main_livestock_feed { get; set; }
        public IList<SelectListItem> main_livestock_feedlist { get; set; }
        public string[] SelLivestockList { get; set; }

        public List<vw_Livestock_Stock> LivestockList { get; set; }
        public List<vw_Livestock_Feeds> LivestockFeedsList { get; set; }

    }

    public class FarmerLivestockViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Livestock")]

        public int LivestockID { get; set; }
        [Display(Name = "Production System")]
        public int ProductionID { get; set; }
        [Display(Name = "Stock (Number)")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public Nullable<int> Stock { get; set; }

        public IList<SelectListItem> Livestocklist { get; set; }
        public IList<SelectListItem> ProductionSysList { get; set; }
        
        public List<vw_Livestock_Stock> FarmerLivestockList { get; set; }

    }

    public class FarmerLivestockFeedViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Feed")]

        public int FeedID { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public Nullable<int> Quantity { get; set; }

        public IList<SelectListItem> Feedslist { get; set; }

        public List<vw_Livestock_Feeds> FarmerFeedsList { get; set; }

    }

    //Aquaculture Management
    public class FarmerViewModel6
    {
        [Key]
        public int BaseID { get; set; }
        [Required]
        [Display(Name = "What types of aquaculture do you practice?")]
        public string AquacultureType { get; set; }
        public IList<SelectListItem> AquacultureTypeList { get; set; }
        public string[] SelAquacultureTypeList { get; set; }
        [Required]
        [Display(Name = "What are your main inputs?")]
        public string AquacultureInputs { get; set; }
        public IList<SelectListItem> AquacultureInputsList { get; set; }
        public string[] SelAquacultureInputsList { get; set; }
        [Display(Name = "Do you utilize fertilizer in the ponds?")]
        public Nullable<bool> FertilizerPonds { get; set; }
        [Display(Name = "What is your production level?")]
        public string AquacultureLevel { get; set; }
        public IList<SelectListItem> AquacultureLevelList { get; set; }
        public string[] SelAquacultureLevelList { get; set; }

        //Display other Aquaculture Tables Information using Views, Editable elsewhere
        public List<vw_Aquaculture_ProductionSystem> FarmerAquaProdSysList { get; set; }
        public List<vw_Aquaculture_Species> FarmerAquaSpeciesList { get; set; }
    }

    public class FarmerAquacultureProdViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Production System")]
        public int ProductID { get; set; }
        [Display(Name = "Active Units")]
        public Nullable<int> ActiveUnits { get; set; }
        [Display(Name = "Active Are/Volume")]
        public Nullable<decimal> Area_Volume { get; set; }
        [Display(Name = "In-Active Units")]
        public Nullable<int> InactiveUnits { get; set; }
        [Display(Name = "In-Active Are/Volume")]
        public Nullable<decimal> InAcArea_Volume { get; set; }

        public IList<SelectListItem> ProdSyslist { get; set; }

        public List<vw_Aquaculture_ProductionSystem> FarmerAquaProdSyslist { get; set; }

    }

    public class FarmerAquacultureSpeciesViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Species")]
        public int SpeciesID { get; set; }
        [Display(Name = "Production System")]
        public int ProductID { get; set; }
        [Display(Name = "Fingerlings")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public Nullable<int> Fingerlings { get; set; }

        public IList<SelectListItem> AquaSpecieslist { get; set; }
        public IList<SelectListItem> ProdSyslist { get; set; }

        public List<vw_Aquaculture_Species> FarmerAquaSpecieslist { get; set; }

    }

    //Farm Mechanization
    public class FarmerViewModel7
    {
        [Key]
        public int BaseID { get; set; }
        [Display(Name = "What is the main source of power for farm activities?")]
        public string PowerSource { get; set; }
        public IList<SelectListItem> PowerSourceList { get; set; }
        [Display(Name = "For labor, what is the main source?")]
        public string FarmLaborSource { get; set; }
        public IList<SelectListItem> FarmLaborSourceList { get; set; }
        //Multi-select option
        [Display(Name = "What farm structures do you have?")]
        public string FarmStructures { get; set; }
        public IList<SelectListItem> FarmStructuresList { get; set; }
        public string[] SelFarmStructuresList { get; set; }
        [Display(Name = "Who owns most of the frequently used equipment?")]
        public string FarmEquipmentOwner { get; set; }
        public IList<SelectListItem> FarmEquipmentOwnerList { get; set; }
        //Used to Display List of Equipment owned by farmers
        public List<vw_Farmer_Equipment_Qty> FarmerEquipmentlist { get; set; }
    }

    public class FarmerEquipmentViewModel
    {
        public int BaseID { get; set; }
        [Display(Name = "Equipment")]
        public int EquipID { get; set; }
        [Display(Name = "Quantity")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public Nullable<int> Qty { get; set; }

        public IList<SelectListItem> Equipmentlist { get; set; }

        public List<vw_Farmer_Equipment_Qty> FarmerEquipmentlist { get; set; }

    }
    //Land and Water Management
    public class FarmerViewModel8
    {
        [Key]
        public int BaseID { get; set; }
        //Multi-select
        [Display(Name = "What sustainable land management practices do you carry out?")]
        public string LandManagementPractices { get; set; }
        public IList<SelectListItem> LandManagementPracticesList { get; set; }
        public string[] SelLandManagementPractices { get; set; }
        //Multi-select
        [Display(Name = "What are the main types of fertilizer used on this farm?")]
        public string FertilizerTypeUsed { get; set; }
        public IList<SelectListItem> FertilizerTypeUsedList { get; set; }
        public string[] SelFertilizerTypeUsed { get; set; }
        [Display(Name = "Do you use lime on your soil?")]
        public Nullable<bool> LimeOnSoil { get; set; }
        [Display(Name = "Have you done soil testing in your main farm in the last 3 years?")]
        public Nullable<bool> SoilTesting { get; set; }
        //Multi-select might be allowed (consult)
        [Display(Name = "What is the main source of water for farm activities?")]
        public string MainWaterSource { get; set; }
        public IList<SelectListItem> MainWaterSourceList { get; set; }
        [Display(Name = "Do you undertake irrigation?")]
        public Nullable<bool> IrrigationDone { get; set; }
        //Multi-select options might be needed
        [Display(Name = "Type of irrigation")]
        public string TypeOfIrrigation { get; set; }
        public IList<SelectListItem> TypeOfIrrigationList { get; set; }
        [Display(Name = "What is the total area under irrigation?")]
        public Nullable<decimal> TotalAreaIrrigated { get; set; }
        [Display(Name = "Type of irrigation project:")]
        public string TypeIrrigationProject { get; set; }
        public IList<SelectListItem> TypeIrrigationProjectList { get; set; }

        [Display(Name = "For irrigation schemes, what is the implementing body?")]
        public string IrrigationImplementedBy { get; set; }
        public IList<SelectListItem> IrrigationImplementedByList { get; set; }

        [Display(Name = "What is your main source of financial livelihood?")]
        public string SourceFinancialLivehood { get; set; }
        public IList<SelectListItem> SourceFinancialLivehoodList { get; set; }
        [Display(Name = "Do you belong to any of the following types of groups?")]
        public string FarmerGroupAffiliate { get; set; }
        public IList<SelectListItem> FarmerGroupAffiliateList { get; set; }
        public string[] SelFarmerGroupAffiliate { get; set; }
        //Multi-select feature, multiple funding sources can be selected
        [Display(Name = "Where do you access financial / credit services to facilitate your operations?")]
        public string FinancialServices { get; set; }
        public IList<SelectListItem> FinancialServicesList { get; set; }
        public string[] SelFinancialServices { get; set; }
        [Display(Name = "Do you insure your crops?")]
        public Nullable<bool> InsureCrops { get; set; }
        [Display(Name = "Do you insure your livestock?")]
        public Nullable<bool> InsureLivestock { get; set; }
        [Display(Name = "Do you insure your farm buildings and other assets?")]
        public Nullable<bool> InsureFarmAssets { get; set; }
        [Display(Name = "Do you keep written farm records?")]
        public Nullable<bool> KeepFarmRecords { get; set; }
        [Display(Name = "What is your main source of information on good agricultural practices (GAP)?")]
        public string SourceGAP { get; set; }
        public IList<SelectListItem> SourceGAPList { get; set; }
        [Display(Name = "Who is your main extension service provider?")]
        public string MainExtensionProvider { get; set; }
        public IList<SelectListItem> MainExtensionProviderList { get; set; }
        [Display(Name = "Which is your preferred format for receiving agricultural advisories?")]
        public string FormatForAdvisories { get; set; }
        public IList<SelectListItem> FormatForAdvisoriesList { get; set; }
        [Display(Name = "Preferred time for receiving agricultural advisories?")]
        public string PreferredTimeAdvisories { get; set; }
        public IList<SelectListItem> PreferredTimeAdvisoriesList { get; set; }
        [Display(Name = "Which is your preferred language for receiving agricultural advisories?")]
        public string PreferredLanguageAdvisories { get; set; }
        public IList<SelectListItem> PreferredLanguageAdvisoriesList { get; set; }
        [Display(Name = "Specify Other Language")]
        public string PreferredLanguageAdvisoriesOther { get; set; }
    }
}