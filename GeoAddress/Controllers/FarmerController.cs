using GeoAddress.Models;
using GeoAddress.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Controllers
{
    [Authorize] 
    public class FarmerController : Controller
    {
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        // GET: Farmer
        public ActionResult Index()
        {
            IEnumerable<FarmerViewModel> bizness = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            ViewBag.LocalPath = localpath;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Farmer/All/" + mUser.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FarmerViewModel>>();
                    readTask.Wait();

                    bizness = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bizness = Enumerable.Empty<FarmerViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            //KEGooglePlusEntities Db = new KEGooglePlusEntities();
            //var myrole = (from m in Db.UserRoleAssignments
            //              where m.UserID == mUser
            //              select m).SingleOrDefault();

            //var entity = (from p in Db.HOUSEHOLDS
            //              join r in Db.BaseTables on p.BaseID equals r.BaseID
            //              join z in Db.FARMERs on p.BaseID equals z.BaseID
            //              join w in Db.STATIC_HOUSEHOLD_TYPE on p.HouseHoldTypeId equals w.HouseHoldTypeID
            //              join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
            //              join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
            //              join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
            //              join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
            //              join m in Db.STATIC_AGRICULTURE_CATEGORY on z.CategoryID equals m.CategoryID
            //              join n in Db.STATIC_AGRICULTURE_TYPE on z.TypeID equals n.TypeID
            //              join k in Db.STATIC_AGRICULTURE_ITEM on z.ItemID equals k.ItemID
            //              from cty in ctydb.DefaultIfEmpty()
            //              from scty in sctydb.DefaultIfEmpty()
            //              from cons in consdb.DefaultIfEmpty()
            //              from wds in wdsdb.DefaultIfEmpty()
            //              where r.Category == "F" && (r.UserID == mUser || myrole.RoleID == 1)
            //              select new FarmerViewModel()
            //              { // result selector 
            //                  BaseID = p.BaseID,
            //                  HouseHoldName = p.HouseHoldName,
            //                  HouseHoldTypeId = w.HouseHoldType,
            //                  ContactPhone = p.ContactPhone,
            //                  ContactEmail = p.ContactEmail,
            //                  Website = p.Website,
            //                  Building = p.Building,
            //                  County_Code = cty.County_Name,
            //                  Constituency_Code = cons.Constituency_Name,
            //                  Sub_County_Code = scty.Sub_County_Name,
            //                  Ward_Code = wds.Ward_Name,
            //                  Village = p.Village,
            //                  LandRegistration = p.LandRegistration,
            //                  NoWorkers = (int)p.NoWorkers,
            //                  Latitude = r.Latitude,
            //                  Longitude = r.Longitude,
            //                  Pluscode = r.Pluscode,
            //                  Address = r.Address,
            //                  CategoryID = m.Category,
            //                  TypeID = n.TypeDescription,
            //                  ItemID = k.ItemDescription
            //              }).ToArray();

            return View(bizness);
        }

        // GET: Farmer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Farmer/Create
        public ActionResult Create()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            FarmerViewModel2 ViewModel = new FarmerViewModel2();
            Session["id"] = 0;
            List<SelectListItem> hseTypes = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();
            List<SelectListItem> agrCategories = new List<SelectListItem>();
            List<SelectListItem> agrType = new List<SelectListItem>();
            List<SelectListItem> agrItem = new List<SelectListItem>();

            List<SelectListItem> Maritals = new List<SelectListItem>();
            List<SelectListItem> Academics = new List<SelectListItem>();
            List<SelectListItem> Ownereship = new List<SelectListItem>();
            List<SelectListItem> Genders = new List<SelectListItem>();

            List<STATIC_HOUSEHOLD_TYPE> ctypes = Db.STATIC_HOUSEHOLD_TYPE.ToList();
            ctypes.ForEach(x =>
            {
                hseTypes.Add(new SelectListItem { Text = x.HouseHoldType, Value = x.HouseHoldTypeID.ToString() });
            });

            ViewModel.HseTypes = hseTypes;

            List<COUNTY> cties = Db.COUNTies.ToList();
            cties.ForEach(x =>
            {
                countyNames.Add(new SelectListItem { Text = x.County_Name, Value = x.County_Code.ToString() });
            });
            ViewModel.CountyNames = countyNames;

            List<SUB_COUNTY> scties = Db.SUB_COUNTY.ToList();
            scties.ForEach(x =>
            {
                scountyNames.Add(new SelectListItem { Text = x.Sub_County_Name, Value = x.Sub_County_Code.ToString() });
            });
            ViewModel.SubCountyNames = scountyNames;

            List<CONSTITUENCY> cons = Db.CONSTITUENCies.ToList();
            cons.ForEach(x =>
            {
                consNames.Add(new SelectListItem { Text = x.Constituency_Name, Value = x.Constituency_Code.ToString() });
            });
            ViewModel.Constituencies = consNames;

            List<WARD> wads = Db.WARDs.ToList();
            wads.ForEach(x =>
            {
                wardNames.Add(new SelectListItem { Text = x.Ward_Name, Value = x.Ward_Code.ToString() });
            });
            ViewModel.Wards = wardNames;

            List<STATIC_AGRICULTURE_CATEGORY> agrcat = Db.STATIC_AGRICULTURE_CATEGORY.ToList();
            agrcat.ForEach(x =>
            {
                agrCategories.Add(new SelectListItem { Text = x.Category, Value = x.CategoryID.ToString() });
            });
            ViewModel.AgriCategory = agrCategories;

            List<STATIC_AGRICULTURE_TYPE> agrtyp = Db.STATIC_AGRICULTURE_TYPE.ToList();
            agrtyp.ForEach(x =>
            {
                agrType.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
            });
            ViewModel.AgriType = agrType;

            ViewModel.AgriItem = agrItem;


            List<STATIC_HOUSEHOLD_MARITAL> aMarred = Db.STATIC_HOUSEHOLD_MARITAL.ToList();
            aMarred.ForEach(x =>
            {
                Maritals.Add(new SelectListItem { Text = x.MaritalType, Value = x.MaritalID.ToString() });
            });
            List<STATIC_SCHOOL_LEVEL> aAcad = Db.STATIC_SCHOOL_LEVEL.ToList();
            aAcad.ForEach(x =>
            {
                Academics.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
            });
            List<STATIC_FARMER_LAND_REGISTRATION> aOwner = Db.STATIC_FARMER_LAND_REGISTRATION.ToList();
            aOwner.ForEach(x =>
            {
                Ownereship.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
            });
            List<STATIC_HOUSEHOLD_GENDER> aGender = Db.STATIC_HOUSEHOLD_GENDER.ToList();
            aGender.ForEach(x =>
            {
                Genders.Add(new SelectListItem { Text = x.GenderType, Value = x.GenderID.ToString() });
            });
            ViewModel.Academics = Academics;
            ViewModel.Maritals = Maritals;
            ViewModel.LandOwner = Ownereship;
            ViewModel.Genders = Genders;

            return View(ViewModel);
        }

        // POST: Farmer/Create
        [HttpPost]
        public ActionResult Create(FarmerViewModel2 model)
        {
            ViewBag.Message = "";
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var bizna = model;
                //Use BaseTable, Pick the next BaseID for the record

                var maxId = Db.BaseTables.Max(x => (int?)x.BaseID) ?? 0;

                int maxValue = maxId + 1;

                Session["id"] = maxValue;

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode,
                    Address = bizna.Address,
                    Category = "F",
                    UserID = mUser
                });

                Db.HOUSEHOLDS.Add(new HOUSEHOLD()
                {
                    BaseID = maxValue,
                    HouseHoldTypeId = bizna.HouseHoldTypeId,
                    HouseHoldName = bizna.HouseHoldName,
                    ContactPhone = bizna.ContactPhone,
                    ContactEmail = bizna.ContactEmail,
                    Website = bizna.Website,
                    Building = bizna.Building,
                    County_Code = bizna.County_Code,
                    Constituency_Code = bizna.Constituency_Code,
                    Sub_County_Code = bizna.Sub_County_Code,
                    Ward_Code = bizna.Ward_Code,
                    Village = bizna.Village,
                    LandRegistration = bizna.LandRegistration,
                    NoWorkers = bizna.NoWorkers,
                    ContactFirstname = bizna.ContactFirstname,
                    ContactSurname = bizna.ContactSurname,
                    ContactOthername = bizna.ContactOthername,
                    ContactGender = bizna.ContactGender,
                    MaritalStatus = bizna.MaritalStatus
                });
                
                Db.FARMERs.Add(new FARMER()
                {
                    BaseID = maxValue,
                    CategoryID = bizna.CategoryID,
                    TypeID = bizna.TypeID,
                    ItemID = bizna.ItemID,
                    SupportedBy = bizna.SupportedBy,
                    SupportSort = bizna.SupportSort,
                    Challenges = bizna.Challenges,
                    FormalTraining = bizna.FormalTraining,
                    FormalHighestEducation = bizna.FormalHighestEducation,
                    FarmerDateOfBirth = bizna.FarmerDateOfBirth
                });


                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<SelectListItem> hseTypes = new List<SelectListItem>();
                List<SelectListItem> countyNames = new List<SelectListItem>();
                List<SelectListItem> scountyNames = new List<SelectListItem>();
                List<SelectListItem> consNames = new List<SelectListItem>();
                List<SelectListItem> wardNames = new List<SelectListItem>();
                List<SelectListItem> agrCategories = new List<SelectListItem>();
                List<SelectListItem> agrType = new List<SelectListItem>();
                List<SelectListItem> agrItem = new List<SelectListItem>();

                List<SelectListItem> Maritals = new List<SelectListItem>();
                List<SelectListItem> Academics = new List<SelectListItem>();
                List<SelectListItem> Ownereship = new List<SelectListItem>();
                List<SelectListItem> Genders = new List<SelectListItem>();

                List<STATIC_HOUSEHOLD_TYPE> ctypes = Db.STATIC_HOUSEHOLD_TYPE.ToList();
                ctypes.ForEach(x =>
                {
                    hseTypes.Add(new SelectListItem { Text = x.HouseHoldType, Value = x.HouseHoldTypeID.ToString() });
                });

                model.HseTypes = hseTypes;

                List<COUNTY> cties = Db.COUNTies.ToList();
                cties.ForEach(x =>
                {
                    countyNames.Add(new SelectListItem { Text = x.County_Name, Value = x.County_Code.ToString() });
                });
                model.CountyNames = countyNames;

                List<SUB_COUNTY> scties = Db.SUB_COUNTY.ToList();
                scties.ForEach(x =>
                {
                    scountyNames.Add(new SelectListItem { Text = x.Sub_County_Name, Value = x.Sub_County_Code.ToString() });
                });
                model.SubCountyNames = scountyNames;

                List<CONSTITUENCY> cons = Db.CONSTITUENCies.ToList();
                cons.ForEach(x =>
                {
                    consNames.Add(new SelectListItem { Text = x.Constituency_Name, Value = x.Constituency_Code.ToString() });
                });
                model.Constituencies = consNames;

                List<WARD> wads = Db.WARDs.ToList();
                wads.ForEach(x =>
                {
                    wardNames.Add(new SelectListItem { Text = x.Ward_Name, Value = x.Ward_Code.ToString() });
                });
                model.Wards = wardNames;

                List<STATIC_AGRICULTURE_CATEGORY> agrcat = Db.STATIC_AGRICULTURE_CATEGORY.ToList();
                agrcat.ForEach(x =>
                {
                    agrCategories.Add(new SelectListItem { Text = x.Category, Value = x.CategoryID.ToString() });
                });
                model.AgriCategory = agrCategories;

                List<STATIC_AGRICULTURE_TYPE> agrtyp = Db.STATIC_AGRICULTURE_TYPE.ToList();
                agrtyp.ForEach(x =>
                {
                    agrType.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
                });
                model.AgriType = agrType;

                model.AgriItem = agrItem;


                List<STATIC_HOUSEHOLD_MARITAL> aMarred = Db.STATIC_HOUSEHOLD_MARITAL.ToList();
                aMarred.ForEach(x =>
                {
                    Maritals.Add(new SelectListItem { Text = x.MaritalType, Value = x.MaritalID.ToString() });
                });
                List<STATIC_SCHOOL_LEVEL> aAcad = Db.STATIC_SCHOOL_LEVEL.ToList();
                aAcad.ForEach(x =>
                {
                    Academics.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
                });
                List<STATIC_FARMER_LAND_REGISTRATION> aOwner = Db.STATIC_FARMER_LAND_REGISTRATION.ToList();
                aOwner.ForEach(x =>
                {
                    Ownereship.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
                });
                List<STATIC_HOUSEHOLD_GENDER> aGender = Db.STATIC_HOUSEHOLD_GENDER.ToList();
                aGender.ForEach(x =>
                {
                    Genders.Add(new SelectListItem { Text = x.GenderType, Value = x.GenderID.ToString() });
                });
                model.Academics = Academics;
                model.Maritals = Maritals;
                model.LandOwner = Ownereship;
                model.Genders = Genders;
                model.BaseID = maxValue;
            }
            return RedirectToAction("Edit", new { id = model.BaseID });
        }

        // GET: Farmer/Edit/5
        public ActionResult Edit(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            Session["id"] = id;

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel2()
                          { // result selector 
                              BaseID = p.BaseID,
                              HouseHoldName = p.HouseHoldName,
                              HouseHoldTypeId = (int)p.HouseHoldTypeId,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              Village = p.Village,
                              LandRegistration = p.LandRegistration,
                              NoWorkers = (int)p.NoWorkers,
                              County_Code = p.County_Code,
                              Constituency_Code = p.Constituency_Code,
                              Sub_County_Code = p.Sub_County_Code,
                              Ward_Code = p.Ward_Code,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode,
                              Address = r.Address,
                              CategoryID = w.CategoryID,
                              TypeID = w.TypeID,
                              ItemID = w.ItemID,
                              SupportedBy = w.SupportedBy,
                              SupportSort = w.SupportSort,
                              Challenges = w.Challenges,
                              
                              ContactFirstname = p.ContactFirstname,
                              ContactSurname = p.ContactSurname,
                              ContactOthername = p.ContactOthername,
                              ContactGender = p.ContactGender,
                              MaritalStatus = p.MaritalStatus,
                              FormalTraining = w.FormalTraining,
                              FormalHighestEducation = w.FormalHighestEducation,
                              FarmerDateOfBirth = w.FarmerDateOfBirth
                          }).SingleOrDefault();

            List<SelectListItem> hseTypes = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();
            List<SelectListItem> agrCategories = new List<SelectListItem>();
            List<SelectListItem> agrType = new List<SelectListItem>();
            List<SelectListItem> agrItem = new List<SelectListItem>();
            List<SelectListItem> Maritals = new List<SelectListItem>();
            List<SelectListItem> Academics = new List<SelectListItem>();
            List<SelectListItem> Ownereship = new List<SelectListItem>();
            List<SelectListItem> Genders = new List<SelectListItem>();

            List<STATIC_HOUSEHOLD_TYPE> ctypes = Db.STATIC_HOUSEHOLD_TYPE.ToList();
            ctypes.ForEach(x =>
            {
                hseTypes.Add(new SelectListItem { Text = x.HouseHoldType, Value = x.HouseHoldTypeID.ToString() });
            });
            entity.HseTypes = hseTypes;

            List<COUNTY> cties = Db.COUNTies.ToList();
            cties.ForEach(x =>
            {
                countyNames.Add(new SelectListItem { Text = x.County_Name, Value = x.County_Code.ToString() });
            });
            entity.CountyNames = countyNames;

            List<SUB_COUNTY> scties = Db.SUB_COUNTY.ToList();
            scties.ForEach(x =>
            {
                scountyNames.Add(new SelectListItem { Text = x.Sub_County_Name, Value = x.Sub_County_Code.ToString() });
            });
            entity.SubCountyNames = scountyNames;

            List<CONSTITUENCY> cons = Db.CONSTITUENCies.ToList();
            cons.ForEach(x =>
            {
                consNames.Add(new SelectListItem { Text = x.Constituency_Name, Value = x.Constituency_Code.ToString() });
            });
            entity.Constituencies = consNames;

            List<WARD> wads = Db.WARDs.ToList();
            wads.ForEach(x =>
            {
                wardNames.Add(new SelectListItem { Text = x.Ward_Name, Value = x.Ward_Code.ToString() });
            });
            entity.Wards = wardNames;

            List<STATIC_AGRICULTURE_CATEGORY> agrcat = Db.STATIC_AGRICULTURE_CATEGORY.ToList();
            agrcat.ForEach(x =>
            {
                agrCategories.Add(new SelectListItem { Text = x.Category, Value = x.CategoryID.ToString() });
            });
            entity.AgriCategory = agrCategories;

            List<STATIC_AGRICULTURE_TYPE> agrtyp = Db.STATIC_AGRICULTURE_TYPE.ToList();
            agrtyp.ForEach(x =>
            {
                agrType.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
            });
            entity.AgriType = agrType;

            List<STATIC_AGRICULTURE_ITEM> agritm = (from p in Db.STATIC_AGRICULTURE_ITEM
                                                    where p.TypeID == entity.TypeID
                                                    select p).ToList();
            agritm.ForEach(x =>
            {
                agrItem.Add(new SelectListItem { Text = x.ItemDescription, Value = x.ItemID.ToString() });
            });
            entity.AgriItem = agrItem;

            List<STATIC_HOUSEHOLD_MARITAL> aMarred = Db.STATIC_HOUSEHOLD_MARITAL.ToList();
            aMarred.ForEach(x =>
            {
                Maritals.Add(new SelectListItem { Text = x.MaritalType, Value = x.MaritalID.ToString() });
            });
            List<STATIC_SCHOOL_LEVEL> aAcad = Db.STATIC_SCHOOL_LEVEL.ToList();
            aAcad.ForEach(x =>
            {
                Academics.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
            });
            List<STATIC_FARMER_LAND_REGISTRATION> aOwner = Db.STATIC_FARMER_LAND_REGISTRATION.ToList();
            aOwner.ForEach(x =>
            {
                Ownereship.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
            });
            List<STATIC_HOUSEHOLD_GENDER> aGender = Db.STATIC_HOUSEHOLD_GENDER.ToList();
            aGender.ForEach(x =>
            {
                Genders.Add(new SelectListItem { Text = x.GenderType, Value = x.GenderID.ToString() });
            });
            entity.Academics = Academics;
            entity.Maritals = Maritals;
            entity.LandOwner = Ownereship;
            entity.Genders = Genders;

            return View(entity);
        }

        // POST: Farmer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FarmerViewModel2 mybiz)
        {
            ViewBag.Message = "";
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;
                //var query = (from p in Db.BaseTables
                //            select p).ToList();
                //var highestId = query.Any() ? query.Max(x => x.ID) : 0;

                var abizna = Db.BaseTables.Where(s => s.BaseID == id).FirstOrDefault();
                var mcreator = abizna.UserID;
                //Db.BaseTables.Remove(abizna);

                var abizd = Db.HOUSEHOLDS.Where(s => s.BaseID == id).FirstOrDefault();
                //Db.HOUSEHOLDS.Remove(abizd);

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                //Db.FARMERs.Remove(afamd);

                int maxValue = mybiz.BaseID;

                //maxValue += 1;
                if (abizna == null)
                {
                    Db.BaseTables.Add(new BaseTable()
                    {
                        BaseID = maxValue,
                        Latitude = bizna.Latitude,
                        Longitude = bizna.Longitude,
                        Pluscode = bizna.Pluscode,
                        Address = bizna.Address,
                        Category = "F",
                        UserID = mcreator
                    });
                }
                else
                {
                    abizna.Latitude = bizna.Latitude;
                    abizna.Longitude = bizna.Longitude;
                    abizna.Pluscode = bizna.Pluscode;
                    abizna.Address = bizna.Address;
                    abizna.Category = "F";
                    abizna.UserID = mcreator;
                }
                if (abizd == null)
                {
                    Db.HOUSEHOLDS.Add(new HOUSEHOLD()
                    {
                        BaseID = maxValue,
                        HouseHoldTypeId = bizna.HouseHoldTypeId,
                        HouseHoldName = bizna.HouseHoldName,
                        ContactPhone = bizna.ContactPhone,
                        ContactEmail = bizna.ContactEmail,
                        Website = bizna.Website,
                        Building = bizna.Building,
                        County_Code = bizna.County_Code,
                        Constituency_Code = bizna.Constituency_Code,
                        Sub_County_Code = bizna.Sub_County_Code,
                        Ward_Code = bizna.Ward_Code,
                        Village = bizna.Village,
                        LandRegistration = bizna.LandRegistration,
                        NoWorkers = bizna.NoWorkers,
                        ContactFirstname = bizna.ContactFirstname,
                        ContactSurname = bizna.ContactSurname,
                        ContactOthername = bizna.ContactOthername,
                        ContactGender = bizna.ContactGender,
                        MaritalStatus = bizna.MaritalStatus
                    });
                }
                else
                {
                    abizd.HouseHoldTypeId = bizna.HouseHoldTypeId;
                    abizd.HouseHoldName = bizna.HouseHoldName;
                    abizd.ContactPhone = bizna.ContactPhone;
                    abizd.ContactEmail = bizna.ContactEmail;
                    abizd.Website = bizna.Website;
                    abizd.Building = bizna.Building;
                    abizd.County_Code = bizna.County_Code;
                    abizd.Constituency_Code = bizna.Constituency_Code;
                    abizd.Sub_County_Code = bizna.Sub_County_Code;
                    abizd.Ward_Code = bizna.Ward_Code;
                    abizd.Village = bizna.Village;
                    abizd.LandRegistration = bizna.LandRegistration;
                    abizd.NoWorkers = bizna.NoWorkers;
                    abizd.ContactFirstname = bizna.ContactFirstname;
                    abizd.ContactSurname = bizna.ContactSurname;
                    abizd.ContactOthername = bizna.ContactOthername;
                    abizd.ContactGender = bizna.ContactGender;
                    abizd.MaritalStatus = bizna.MaritalStatus;
                }
                if (afamd == null)
                {
                    Db.FARMERs.Add(new FARMER()
                    {
                        BaseID = maxValue,
                        CategoryID = bizna.CategoryID,
                        TypeID = bizna.TypeID,
                        ItemID = bizna.ItemID,
                        SupportedBy = bizna.SupportedBy,
                        SupportSort = bizna.SupportSort,
                        Challenges = bizna.Challenges,
                        FormalTraining = bizna.FormalTraining,
                        FormalHighestEducation = bizna.FormalHighestEducation,
                        FarmerDateOfBirth = bizna.FarmerDateOfBirth
                    });
                }
                else
                {
                    afamd.CategoryID = bizna.CategoryID;
                    afamd.TypeID = bizna.TypeID;
                    afamd.ItemID = bizna.ItemID;
                    afamd.SupportedBy = bizna.SupportedBy;
                    afamd.SupportSort = bizna.SupportSort;
                    afamd.Challenges = bizna.Challenges;
                    afamd.FormalTraining = bizna.FormalTraining;
                    afamd.FormalHighestEducation = bizna.FormalHighestEducation;
                    afamd.FarmerDateOfBirth = bizna.FarmerDateOfBirth;
                }
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<SelectListItem> hseTypes = new List<SelectListItem>();
                List<SelectListItem> countyNames = new List<SelectListItem>();
                List<SelectListItem> scountyNames = new List<SelectListItem>();
                List<SelectListItem> consNames = new List<SelectListItem>();
                List<SelectListItem> wardNames = new List<SelectListItem>();
                List<SelectListItem> agrCategories = new List<SelectListItem>();
                List<SelectListItem> agrType = new List<SelectListItem>();
                List<SelectListItem> agrItem = new List<SelectListItem>();
                List<SelectListItem> Maritals = new List<SelectListItem>();
                List<SelectListItem> Academics = new List<SelectListItem>();
                List<SelectListItem> Ownereship = new List<SelectListItem>();
                List<SelectListItem> Genders = new List<SelectListItem>();

                List<STATIC_HOUSEHOLD_TYPE> ctypes = Db.STATIC_HOUSEHOLD_TYPE.ToList();
                ctypes.ForEach(x =>
                {
                    hseTypes.Add(new SelectListItem { Text = x.HouseHoldType, Value = x.HouseHoldTypeID.ToString() });
                });
                mybiz.HseTypes = hseTypes;

                List<COUNTY> cties = Db.COUNTies.ToList();
                cties.ForEach(x =>
                {
                    countyNames.Add(new SelectListItem { Text = x.County_Name, Value = x.County_Code.ToString() });
                });
                mybiz.CountyNames = countyNames;

                List<SUB_COUNTY> scties = Db.SUB_COUNTY.ToList();
                scties.ForEach(x =>
                {
                    scountyNames.Add(new SelectListItem { Text = x.Sub_County_Name, Value = x.Sub_County_Code.ToString() });
                });
                mybiz.SubCountyNames = scountyNames;

                List<CONSTITUENCY> cons = Db.CONSTITUENCies.ToList();
                cons.ForEach(x =>
                {
                    consNames.Add(new SelectListItem { Text = x.Constituency_Name, Value = x.Constituency_Code.ToString() });
                });
                mybiz.Constituencies = consNames;

                List<WARD> wads = Db.WARDs.ToList();
                wads.ForEach(x =>
                {
                    wardNames.Add(new SelectListItem { Text = x.Ward_Name, Value = x.Ward_Code.ToString() });
                });
                mybiz.Wards = wardNames;

                List<STATIC_AGRICULTURE_CATEGORY> agrcat = Db.STATIC_AGRICULTURE_CATEGORY.ToList();
                agrcat.ForEach(x =>
                {
                    agrCategories.Add(new SelectListItem { Text = x.Category, Value = x.CategoryID.ToString() });
                });
                mybiz.AgriCategory = agrCategories;

                List<STATIC_AGRICULTURE_TYPE> agrtyp = Db.STATIC_AGRICULTURE_TYPE.ToList();
                agrtyp.ForEach(x =>
                {
                    agrType.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
                });
                mybiz.AgriType = agrType;

                List<STATIC_AGRICULTURE_ITEM> agritm = (from p in Db.STATIC_AGRICULTURE_ITEM
                                                        where p.TypeID == mybiz.TypeID
                                                        select p).ToList();
                agritm.ForEach(x =>
                {
                    agrItem.Add(new SelectListItem { Text = x.ItemDescription, Value = x.ItemID.ToString() });
                });
                mybiz.AgriItem = agrItem;

                List<STATIC_HOUSEHOLD_MARITAL> aMarred = Db.STATIC_HOUSEHOLD_MARITAL.ToList();
                aMarred.ForEach(x =>
                {
                    Maritals.Add(new SelectListItem { Text = x.MaritalType, Value = x.MaritalID.ToString() });
                });
                List<STATIC_SCHOOL_LEVEL> aAcad = Db.STATIC_SCHOOL_LEVEL.ToList();
                aAcad.ForEach(x =>
                {
                    Academics.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
                });
                List<STATIC_FARMER_LAND_REGISTRATION> aOwner = Db.STATIC_FARMER_LAND_REGISTRATION.ToList();
                aOwner.ForEach(x =>
                {
                    Ownereship.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
                });
                List<STATIC_HOUSEHOLD_GENDER> aGender = Db.STATIC_HOUSEHOLD_GENDER.ToList();
                aGender.ForEach(x =>
                {
                    Genders.Add(new SelectListItem { Text = x.GenderType, Value = x.GenderID.ToString() });
                });
                mybiz.Academics = Academics;
                mybiz.Maritals = Maritals;
                mybiz.LandOwner = Ownereship;
                mybiz.Genders = Genders;

            }

            return View("Edit", mybiz);
        }

        // GET: Farmer/IFarmHold/5
        public ActionResult IFarmHold(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel3()
                          { // result selector 
                              BaseID = p.BaseID,
                              Group_CIG_VGM_PO = w.Group_CIG_VGM_PO,
                              Group_Name = w.Group_Name,
                              LegalStatusHolding = w.LegalStatusHolding,
                              LRNo = w.LRNo,
                              LandAcre = w.LandAcre,
                              OtherFarm = w.OtherFarm,

                              CropSubsistence = w.CropSubsistence,
                              CropSale = w.CropSale,
                              LivestockSubsistence = w.LivestockSubsistence,
                              LivestockSale = w.LivestockSale,
                              AquacultureSubsistence = w.AquacultureSubsistence,
                              AquacultureSale = w.AquacultureSale,
                              TreeFarming = w.TreeFarming
                          }).SingleOrDefault();

            List<SelectListItem> aGroups = new List<SelectListItem>();
            List<SelectListItem> aHoldings = new List<SelectListItem>();

            List<STATIC_FARMER_CATEGORYGROUPS> ctypes = Db.STATIC_FARMER_CATEGORYGROUPS.ToList();
            ctypes.ForEach(x =>
            {
                aGroups.Add(new SelectListItem { Text = x.GroupDesc, Value = x.GroupID.ToString() });
            });

            List<STATIC_FARMER_LEGAL_STATUS> chold = Db.STATIC_FARMER_LEGAL_STATUS.ToList();
            chold.ForEach(x =>
            {
                aHoldings.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
            });

            entity.Groups = aGroups;
            entity.Holdings = aHoldings;

            return View(entity);
        }

        // POST: Farmer/IFarmHold/5
        [HttpPost]
        public ActionResult IFarmHold(int id, FarmerViewModel3 mybiz)
        {
            ViewBag.Message = "";
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                
                if(afamd != null)
                {
                    afamd.Group_CIG_VGM_PO = bizna.Group_CIG_VGM_PO;
                    afamd.Group_Name = bizna.Group_Name;
                    afamd.LegalStatusHolding = bizna.LegalStatusHolding;
                    afamd.LRNo = bizna.LRNo;
                    afamd.LandAcre = bizna.LandAcre;
                    afamd.OtherFarm = bizna.OtherFarm;

                    afamd.CropSubsistence = bizna.CropSubsistence;
                    afamd.CropSale = bizna.CropSale;
                    afamd.LivestockSubsistence = bizna.LivestockSubsistence;
                    afamd.LivestockSale = bizna.LivestockSale;
                    afamd.AquacultureSubsistence = bizna.AquacultureSubsistence;
                    afamd.AquacultureSale = bizna.AquacultureSale;
                    afamd.TreeFarming = bizna.TreeFarming;
                };

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<SelectListItem> aGroups = new List<SelectListItem>();
                List<SelectListItem> aHoldings = new List<SelectListItem>();

                List<STATIC_FARMER_CATEGORYGROUPS> ctypes = Db.STATIC_FARMER_CATEGORYGROUPS.ToList();
                ctypes.ForEach(x =>
                {
                    aGroups.Add(new SelectListItem { Text = x.GroupDesc, Value = x.GroupID.ToString() });
                });

                List<STATIC_FARMER_LEGAL_STATUS> chold = Db.STATIC_FARMER_LEGAL_STATUS.ToList();
                chold.ForEach(x =>
                {
                    aHoldings.Add(new SelectListItem { Text = x.StatusDescription, Value = x.StatusID.ToString() });
                });

                mybiz.Groups = aGroups;
                mybiz.Holdings = aHoldings;

            }

            return View("IFarmHold", mybiz);
        }

        // GET: Farmer/Delete/5
        public ActionResult Delete(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join z in Db.FARMERs on p.BaseID equals z.BaseID
                          join w in Db.STATIC_HOUSEHOLD_TYPE on p.HouseHoldTypeId equals w.HouseHoldTypeID
                          join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                          join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                          join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                          join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                          join m in Db.STATIC_AGRICULTURE_CATEGORY on z.CategoryID equals m.CategoryID
                          join n in Db.STATIC_AGRICULTURE_TYPE on z.TypeID equals n.TypeID
                          join k in Db.STATIC_AGRICULTURE_ITEM on z.ItemID equals k.ItemID
                          from cty in ctydb.DefaultIfEmpty()
                          from scty in sctydb.DefaultIfEmpty()
                          from cons in consdb.DefaultIfEmpty()
                          from wds in wdsdb.DefaultIfEmpty()
                          where p.BaseID == id
                          select new FarmerViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              HouseHoldName = p.HouseHoldName,
                              HouseHoldTypeId = w.HouseHoldType,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              County_Code = cty.County_Name,
                              Constituency_Code = cons.Constituency_Name,
                              Sub_County_Code = scty.Sub_County_Name,
                              Ward_Code = wds.Ward_Name,
                              Village = p.Village,
                              LandRegistration = p.LandRegistration,
                              NoWorkers = (int)p.NoWorkers,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode,
                              Address = r.Address,
                              CategoryID = m.Category,
                              TypeID = n.TypeDescription,
                              ItemID = k.ItemDescription,
                              SupportedBy = z.SupportedBy,
                              SupportSort = z.SupportSort,
                              Challenges = z.Challenges
                          }).SingleOrDefault();

            return View(entity);
        }

        // POST: Farmer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (var ctx = new KEGooglePlusEntities())
                {
                    var abizna = ctx.BaseTables
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();

                    ctx.Entry(abizna).State = System.Data.Entity.EntityState.Deleted;

                    var abisd = ctx.HOUSEHOLDS
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();
                    ctx.Entry(abisd).State = System.Data.Entity.EntityState.Deleted;

                    var afamd = ctx.FARMERs
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();
                    ctx.Entry(afamd).State = System.Data.Entity.EntityState.Deleted;

                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Farmer/IFarmCrop/5
        public ActionResult IFarmCrop(int id)
        {

            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel4()
                          { // result selector 
                              BaseID = p.BaseID,
                              CropProductionSystem = w.CropProductionSystem,
                              CropAcreage = w.CropAcreage,
                              CropAverageAcreYield = w.CropAverageAcreYield,
                              DateCropdPlanted = w.DateCropdPlanted,
                              UseFertilizer = w.UseFertilizer,
                              FertilizerUse = w.FertilizerUse,
                              AverageFertilizerUse = w.AverageFertilizerUse,
                              FertilizerSourceDistince = w.FertilizerSourceDistince,
                              PlaceFertilizerSourced = w.PlaceFertilizerSourced,
                              SupportOtherCrops = w.SupportOtherCrops,
                              CropChemicalUse = w.CropChemicalUse,
                              VarietyOfCrop = w.VarietyOfCrop
                          }).SingleOrDefault();

            var mybiz = entity;

            List<vw_Farmer_Crops> aCrops = Db.vw_Farmer_Crops.Where(p=>p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.Crops = aCrops;
            
            List<SelectListItem> aProdSysList = new List<SelectListItem>();
            List<STATIC_FARMER_PRODUCTION_SYS> cProd = Db.STATIC_FARMER_PRODUCTION_SYS.ToList();
            cProd.ForEach(x =>
            {
                aProdSysList.Add(new SelectListItem { Text = x.ProductionDescription, Value = x.ProdID.ToString() });
            });
            if (mybiz.CropProductionSystem != null)
            {
                var prodstrings = mybiz.CropProductionSystem.Split(',');
                //mybiz.SelCropProdSystemList = prodstrings;
                List<string> CurProdSysList = prodstrings != null ? prodstrings.ToList() : null;
                if (CurProdSysList != null)
                {
                    List<SelectListItem> selectedItems = aProdSysList.Where(p => CurProdSysList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.CropProdSystemList = aProdSysList;

            List<SelectListItem> AchemUseList = new List<SelectListItem>();
            List<STATIC_FARMER_CROP_CHEMICAL_USE> cChemUse = Db.STATIC_FARMER_CROP_CHEMICAL_USE.ToList();
            cChemUse.ForEach(x =>
            {
                AchemUseList.Add(new SelectListItem { Text = x.ChemDescription, Value = x.ChemID.ToString() });
            });
            if (mybiz.CropChemicalUse != null)
            {
                var chemstrings = mybiz.CropChemicalUse.Split(',');
                List<string> CurChemicalUseList = chemstrings != null ? chemstrings.ToList() : null; 
                //List<string> CurChemicalUseList = chemstrings.ToList();
                if (CurChemicalUseList != null)
                {
                    List<SelectListItem> selectedItems = AchemUseList.Where(p => CurChemicalUseList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.CropChemicalUseList = AchemUseList;

            List<SelectListItem> AFertilizerList = new List<SelectListItem>();
            List<STATIC_FARMER_FERTILIZER> cFertilizer = Db.STATIC_FARMER_FERTILIZER.ToList();
            cFertilizer.ForEach(x =>
            {
                AFertilizerList.Add(new SelectListItem { Text = x.FertilizerDesc, Value = x.FertilizerID.ToString() });
            });
            if (mybiz.FertilizerUse != null)
            {
                var ferstrings = mybiz.FertilizerUse.Split(',');
                List<string> CurFertilizerList = ferstrings != null ? ferstrings.ToList() : null;
                //List<string> CurFertilizerList = ferstrings.ToList();
                if (CurFertilizerList != null)
                {
                    List<SelectListItem> selectedItems = AFertilizerList.Where(p => CurFertilizerList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FertilizerList = AFertilizerList;
            return View(mybiz);
        }

        // POST: Farmer/IFarmCrop/5
        [HttpPost]
        public ActionResult IFarmCrop(int id, FarmerViewModel4 mybiz)
        {
            ViewBag.Message = "";
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                if(mybiz.SelCropProdSystemList != null)
                {
                    string mitemdsc = string.Join(",", bizna.SelCropProdSystemList);
                    bizna.CropProductionSystem = mitemdsc;
                }
                if (mybiz.SelFertilizerList != null)
                {
                    string mitemdsc = string.Join(",", bizna.SelFertilizerList);
                    bizna.FertilizerUse = mitemdsc;
                }
                if (mybiz.SelCropChemicalUseList != null)
                {
                    string mitemdsc = string.Join(",", bizna.SelCropChemicalUseList);
                    bizna.CropChemicalUse = mitemdsc;
                }
                if (afamd != null)
                {
                    afamd.CropProductionSystem = bizna.CropProductionSystem;
                    afamd.CropAcreage = bizna.CropAcreage;
                    afamd.CropAverageAcreYield = bizna.CropAverageAcreYield;
                    afamd.DateCropdPlanted = bizna.DateCropdPlanted;
                    afamd.UseFertilizer = bizna.UseFertilizer;
                    afamd.FertilizerUse = bizna.FertilizerUse;
                    afamd.AverageFertilizerUse = bizna.AverageFertilizerUse;
                    afamd.FertilizerSourceDistince = bizna.FertilizerSourceDistince;
                    afamd.PlaceFertilizerSourced = bizna.PlaceFertilizerSourced;
                    afamd.SupportOtherCrops = bizna.SupportOtherCrops;
                    afamd.CropChemicalUse = bizna.CropChemicalUse;
                    afamd.VarietyOfCrop = bizna.VarietyOfCrop;
                };

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<vw_Farmer_Crops> aCrops = Db.vw_Farmer_Crops.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
                mybiz.Crops = aCrops;

                List<SelectListItem> aProdSysList = new List<SelectListItem>();
                List<STATIC_FARMER_PRODUCTION_SYS> cProd = Db.STATIC_FARMER_PRODUCTION_SYS.ToList();
                cProd.ForEach(x =>
                {
                    aProdSysList.Add(new SelectListItem { Text = x.ProductionDescription, Value = x.ProdID.ToString() });
                });
                if (mybiz.CropProductionSystem != null)
                {
                    var prodstrings = mybiz.CropProductionSystem.Split(',');
                    //mybiz.SelCropProdSystemList = prodstrings;
                    List<string> CurProdSysList = prodstrings != null ? prodstrings.ToList() : null;
                    if (CurProdSysList != null)
                    {
                        List<SelectListItem> selectedItems = aProdSysList.Where(p => CurProdSysList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                    mybiz.SelCropProdSystemList = prodstrings;
                }
                mybiz.CropProdSystemList = aProdSysList;

                List<SelectListItem> AchemUseList = new List<SelectListItem>();
                List<STATIC_FARMER_CROP_CHEMICAL_USE> cChemUse = Db.STATIC_FARMER_CROP_CHEMICAL_USE.ToList();
                cChemUse.ForEach(x =>
                {
                    AchemUseList.Add(new SelectListItem { Text = x.ChemDescription, Value = x.ChemID.ToString() });
                });
                if (mybiz.CropChemicalUse != null)
                {
                    var chemstrings = mybiz.CropChemicalUse.Split(',');
                    List<string> CurChemicalUseList = chemstrings != null ? chemstrings.ToList() : null;
                    //List<string> CurChemicalUseList = chemstrings.ToList();
                    if (CurChemicalUseList != null)
                    {
                        List<SelectListItem> selectedItems = AchemUseList.Where(p => CurChemicalUseList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                    mybiz.SelCropChemicalUseList = chemstrings;
                }
                mybiz.CropChemicalUseList = AchemUseList;

                List<SelectListItem> AFertilizerList = new List<SelectListItem>();
                List<STATIC_FARMER_FERTILIZER> cFertilizer = Db.STATIC_FARMER_FERTILIZER.ToList();
                cFertilizer.ForEach(x =>
                {
                    AFertilizerList.Add(new SelectListItem { Text = x.FertilizerDesc, Value = x.FertilizerID.ToString() });
                });
                if (mybiz.FertilizerUse != null)
                {
                    var ferstrings = mybiz.FertilizerUse.Split(',');
                    List<string> CurFertilizerList = ferstrings != null ? ferstrings.ToList() : null;
                    //List<string> CurFertilizerList = ferstrings.ToList();
                    if (CurFertilizerList != null)
                    {
                        List<SelectListItem> selectedItems = AFertilizerList.Where(p => CurFertilizerList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                    mybiz.SelFertilizerList = ferstrings;
                }
                mybiz.FertilizerList = AFertilizerList;
            }

            return View("IFarmCrop", mybiz);
        }

        public ActionResult GetCropId(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var acrop = Db.vw_Farmer_Crops.Where(x => x.CropID == id && x.BaseID == (int)Session["id"]); ;
            FarmerCropViewModel model = new FarmerCropViewModel();

            if (acrop != null)
            {

                foreach (var item in acrop)
                {
                    model.BaseID = item.BaseID;
                    model.CropID = item.CropID;
                    model.Acreage = item.Acreage;
                    model.Purpose = item.Purpose;
                    model.WaterSourceID = item.WaterSourceID;
                    model.Seeds = item.Seeds;
                }

            }
            else
            {
                model.BaseID = (int)Session["id"];
                model.CropID = id;
            }

            List<SelectListItem> aCropList = new List<SelectListItem>();
            List<SelectListItem> aWaterSourceList = new List<SelectListItem>();

            List<STATIC_CROPS> cCrops = Db.STATIC_CROPS.ToList();
            cCrops.ForEach(x =>
            {
                aCropList.Add(new SelectListItem { Text = x.CropDescription, Value = x.CropID.ToString() });
            });
            List<STATIC_FARMER_WATERSOURCE> cWaters = Db.STATIC_FARMER_WATERSOURCE.ToList();
            cWaters.ForEach(x =>
            {
                aWaterSourceList.Add(new SelectListItem { Text = x.WaterSource, Value = x.WaterSourceID.ToString() });
            });

            model.Crops = aCropList;

            return PartialView("GetCropId", model);
        }

        public ActionResult AddCrop(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerCropViewModel model = new FarmerCropViewModel();
            if (id != 0)
            {
                var acrop = Db.vw_Farmer_Crops.Where(x => x.CropID == id && x.BaseID == baseid).DefaultIfEmpty();
                if (acrop != null)
                {

                    foreach (var item in acrop)
                    {
                        model.BaseID = item.BaseID;
                        model.CropID = item.CropID;
                        model.Acreage = item.Acreage;
                        model.Purpose = item.Purpose;
                        model.WaterSourceID = item.WaterSourceID;
                        model.Seeds = item.Seeds;
                    }

                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }

            List<SelectListItem> aCropList = new List<SelectListItem>();
            List<SelectListItem> aWaterSourceList = new List<SelectListItem>();

            List<STATIC_CROPS> cCrops = Db.STATIC_CROPS.ToList();
            cCrops.ForEach(x =>
            {
                aCropList.Add(new SelectListItem { Text = x.CropDescription, Value = x.CropID.ToString() });
            });
            List<STATIC_FARMER_WATERSOURCE> cWaters = Db.STATIC_FARMER_WATERSOURCE.ToList();
            cWaters.ForEach(x =>
            {
                aWaterSourceList.Add(new SelectListItem { Text = x.WaterSource, Value = x.WaterSourceID.ToString() });
            });

            List<vw_Farmer_Crops> aCrops = Db.vw_Farmer_Crops.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();
            
            model.CropsList = aCrops;
            model.Crops = aCropList;
            model.WaterSource = aWaterSourceList;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCrop(FarmerCropViewModel model)
        {
            var basedid = model.BaseID;
            FarmerCropViewModel model2 = new FarmerCropViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_CROPS.Where(s => s.BaseID == model.BaseID && s.CropID == model.CropID).FirstOrDefault();

                if (afamd != null)
                {
                    afamd.Acreage = model.Acreage;
                    afamd.Purpose = model.Purpose;
                    afamd.WaterSourceID = model.WaterSourceID;
                    afamd.Seeds = model.Seeds;
                }
                else
                {
                    //Add new record into Database
                    Db.FARMER_CROPS.Add(new FARMER_CROPS()
                    {
                        BaseID = model.BaseID,
                        CropID = model.CropID,
                        Acreage = model.Acreage,
                        Purpose = model.Purpose,
                        WaterSourceID = model.WaterSourceID,
                        Seeds = model.Seeds
                    });
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return View("AddCrop", "Farmer", new { id = 0});
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                model2.BaseID = basedid;

                List<SelectListItem> aCropList = new List<SelectListItem>();
                List<SelectListItem> aWaterSourceList = new List<SelectListItem>();

                List<STATIC_CROPS> cCrops = Db.STATIC_CROPS.ToList();
                cCrops.ForEach(x =>
                {
                    aCropList.Add(new SelectListItem { Text = x.CropDescription, Value = x.CropID.ToString() });
                });
                List<STATIC_FARMER_WATERSOURCE> cWaters = Db.STATIC_FARMER_WATERSOURCE.ToList();
                cWaters.ForEach(x =>
                {
                    aWaterSourceList.Add(new SelectListItem { Text = x.WaterSource, Value = x.WaterSourceID.ToString() });
                });

                List<vw_Farmer_Crops> aCrops = Db.vw_Farmer_Crops.Where(p=> p.BaseID == basedid).ToList();
                model2.CropsList = aCrops;
                model2.Crops = aCropList;
                model2.WaterSource = aWaterSourceList;
            }
            return View(model2);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelCrop(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_CROPS.Where(s => s.BaseID == baseid && s.CropID == id).FirstOrDefault();

                Db.FARMER_CROPS.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        // GET: Farmer/IFarmLivestock/5
        public ActionResult IFarmLivestock(int id)
        {

            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel5()
                          { // result selector 
                              BaseID = p.BaseID,
                              main_livestock_feed = w.LivestockFeeds
                          }).SingleOrDefault();

            var mybiz = entity;

            List<vw_Livestock_Stock> aLivestock = Db.vw_Livestock_Stock.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.LivestockList = aLivestock;
            List<vw_Livestock_Feeds> aLivestockFds = Db.vw_Livestock_Feeds.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.LivestockFeedsList = aLivestockFds;

            List<SelectListItem> aFeedsList = new List<SelectListItem>();
            List<STATIC_FARMER_FEEDS> cFeeds = Db.STATIC_FARMER_FEEDS.ToList();
            cFeeds.ForEach(x =>
            {
                aFeedsList.Add(new SelectListItem { Text = x.FeedDescription, Value = x.FeedID.ToString() });
            });
            if (mybiz.main_livestock_feed != null)
            {
                var fdstrings = mybiz.main_livestock_feed.Split(',');
                //mybiz.SelCropProdSystemList = prodstrings;
                List<string> CurFeedsList = fdstrings != null ? fdstrings.ToList() : null;
                if (CurFeedsList != null)
                {
                    List<SelectListItem> selectedItems = aFeedsList.Where(p => CurFeedsList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.main_livestock_feedlist = aFeedsList;

            return View(mybiz);
        }


        // POST: Farmer/IFarmCrop/5
        [HttpPost]
        public ActionResult IFarmLivestock(int id, FarmerViewModel5 mybiz)
        {
            ViewBag.Message = "";
            FarmerViewModel5 mybiz2 = new FarmerViewModel5();
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                if (mybiz.SelLivestockList != null)
                {
                    string mitemdsc = string.Join(",", bizna.SelLivestockList);
                    bizna.main_livestock_feed = mitemdsc;
                }
                
                if (afamd != null)
                {
                    afamd.LivestockFeeds = bizna.main_livestock_feed;
                };

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                
                List<vw_Livestock_Stock> aLivestock = Db.vw_Livestock_Stock.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
                mybiz2.LivestockList = aLivestock;

                List<SelectListItem> aFeedsList = new List<SelectListItem>();
                List<STATIC_FARMER_FEEDS> cFeeds = Db.STATIC_FARMER_FEEDS.ToList();
                cFeeds.ForEach(x =>
                {
                    aFeedsList.Add(new SelectListItem { Text = x.FeedDescription, Value = x.FeedID.ToString() });
                });
                if (mybiz.main_livestock_feed != null)
                {
                    var fdstrings = mybiz.main_livestock_feed.Split(',');
                    //mybiz.SelCropProdSystemList = prodstrings;
                    List<string> CurFeedsList = fdstrings != null ? fdstrings.ToList() : null;
                    if (CurFeedsList != null)
                    {
                        List<SelectListItem> selectedItems = aFeedsList.Where(p => CurFeedsList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.main_livestock_feedlist = aFeedsList;
            }

            return View("IFarmLivestock", mybiz2);
        }

        //Get a Livestock Record to a a farmer
        public ActionResult AddLivestock(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerLivestockViewModel model = new FarmerLivestockViewModel();
            if (id != 0)
            {
                var alivestock = Db.vw_Livestock_Stock.Where(x => x.LivestockID == id && x.BaseID == baseid).DefaultIfEmpty();
                if (alivestock != null)
                {
                    foreach (var item in alivestock)
                    {
                        model.BaseID = item.BaseID;
                        model.LivestockID = item.LivestockID;
                        model.ProductionID = item.ProductionID;
                        model.Stock = item.Stock;
                    }
                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }

            List<SelectListItem> aLivestockList = new List<SelectListItem>();
            List<SelectListItem> aProdList = new List<SelectListItem>();

            List<STATIC_FARMER_LIVESTOCK_SPECIES> cSpecies = Db.STATIC_FARMER_LIVESTOCK_SPECIES.ToList();
            cSpecies.ForEach(x =>
            {
                aLivestockList.Add(new SelectListItem { Text = x.Livestock, Value = x.LivestockID.ToString() });
            });

            List<STATIC_FARMER_LIVESTOCK_PRODSYS> cProd = Db.STATIC_FARMER_LIVESTOCK_PRODSYS.ToList();
            cProd.ForEach(x =>
            {
                aProdList.Add(new SelectListItem { Text = x.ProductionSystem, Value = x.ProductionID.ToString() });
            });

            List<vw_Livestock_Stock> aLivestocks = Db.vw_Livestock_Stock.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();

            model.Livestocklist = aLivestockList;
            model.ProductionSysList = aProdList;
            model.FarmerLivestockList = aLivestocks;

            return View(model);
        }

        //Save a Livestock Record to a a farmer
        [HttpPost]
        public ActionResult AddLivestock(FarmerLivestockViewModel model)
        {
            var basedid = model.BaseID;
            FarmerLivestockViewModel model2 = new FarmerLivestockViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_LIVESTOCK.Where(s => s.BaseID == model.BaseID && s.LivestockID == model.LivestockID).FirstOrDefault();

                if (afamd != null)
                {
                    afamd.ProductionID = model.ProductionID;
                    afamd.Stock = model.Stock;
                }
                else
                {
                    //Add new record into Database
                    Db.FARMER_LIVESTOCK.Add(new FARMER_LIVESTOCK()
                    {
                        BaseID = model.BaseID,
                        LivestockID = model.LivestockID,
                        ProductionID = model.ProductionID,
                        Stock = model.Stock
                    });
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return View("AddCrop", "Farmer", new { id = 0});
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                model2.BaseID = basedid;
                List<SelectListItem> aLivestockList = new List<SelectListItem>();
                List<SelectListItem> aProdList = new List<SelectListItem>();

                List<STATIC_FARMER_LIVESTOCK_SPECIES> cSpecies = Db.STATIC_FARMER_LIVESTOCK_SPECIES.ToList();
                cSpecies.ForEach(x =>
                {
                    aLivestockList.Add(new SelectListItem { Text = x.Livestock, Value = x.LivestockID.ToString() });
                });

                List<STATIC_FARMER_LIVESTOCK_PRODSYS> cProd = Db.STATIC_FARMER_LIVESTOCK_PRODSYS.ToList();
                cProd.ForEach(x =>
                {
                    aProdList.Add(new SelectListItem { Text = x.ProductionSystem, Value = x.ProductionID.ToString() });
                });

                List<vw_Livestock_Stock> aLivestocks = Db.vw_Livestock_Stock.Where(p => p.BaseID == basedid).DefaultIfEmpty().ToList();

                model2.Livestocklist = aLivestockList;
                model2.ProductionSysList = aProdList;
                model2.FarmerLivestockList = aLivestocks;
            }
            return View(model2);
        }
        //Delete a Lvestock Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelLivestock(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_LIVESTOCK.Where(s => s.BaseID == baseid && s.LivestockID == id).FirstOrDefault();

                Db.FARMER_LIVESTOCK.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        //Get a Livestock Feed Record to a a farmer
        public ActionResult AddLivestockFeed(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerLivestockFeedViewModel model = new FarmerLivestockFeedViewModel();
            if (id != 0)
            {
                var aFeed = Db.vw_Livestock_Feeds.Where(x => x.FeedID == id && x.BaseID == baseid).DefaultIfEmpty();
                if (aFeed != null)
                {
                    foreach (var item in aFeed)
                    {
                        model.BaseID = item.BaseID;
                        model.FeedID = item.FeedID;
                        model.Quantity = item.Quantity;
                    }
                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }

            List<SelectListItem> aFeedList = new List<SelectListItem>();

            List<STATIC_FARMER_FEEDS> cFeeds = Db.STATIC_FARMER_FEEDS.ToList();
            cFeeds.ForEach(x =>
            {
                aFeedList.Add(new SelectListItem { Text = x.FeedDescription, Value = x.FeedID.ToString() });
            });
            
            List<vw_Livestock_Feeds> aLivestockFeeds = Db.vw_Livestock_Feeds.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();

            model.Feedslist = aFeedList;
            model.FarmerFeedsList = aLivestockFeeds;

            return View(model);
        }

        //Save a Livestock Feed Record to a a farmer
        [HttpPost]
        public ActionResult AddLivestockFeed(FarmerLivestockFeedViewModel model)
        {
            var basedid = (int)Session["id"];
            FarmerLivestockFeedViewModel model2 = new FarmerLivestockFeedViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                Db.proc_Ins_Livestock_Feeds(model.BaseID, model.FeedID, model.Quantity);

                //var afamd = Db.FARMER_ANIMAL_FEEDS.Where(s => s.BaseID == model.BaseID && s.FeedID == model.FeedID).FirstOrDefault();

                //if (afamd != null)
                //{
                //    afamd.Quantity = model.Quantity;
                //    Db.SaveChanges();
                //}
                //else
                //{
                //    //Add new record into Database
                //    Db.FARMER_ANIMAL_FEEDS.Add(new FARMER_ANIMAL_FEEDS()
                //    {
                //        BaseID = basedid,
                //        FeedID = model.FeedID,
                //        Quantity = model.Quantity
                //    });
                //    Db.SaveChanges();
                //}

                ViewBag.Message = String.Format("Record Successfully Saved!!! {0} - {1} - {2}",model.BaseID, model.FeedID, model.Quantity);
                
                model2.BaseID = basedid;
                List<SelectListItem> aFeedList = new List<SelectListItem>();

                List<STATIC_FARMER_FEEDS> cSpecies = Db.STATIC_FARMER_FEEDS.ToList();
                cSpecies.ForEach(x =>
                {
                    aFeedList.Add(new SelectListItem { Text = x.FeedDescription, Value = x.FeedID.ToString() });
                });


                List<vw_Livestock_Feeds> aFarmerFeeds = Db.vw_Livestock_Feeds.Where(p => p.BaseID == basedid).DefaultIfEmpty().ToList();

                model2.Feedslist = aFeedList;
                model2.FarmerFeedsList = aFarmerFeeds;
            }
            return View(model2);
        }
       
        //Delete a Lvestock Feed Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelFeed(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_ANIMAL_FEEDS.Where(s => s.BaseID == baseid && s.FeedID == id).FirstOrDefault();

                Db.FARMER_ANIMAL_FEEDS.Remove(afamd);
                Db.SaveChanges();

                Db.proc_Del_Livestock_Feeds(baseid, id, 0);

                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult DelLivestockFeed(int id)
        {
            //update database
            var bsid = (int)Session["id"];
            ViewBag.Message2 = String.Format("{0} - {1}", bsid, id);

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = bsid;
                var afamd = Db.FARMER_ANIMAL_FEEDS.Where(s => s.BaseID == baseid && s.FeedID == id).FirstOrDefault();
                
                Db.proc_Del_Livestock_Feeds(baseid, id, 0);

                ViewBag.Message = "Record Successfully Deleted!!!";

                return RedirectToAction("AddLivestockFeed", "Farmer", new { id = 0 });
            }

        }

        //Aquaculture Methods
        // GET: Farmer/IFarmAquaculture/5
        public ActionResult IFarmAquaculture(int id)
        {

            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel6()
                          { // result selector 
                              BaseID = p.BaseID,
                              AquacultureType = w.AquacultureType,
                              AquacultureInputs = w.AquacultureInputs,
                              AquacultureLevel = w.AquacultureLevel,
                              FertilizerPonds = w.FertilizerPonds
                          }).SingleOrDefault();

            var mybiz = entity;
            //Captured Records for the Farmer ready for display
            List<vw_Aquaculture_ProductionSystem> aAquaProdSysList = Db.vw_Aquaculture_ProductionSystem.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.FarmerAquaProdSysList = aAquaProdSysList;
            List<vw_Aquaculture_Species> aAquaSpeciesList = Db.vw_Aquaculture_Species.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.FarmerAquaSpeciesList = aAquaSpeciesList;

            //Types
            List<SelectListItem> aQuaTypeList = new List<SelectListItem>();
            List<STATIC_FARMER_AQUACULTURE_TYPES> cTypes = Db.STATIC_FARMER_AQUACULTURE_TYPES.ToList();
            cTypes.ForEach(x =>
            {
                aQuaTypeList.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
            });
            if (mybiz.AquacultureType != null)
            {
                var aqtypestrings = mybiz.AquacultureType.Split(',');
                List<string> CurAquTypesList = aqtypestrings != null ? aqtypestrings.ToList() : null;
                if (CurAquTypesList != null)
                {
                    List<SelectListItem> selectedItems = aQuaTypeList.Where(p => CurAquTypesList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.AquacultureTypeList = aQuaTypeList;
            //Inputs
            List<SelectListItem> aQuaInputsList = new List<SelectListItem>();
            List<STATIC_FARMER_AQUACULTURE_INPUTS> cInputs = Db.STATIC_FARMER_AQUACULTURE_INPUTS.ToList();
            cInputs.ForEach(x =>
            {
                aQuaInputsList.Add(new SelectListItem { Text = x.InputDescription, Value = x.InputID.ToString() });
            });
            if (mybiz.AquacultureInputs != null)
            {
                var aqinputstrings = mybiz.AquacultureInputs.Split(',');
                List<string> CurAquInputsList = aqinputstrings != null ? aqinputstrings.ToList() : null;
                if (CurAquInputsList != null)
                {
                    List<SelectListItem> selectedItems = aQuaInputsList.Where(p => CurAquInputsList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.AquacultureInputsList = aQuaInputsList;
            //Levels
            List<SelectListItem> aQuaLevelsList = new List<SelectListItem>();
            List<STATIC_FARMER_AQUACULTURE_LEVELS> cLevels = Db.STATIC_FARMER_AQUACULTURE_LEVELS.ToList();
            cLevels.ForEach(x =>
            {
                aQuaLevelsList.Add(new SelectListItem { Text = x.LevelDescription, Value = x.LevelID.ToString() });
            });
            if (mybiz.AquacultureLevel != null)
            {
                var aqlevelstrings = mybiz.AquacultureLevel.Split(',');
                List<string> CurAquLevelsList = aqlevelstrings != null ? aqlevelstrings.ToList() : null;
                if (CurAquLevelsList != null)
                {
                    List<SelectListItem> selectedItems = aQuaLevelsList.Where(p => CurAquLevelsList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.AquacultureLevelList = aQuaLevelsList;
            
            return View(mybiz);
        }


        // POST: Farmer/IFarmAquaculture/5
        [HttpPost]
        public ActionResult IFarmAquaculture(int id, FarmerViewModel6 mybiz)
        {
            ViewBag.Message = "";
            FarmerViewModel6 mybiz2 = new FarmerViewModel6();
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                if (mybiz.SelAquacultureTypeList != null)
                {
                    string mtypedsc = string.Join(",", bizna.SelAquacultureTypeList);
                    bizna.AquacultureType = mtypedsc;
                }
                if (mybiz.SelAquacultureLevelList != null)
                {
                    string mleveldsc = string.Join(",", bizna.SelAquacultureLevelList);
                    bizna.AquacultureLevel = mleveldsc;
                }
                if (mybiz.SelAquacultureInputsList != null)
                {
                    string minputdsc = string.Join(",", bizna.SelAquacultureInputsList);
                    bizna.AquacultureInputs = minputdsc;
                }

                if (afamd != null)
                {
                    afamd.AquacultureInputs = bizna.AquacultureInputs;
                    afamd.AquacultureLevel = bizna.AquacultureLevel;
                    afamd.AquacultureType = bizna.AquacultureType;
                    afamd.FertilizerPonds = bizna.FertilizerPonds;
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                //var mybiz = mybiz2;
                //Captured Records for the Farmer ready for display
                List<vw_Aquaculture_ProductionSystem> aAquaProdSysList = Db.vw_Aquaculture_ProductionSystem.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
                mybiz.FarmerAquaProdSysList = aAquaProdSysList;
                List<vw_Aquaculture_Species> aAquaSpeciesList = Db.vw_Aquaculture_Species.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
                mybiz.FarmerAquaSpeciesList = aAquaSpeciesList;

                //Types
                List<SelectListItem> aQuaTypeList = new List<SelectListItem>();
                List<STATIC_FARMER_AQUACULTURE_TYPES> cTypes = Db.STATIC_FARMER_AQUACULTURE_TYPES.ToList();
                cTypes.ForEach(x =>
                {
                    aQuaTypeList.Add(new SelectListItem { Text = x.TypeDescription, Value = x.TypeID.ToString() });
                });
                if (mybiz.AquacultureType != null)
                {
                    var aqtypestrings = mybiz.AquacultureType.Split(',');
                    List<string> CurAquTypesList = aqtypestrings != null ? aqtypestrings.ToList() : null;
                    if (CurAquTypesList != null)
                    {
                        List<SelectListItem> selectedItems = aQuaTypeList.Where(p => CurAquTypesList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }

                mybiz.AquacultureTypeList = aQuaTypeList;
                //Inputs
                List<SelectListItem> aQuaInputsList = new List<SelectListItem>();
                List<STATIC_FARMER_AQUACULTURE_INPUTS> cInputs = Db.STATIC_FARMER_AQUACULTURE_INPUTS.ToList();
                cInputs.ForEach(x =>
                {
                    aQuaInputsList.Add(new SelectListItem { Text = x.InputDescription, Value = x.InputID.ToString() });
                });
                if (mybiz.AquacultureInputs != null)
                {
                    var aqinputstrings = mybiz.AquacultureInputs.Split(',');
                    List<string> CurAquInputsList = aqinputstrings != null ? aqinputstrings.ToList() : null;
                    if (CurAquInputsList != null)
                    {
                        List<SelectListItem> selectedItems = aQuaInputsList.Where(p => CurAquInputsList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz.AquacultureInputsList = aQuaInputsList;
                //Levels
                List<SelectListItem> aQuaLevelsList = new List<SelectListItem>();
                List<STATIC_FARMER_AQUACULTURE_LEVELS> cLevels = Db.STATIC_FARMER_AQUACULTURE_LEVELS.ToList();
                cLevels.ForEach(x =>
                {
                    aQuaLevelsList.Add(new SelectListItem { Text = x.LevelDescription, Value = x.LevelID.ToString() });
                });
                if (mybiz.AquacultureLevel != null)
                {
                    var aqlevelstrings = mybiz.AquacultureLevel.Split(',');
                    List<string> CurAquLevelsList = aqlevelstrings != null ? aqlevelstrings.ToList() : null;
                    if (CurAquLevelsList != null)
                    {
                        List<SelectListItem> selectedItems = aQuaLevelsList.Where(p => CurAquLevelsList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz.AquacultureLevelList = aQuaLevelsList;

            }

            return View("IFarmAquaculture", mybiz);
        }

        //Get a Aquacuture Production System Record to a a farmer for View/Edits
        public ActionResult AddAquacultureProd(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerAquacultureProdViewModel model = new FarmerAquacultureProdViewModel();
            if (id != 0)
            {
                var apodsys = Db.vw_Aquaculture_ProductionSystem.Where(x => x.ProductID == id && x.BaseID == baseid).DefaultIfEmpty();
                if (apodsys != null)
                {
                    foreach (var item in apodsys)
                    {
                        model.BaseID = item.BaseID;
                        model.ProductID = item.ProductID;
                        model.ActiveUnits = item.ActiveUnits;
                        model.Area_Volume = item.Area_Volume;
                        model.InactiveUnits = item.InactiveUnits;
                        model.InAcArea_Volume = item.InAcArea_Volume;
                    }
                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }
            
            List<SelectListItem> aProdList = new List<SelectListItem>();

            List<STATIC_FARMER_AQUACULTURE_PRODSYS> cSpecies = Db.STATIC_FARMER_AQUACULTURE_PRODSYS.ToList();
            cSpecies.ForEach(x =>
            {
                aProdList.Add(new SelectListItem { Text = x.ProductSystem, Value = x.ProductID.ToString() });
            });
            
            List<vw_Aquaculture_ProductionSystem> aProdSysList = Db.vw_Aquaculture_ProductionSystem.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();

            model.ProdSyslist = aProdList;

            model.FarmerAquaProdSyslist = aProdSysList;

            return View(model);
        }

        //Save a Livestock Record to a a farmer
        [HttpPost]
        public ActionResult AddAquacultureProd(FarmerAquacultureProdViewModel model)
        {
            var basedid = model.BaseID;
            FarmerAquacultureProdViewModel model2 = new FarmerAquacultureProdViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_AQUACULTURE_PRODSYS.Where(s => s.BaseID == model.BaseID && s.ProductID == model.ProductID).FirstOrDefault();

                if (afamd != null)
                {
                    afamd.Area_Volume = model.Area_Volume;
                    afamd.ActiveUnits = model.ActiveUnits;
                    afamd.InAcArea_Volume = model.InAcArea_Volume;
                    afamd.InactiveUnits = model.InactiveUnits;
                }
                else
                {
                    //Add new record into Database
                    Db.FARMER_AQUACULTURE_PRODSYS.Add(new FARMER_AQUACULTURE_PRODSYS()
                    {
                        BaseID = model.BaseID,
                        ProductID = model.ProductID,
                        ActiveUnits = model.ActiveUnits,
                        Area_Volume = model.Area_Volume,
                        InactiveUnits = model.InactiveUnits,
                        InAcArea_Volume = model.InAcArea_Volume
                    });
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return View("AddCrop", "Farmer", new { id = 0});
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                model2.BaseID = basedid;
                List<SelectListItem> aProdList = new List<SelectListItem>();

                List<STATIC_FARMER_AQUACULTURE_PRODSYS> cSpecies = Db.STATIC_FARMER_AQUACULTURE_PRODSYS.ToList();
                cSpecies.ForEach(x =>
                {
                    aProdList.Add(new SelectListItem { Text = x.ProductSystem, Value = x.ProductID.ToString() });
                });

                List<vw_Aquaculture_ProductionSystem> aProdSysList = Db.vw_Aquaculture_ProductionSystem.Where(p => p.BaseID == basedid).DefaultIfEmpty().ToList();

                model2.ProdSyslist = aProdList;

                model2.FarmerAquaProdSyslist = aProdSysList;
            }
            return View(model2);
        }

        //Delete a Lvestock Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelAquacultureProd(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_AQUACULTURE_PRODSYS.Where(s => s.BaseID == baseid && s.ProductID == id).FirstOrDefault();

                Db.FARMER_AQUACULTURE_PRODSYS.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        //Get a Aquacuture Species Record to a a farmer for View/Edits
        public ActionResult AddAquacultureSpecies(int id, int pid)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerAquacultureSpeciesViewModel model = new FarmerAquacultureSpeciesViewModel();
            if (id != 0)
            {
                var apodsys = Db.vw_Aquaculture_Species.Where(x => x.SpeciesID == id && x.ProductID == pid && x.BaseID == baseid).DefaultIfEmpty();
                if (apodsys != null)
                {
                    foreach (var item in apodsys)
                    {
                        model.BaseID = item.BaseID;
                        model.ProductID = item.ProductID;
                        model.SpeciesID = item.SpeciesID;
                        model.Fingerlings = item.Fingerlings;
                    }
                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }

            List<SelectListItem> aProdList = new List<SelectListItem>();

            List<STATIC_FARMER_AQUACULTURE_PRODSYS> cProds = Db.STATIC_FARMER_AQUACULTURE_PRODSYS.ToList();
            cProds.ForEach(x =>
            {
                aProdList.Add(new SelectListItem { Text = x.ProductSystem, Value = x.ProductID.ToString() });
            });

            List<SelectListItem> aSpecList = new List<SelectListItem>();

            List<STATIC_FARMER_AQUACULTURE_SPECIES> cSpecies = Db.STATIC_FARMER_AQUACULTURE_SPECIES.ToList();
            cSpecies.ForEach(x =>
            {
                aSpecList.Add(new SelectListItem { Text = x.SpeciesDescription, Value = x.SpeciesID.ToString() });
            });

            List<vw_Aquaculture_Species> aProdSysList = Db.vw_Aquaculture_Species.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();

            model.ProdSyslist = aProdList;
            model.AquaSpecieslist = aSpecList;

            model.FarmerAquaSpecieslist = aProdSysList;

            return View(model);
        }

        //Save a Livestock Record to a a farmer
        [HttpPost]
        public ActionResult AddAquacultureSpecies(FarmerAquacultureSpeciesViewModel model)
        {
            var basedid = model.BaseID;
            FarmerAquacultureSpeciesViewModel model2 = new FarmerAquacultureSpeciesViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_AQUACULTURE.Where(s => s.BaseID == model.BaseID && s.SpeciesID == model.SpeciesID && s.ProductID == model.ProductID).FirstOrDefault();

                if (afamd != null)
                {
                    afamd.Fingerlings = model.Fingerlings;
                }
                else
                {
                    //Add new record into Database
                    Db.FARMER_AQUACULTURE.Add(new FARMER_AQUACULTURE()
                    {
                        BaseID = model.BaseID,
                        ProductID = model.ProductID,
                        SpeciesID = model.SpeciesID,
                        Fingerlings = model.Fingerlings
                    });
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return View("AddCrop", "Farmer", new { id = 0});
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<SelectListItem> aProdList = new List<SelectListItem>();

                List<STATIC_FARMER_AQUACULTURE_PRODSYS> cProds = Db.STATIC_FARMER_AQUACULTURE_PRODSYS.ToList();
                cProds.ForEach(x =>
                {
                    aProdList.Add(new SelectListItem { Text = x.ProductSystem, Value = x.ProductID.ToString() });
                });

                List<SelectListItem> aSpecList = new List<SelectListItem>();

                List<STATIC_FARMER_AQUACULTURE_SPECIES> cSpecies = Db.STATIC_FARMER_AQUACULTURE_SPECIES.ToList();
                cSpecies.ForEach(x =>
                {
                    aSpecList.Add(new SelectListItem { Text = x.SpeciesDescription, Value = x.SpeciesID.ToString() });
                });

                List<vw_Aquaculture_Species> aProdSysList = Db.vw_Aquaculture_Species.Where(p => p.BaseID == basedid).DefaultIfEmpty().ToList();

                model2.ProdSyslist = aProdList;
                model2.AquaSpecieslist = aSpecList;

                model2.FarmerAquaSpecieslist = aProdSysList;
            }
            return View(model2);
        }

        //Delete a Lvestock Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelAquacultureSpecies(int id, int pid)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_AQUACULTURE.Where(s => s.BaseID == baseid && s.SpeciesID == id && s.ProductID == pid).FirstOrDefault();

                Db.FARMER_AQUACULTURE.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult DelAquacultureSpecies2(int id, int pid)
        {
            //update database
            var bsid = (int)Session["id"];
            ViewBag.Message2 = String.Format("{0} - {1}", bsid, id);

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = bsid;
                var afamd = Db.FARMER_ANIMAL_FEEDS.Where(s => s.BaseID == baseid && s.FeedID == id).FirstOrDefault();

                Db.proc_DelFarmerAquaculture(baseid, id, pid);

                ViewBag.Message = "Record Successfully Deleted!!!";

                return RedirectToAction("AddAquacultureSpecies", "Farmer", new { id = 0 });
            }

        }

        //Aquaculture Methods
        // GET: Farmer/IFarmEquipment/5
        public ActionResult IFarmEquipment(int id)
        {

            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel7()
                          { // result selector 
                              BaseID = p.BaseID,
                              PowerSource = w.PowerSource,
                              FarmLaborSource = w.FarmLaborSource,
                              FarmStructures = w.FarmStructures,
                              FarmEquipmentOwner = w.FarmEquipmentOwner
                          }).SingleOrDefault();

            var mybiz = entity;
            //Captured Records for the Farmer ready for display
            List<vw_Farmer_Equipment_Qty> aEquipOwnerList = Db.vw_Farmer_Equipment_Qty.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
            mybiz.FarmerEquipmentlist = aEquipOwnerList;

            //Power Source
            List<SelectListItem> aPowerList = new List<SelectListItem>();
            List<STATIC_FARMER_POWERSOURCE> cSource = Db.STATIC_FARMER_POWERSOURCE.ToList();
            cSource.ForEach(x =>
            {
                aPowerList.Add(new SelectListItem { Text = x.PowerSource, Value = x.PowerID.ToString() });
            });
            if (mybiz.PowerSource != null)
            {
                var psourcestrings = mybiz.PowerSource.Split(',');
                List<string> CurpsourceList = psourcestrings != null ? psourcestrings.ToList() : null;
                if (CurpsourceList != null)
                {
                    List<SelectListItem> selectedItems = aPowerList.Where(p => CurpsourceList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.PowerSourceList = aPowerList;

            //Labor Source (Main)
            List<SelectListItem> aLaborList = new List<SelectListItem>();
            List<STATIC_FARMER_LABORSOURCE> cLabor = Db.STATIC_FARMER_LABORSOURCE.ToList();
            cLabor.ForEach(x =>
            {
                aLaborList.Add(new SelectListItem { Text = x.LaborDesc, Value = x.LaborID.ToString() });
            });
            
            mybiz.FarmLaborSourceList = aLaborList;

            //Farm Structures
            List<SelectListItem> aStructList = new List<SelectListItem>();
            List<STATIC_FARMER_FARMSTRUCTURE> cStruct = Db.STATIC_FARMER_FARMSTRUCTURE.ToList();
            cStruct.ForEach(x =>
            {
                aStructList.Add(new SelectListItem { Text = x.Structure, Value = x.StructID.ToString() });
            });
            if (mybiz.FarmStructures != null)
            {
                var astructstrings = mybiz.FarmStructures.Split(',');
                List<string> CurStructList = astructstrings != null ? astructstrings.ToList() : null;
                if (CurStructList != null)
                {
                    List<SelectListItem> selectedItems = aStructList.Where(p => CurStructList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FarmStructuresList = aStructList;

            //Equipment Ownership
            List<SelectListItem> aOwnerList = new List<SelectListItem>();
            List<STATIC_FARMER_EQUIPMENTOWNER> cOwner = Db.STATIC_FARMER_EQUIPMENTOWNER.ToList();
            cOwner.ForEach(x =>
            {
                aOwnerList.Add(new SelectListItem { Text = x.OwnerDesc, Value = x.OwnerID.ToString() });
            });

            mybiz.FarmEquipmentOwnerList = aOwnerList;
            
            return View(mybiz);
        }
        
        // POST: Farmer/IFarmEquipment/5
        [HttpPost]
        public ActionResult IFarmEquipment(int id, FarmerViewModel7 mybiz)
        {
            ViewBag.Message = "";
            FarmerViewModel7 mybiz2 = new FarmerViewModel7();
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                if (mybiz.SelFarmStructuresList != null)
                {
                    string mstructdsc = string.Join(",", bizna.SelFarmStructuresList);
                    bizna.FarmStructures = mstructdsc;
                }
                
                if (afamd != null)
                {
                    afamd.PowerSource = bizna.PowerSource;
                    afamd.FarmLaborSource = bizna.FarmLaborSource;
                    afamd.FarmStructures = bizna.FarmStructures;
                    afamd.FarmEquipmentOwner = bizna.FarmEquipmentOwner;
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                //var mybiz = mybiz2;
                //var mybiz = entity;
                //Captured Records for the Farmer ready for display
                List<vw_Farmer_Equipment_Qty> aEquipOwnerList = Db.vw_Farmer_Equipment_Qty.Where(p => p.BaseID == id).DefaultIfEmpty().ToList();
                mybiz2.FarmerEquipmentlist = aEquipOwnerList;

                //Power Source
                List<SelectListItem> aPowerList = new List<SelectListItem>();
                List<STATIC_FARMER_POWERSOURCE> cSource = Db.STATIC_FARMER_POWERSOURCE.ToList();
                cSource.ForEach(x =>
                {
                    aPowerList.Add(new SelectListItem { Text = x.PowerSource, Value = x.PowerID.ToString() });
                });
                if (mybiz.PowerSource != null)
                {
                    var psourcestrings = mybiz.PowerSource.Split(',');
                    List<string> CurpsourceList = psourcestrings != null ? psourcestrings.ToList() : null;
                    if (CurpsourceList != null)
                    {
                        List<SelectListItem> selectedItems = aPowerList.Where(p => CurpsourceList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.PowerSourceList = aPowerList;

                //Labor Source (Main)
                List<SelectListItem> aLaborList = new List<SelectListItem>();
                List<STATIC_FARMER_LABORSOURCE> cLabor = Db.STATIC_FARMER_LABORSOURCE.ToList();
                cLabor.ForEach(x =>
                {
                    aLaborList.Add(new SelectListItem { Text = x.LaborDesc, Value = x.LaborID.ToString() });
                });

                mybiz2.FarmLaborSourceList = aLaborList;

                //Farm Structures
                List<SelectListItem> aStructList = new List<SelectListItem>();
                List<STATIC_FARMER_FARMSTRUCTURE> cStruct = Db.STATIC_FARMER_FARMSTRUCTURE.ToList();
                cStruct.ForEach(x =>
                {
                    aStructList.Add(new SelectListItem { Text = x.Structure, Value = x.StructID.ToString() });
                });
                if (mybiz.FarmStructures != null)
                {
                    var astructstrings = mybiz.FarmStructures.Split(',');
                    List<string> CurStructList = astructstrings != null ? astructstrings.ToList() : null;
                    if (CurStructList != null)
                    {
                        List<SelectListItem> selectedItems = aStructList.Where(p => CurStructList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.FarmStructuresList = aStructList;

                //Equipment Ownership
                List<SelectListItem> aOwnerList = new List<SelectListItem>();
                List<STATIC_FARMER_EQUIPMENTOWNER> cOwner = Db.STATIC_FARMER_EQUIPMENTOWNER.ToList();
                cOwner.ForEach(x =>
                {
                    aOwnerList.Add(new SelectListItem { Text = x.OwnerDesc, Value = x.OwnerID.ToString() });
                });

                mybiz2.FarmEquipmentOwnerList = aOwnerList;

            }

            return View("IFarmEquipment", mybiz2);
        }

        //Get a Farmer Equipment Record of a farmer for View/Edits
        public ActionResult AddFarmEquipment(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var baseid = (int)Session["id"];
            ViewBag.Message = "";
            FarmerEquipmentViewModel model = new FarmerEquipmentViewModel();
            if (id != 0)
            {
                var aequip = Db.vw_Farmer_Equipment_Qty.Where(x => x.EquipID == id && x.BaseID == baseid).DefaultIfEmpty();
                if (aequip != null)
                {
                    foreach (var item in aequip)
                    {
                        model.BaseID = item.BaseID;
                        model.EquipID = item.EquipID;
                        model.Qty = item.Qty;
                    }
                }
            }
            else
            {
                model.BaseID = (int)Session["id"];
            }

            List<SelectListItem> aEquipList = new List<SelectListItem>();

            List<STATIC_FARMER_EQUIPMENT> cEquip = Db.STATIC_FARMER_EQUIPMENT.ToList();
            cEquip.ForEach(x =>
            {
                aEquipList.Add(new SelectListItem { Text = x.Equipment, Value = x.EquipID.ToString() });
            });

            List<vw_Farmer_Equipment_Qty> CurEquipList = Db.vw_Farmer_Equipment_Qty.Where(p => p.BaseID == baseid).DefaultIfEmpty().ToList();

            model.Equipmentlist = aEquipList;

            model.FarmerEquipmentlist = CurEquipList;

            return View(model);
        }

        //Save a Equipment Record to a farmer
        [HttpPost]
        public ActionResult AddFarmEquipment(FarmerEquipmentViewModel model)
        {
            var basedid = model.BaseID;
            FarmerEquipmentViewModel model2 = new FarmerEquipmentViewModel();

            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_EQUIPMENT.Where(s => s.BaseID == model.BaseID && s.EquipID == model.EquipID).FirstOrDefault();

                if (afamd != null)
                {
                    afamd.Qty = model.Qty;
                }
                else
                {
                    //Add new record into Database
                    Db.FARMER_EQUIPMENT.Add(new FARMER_EQUIPMENT()
                    {
                        BaseID = model.BaseID,
                        EquipID = model.EquipID,
                        Qty = model.Qty
                    });
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return View("AddCrop", "Farmer", new { id = 0});
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                List<SelectListItem> aEquipList = new List<SelectListItem>();

                List<STATIC_FARMER_EQUIPMENT> cEquip = Db.STATIC_FARMER_EQUIPMENT.ToList();
                cEquip.ForEach(x =>
                {
                    aEquipList.Add(new SelectListItem { Text = x.Equipment, Value = x.EquipID.ToString() });
                });

                List<vw_Farmer_Equipment_Qty> CurEquipList = Db.vw_Farmer_Equipment_Qty.Where(p => p.BaseID == basedid).DefaultIfEmpty().ToList();

                model2.Equipmentlist = aEquipList;

                model2.FarmerEquipmentlist = CurEquipList;

            }
            return View(model2);
        }

        //Delete a Farmer Equipment Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelFarmEquipment(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var baseid = (int)Session["id"];
                var afamd = Db.FARMER_EQUIPMENT.Where(s => s.BaseID == baseid && s.EquipID == id).FirstOrDefault();

                Db.FARMER_EQUIPMENT.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }

        //Land, Water and Financial Methods
        // GET: Farmer/IFarmLandWaterFinances/5
        public ActionResult IFarmLandWaterFinances(int id)
        {

            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.FARMERs on p.BaseID equals w.BaseID
                          where p.BaseID == id
                          select new FarmerViewModel8()
                          { // result selector 
                              BaseID = p.BaseID,
                              LandManagementPractices = w.LandManagementPractices,
                              FarmerGroupAffiliate = w.FarmerGroupAffiliate,
                              FertilizerTypeUsed = w.FertilizerTypeUsed,
                              FinancialServices = w.FinancialServices,
                              FormatForAdvisories = w.FormatForAdvisories,
                              InsureCrops = w.InsureCrops,
                              InsureFarmAssets = w.InsureFarmAssets,
                              InsureLivestock = w.InsureLivestock,
                              IrrigationDone = w.IrrigationDone,
                              IrrigationImplementedBy = w.IrrigationImplementedBy,
                              KeepFarmRecords = w.KeepFarmRecords,
                              LimeOnSoil = w.LimeOnSoil,
                              MainExtensionProvider = w.MainExtensionProvider,
                              MainWaterSource = w.MainWaterSource,
                              PreferredLanguageAdvisories = w.PreferredLanguageAdvisories,
                              PreferredLanguageAdvisoriesOther = w.VernacularLanguage,
                              PreferredTimeAdvisories = w.PreferredTimeAdvisories,
                              SoilTesting = w.SoilTesting,
                              SourceFinancialLivehood = w.SourceFinancialLivehood,
                              SourceGAP = w.SourceGAP,
                              TotalAreaIrrigated = w.TotalAreaIrrigated,
                              TypeIrrigationProject = w.TypeIrrigationProject,
                              TypeOfIrrigation = w.TypeOfIrrigation
                          }).SingleOrDefault();

            var mybiz = entity;
            //Land Management Lists
            List<SelectListItem> aLandMgtList = new List<SelectListItem>();
            List<STATIC_FARMER_LANDPRACTICE> cTypes = Db.STATIC_FARMER_LANDPRACTICE.ToList();
            cTypes.ForEach(x =>
            {
                aLandMgtList.Add(new SelectListItem { Text = x.LandPractice, Value = x.PracticeID.ToString() });
            });
            if (mybiz.LandManagementPractices != null)
            {
                var aLmgttrings = mybiz.LandManagementPractices.Split(',');
                List<string> CurLMgtList = aLmgttrings != null ? aLmgttrings.ToList() : null;
                if (CurLMgtList != null)
                {
                    List<SelectListItem> selectedItems = aLandMgtList.Where(p => CurLMgtList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.LandManagementPracticesList = aLandMgtList;
            //Fertilizer Type List
            List<SelectListItem> aFertList = new List<SelectListItem>();
            List<STATIC_FARMER_FERTILIZER> cInputs = Db.STATIC_FARMER_FERTILIZER.ToList();
            cInputs.ForEach(x =>
            {
                aFertList.Add(new SelectListItem { Text = x.FertilizerDesc, Value = x.FertilizerID.ToString() });
            });
            if (mybiz.FertilizerTypeUsed != null)
            {
                var aqinputstrings = mybiz.FertilizerTypeUsed.Split(',');
                List<string> CurAquInputsList = aqinputstrings != null ? aqinputstrings.ToList() : null;
                if (CurAquInputsList != null)
                {
                    List<SelectListItem> selectedItems = aFertList.Where(p => CurAquInputsList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FertilizerTypeUsedList = aFertList;
            //Water Source to the Farm
            List<SelectListItem> aWaterSourceList = new List<SelectListItem>();
            List<STATIC_FARMER_WATERSOURCE> cWaterSources = Db.STATIC_FARMER_WATERSOURCE.ToList();
            cWaterSources.ForEach(x =>
            {
                aWaterSourceList.Add(new SelectListItem { Text = x.WaterSource, Value = x.WaterSourceID.ToString() });
            });
            if (mybiz.MainWaterSource != null)
            {
                var aqlevelstrings = mybiz.MainWaterSource.Split(',');
                List<string> CurAquLevelsList = aqlevelstrings != null ? aqlevelstrings.ToList() : null;
                if (CurAquLevelsList != null)
                {
                    List<SelectListItem> selectedItems = aWaterSourceList.Where(p => CurAquLevelsList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.MainWaterSourceList = aWaterSourceList;

            //TypeOfIrrigationList
            List<SelectListItem> aIrrTypList = new List<SelectListItem>();
            List<STATIC_FARMER_IRRIGATIONTYPES> cIrr = Db.STATIC_FARMER_IRRIGATIONTYPES.ToList();
            cIrr.ForEach(x =>
            {
                aIrrTypList.Add(new SelectListItem { Text = x.IrrigationType, Value = x.IrrigationID.ToString() });
            });
            if (mybiz.TypeOfIrrigation != null)
            {
                var arrstrings = mybiz.TypeOfIrrigation.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aIrrTypList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.TypeOfIrrigationList = aIrrTypList;

            //TypeIrrigationProjectList
            List<SelectListItem> aIrrProjList = new List<SelectListItem>();
            List<STATIC_FARMER_IRRIGATIONPROJECT> cIrrProj = Db.STATIC_FARMER_IRRIGATIONPROJECT.ToList();
            cIrrProj.ForEach(x =>
            {
                aIrrProjList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.TypeIrrigationProject != null)
            {
                var arrstrings = mybiz.TypeIrrigationProject.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aIrrProjList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.TypeIrrigationProjectList = aIrrProjList;

            //IrrigationImplementedByList
            List<SelectListItem> aIrrByList = new List<SelectListItem>();
            List<STATIC_FARMER_IRRIGATIONBODY> cIrrBy = Db.STATIC_FARMER_IRRIGATIONBODY.ToList();
            cIrrBy.ForEach(x =>
            {
                aIrrByList.Add(new SelectListItem { Text = x.BodyDescription, Value = x.BodyID.ToString() });
            });
            if (mybiz.IrrigationImplementedBy != null)
            {
                var arrstrings = mybiz.IrrigationImplementedBy.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aIrrByList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.IrrigationImplementedByList = aIrrByList;

            //SourceFinancialLivehoodList
            List<SelectListItem> aSourceFinanceList = new List<SelectListItem>();
            List<STATIC_FARMER_FINANCIALS> cSourceFin = Db.STATIC_FARMER_FINANCIALS.ToList();
            cSourceFin.ForEach(x =>
            {
                aSourceFinanceList.Add(new SelectListItem { Text = x.SourceDesc, Value = x.SourceID.ToString() });
            });
            if (mybiz.SourceFinancialLivehood != null)
            {
                var arrstrings = mybiz.SourceFinancialLivehood.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aSourceFinanceList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.SourceFinancialLivehoodList = aSourceFinanceList;

            //FarmerGroupAffiliateList
            List<SelectListItem> aFGroupList = new List<SelectListItem>();
            List<STATIC_FARMER_GROUPS> cFGroup = Db.STATIC_FARMER_GROUPS.ToList();
            cFGroup.ForEach(x =>
            {
                aFGroupList.Add(new SelectListItem { Text = x.GroupDesc, Value = x.GroupID.ToString() });
            });
            if (mybiz.FarmerGroupAffiliate != null)
            {
                var arrstrings = mybiz.FarmerGroupAffiliate.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aFGroupList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FarmerGroupAffiliateList = aFGroupList;

            //FinancialServicesList
            List<SelectListItem> aFServList = new List<SelectListItem>();
            List<STATIC_FARMER_FINANCESERVICES> cFServ = Db.STATIC_FARMER_FINANCESERVICES.ToList();
            cFServ.ForEach(x =>
            {
                aFServList.Add(new SelectListItem { Text = x.FinanceService, Value = x.FinanceID.ToString() });
            });
            if (mybiz.FinancialServices != null)
            {
                var arrstrings = mybiz.FinancialServices.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aFServList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FinancialServicesList = aFServList;

            //SourceGAPList
            List<SelectListItem> aGAPList = new List<SelectListItem>();
            List<STATIC_FARMER_INFO_SOURCEGAP> cGAP = Db.STATIC_FARMER_INFO_SOURCEGAP.ToList();
            cGAP.ForEach(x =>
            {
                aGAPList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.SourceGAP != null)
            {
                var arrstrings = mybiz.SourceGAP.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aGAPList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.SourceGAPList = aGAPList;

            //MainExtensionProviderList
            List<SelectListItem> aExtList = new List<SelectListItem>();
            List<STATIC_FARMER_EXTENSIONPROVIDER> cExt = Db.STATIC_FARMER_EXTENSIONPROVIDER.ToList();
            cExt.ForEach(x =>
            {
                aExtList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.MainExtensionProvider != null)
            {
                var arrstrings = mybiz.MainExtensionProvider.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aExtList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.MainExtensionProviderList = aExtList;

            //FormatForAdvisoriesList
            List<SelectListItem> aFormatList = new List<SelectListItem>();
            List<STATIC_FARMER_INFOMEDIA> cFormat = Db.STATIC_FARMER_INFOMEDIA.ToList();
            cFormat.ForEach(x =>
            {
                aFormatList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.FormatForAdvisories != null)
            {
                var arrstrings = mybiz.FormatForAdvisories.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aFormatList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.FormatForAdvisoriesList = aFormatList;

            //PreferredTimeAdvisoriesList
            List<SelectListItem> aTmAdvList = new List<SelectListItem>();
            List<STATIC_FARMER_PREFERREDADVISORYPERIOD> cTmAdv = Db.STATIC_FARMER_PREFERREDADVISORYPERIOD.ToList();
            cTmAdv.ForEach(x =>
            {
                aTmAdvList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.PreferredTimeAdvisories != null)
            {
                var arrstrings = mybiz.PreferredTimeAdvisories.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aTmAdvList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.PreferredTimeAdvisoriesList = aTmAdvList;

            //PreferredLanguageAdvisoriesList
            List<SelectListItem> aPrefLangList = new List<SelectListItem>();
            List<STATIC_FARMER_PREFERREDADVISORYLANGUAGE> cPrefLang = Db.STATIC_FARMER_PREFERREDADVISORYLANGUAGE.ToList();
            cPrefLang.ForEach(x =>
            {
                aPrefLangList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });
            if (mybiz.PreferredTimeAdvisories != null)
            {
                var arrstrings = mybiz.PreferredTimeAdvisories.Split(',');
                List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                if (CurArrList != null)
                {
                    List<SelectListItem> selectedItems = aPrefLangList.Where(p => CurArrList.Contains(p.Value)).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
            mybiz.PreferredLanguageAdvisoriesList = aPrefLangList;
            
            return View(mybiz);
        }


        // POST: Farmer/IFarmLandWaterFinances/5
        [HttpPost]
        public ActionResult IFarmLandWaterFinances(int id, FarmerViewModel8 mybiz)
        {
            ViewBag.Message = "";
            FarmerViewModel8 mybiz2 = new FarmerViewModel8();
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                var bizna = mybiz;

                var afamd = Db.FARMERs.Where(s => s.BaseID == id).FirstOrDefault();
                if (mybiz.SelLandManagementPractices != null)
                {
                    string mlmgtdsc = string.Join(",", bizna.SelLandManagementPractices);
                    bizna.LandManagementPractices = mlmgtdsc;
                }
                if (mybiz.SelFertilizerTypeUsed != null)
                {
                    string mfertdsc = string.Join(",", bizna.SelFertilizerTypeUsed);
                    bizna.FertilizerTypeUsed = mfertdsc;
                }
                if (mybiz.SelFarmerGroupAffiliate != null)
                {
                    string mgroupdsc = string.Join(",", bizna.SelFarmerGroupAffiliate);
                    bizna.FarmerGroupAffiliate = mgroupdsc;
                }
                if (mybiz.SelFinancialServices != null)
                {
                    string mfinservdsc = string.Join(",", bizna.SelFinancialServices);
                    bizna.FinancialServices = mfinservdsc;
                }
                if(bizna.PreferredLanguageAdvisories != "3")
                {
                    bizna.PreferredLanguageAdvisoriesOther = "";
                }

                if (afamd != null)
                {
                    afamd.LandManagementPractices = bizna.LandManagementPractices;
                    afamd.FarmerGroupAffiliate = bizna.FarmerGroupAffiliate;
                    afamd.FertilizerTypeUsed = bizna.FertilizerTypeUsed;
                    afamd.FinancialServices = bizna.FinancialServices;
                    afamd.FormatForAdvisories = bizna.FormatForAdvisories;
                    afamd.InsureCrops = bizna.InsureCrops;
                    afamd.InsureFarmAssets = bizna.InsureFarmAssets;
                    afamd.InsureLivestock = bizna.InsureLivestock;
                    afamd.IrrigationDone = bizna.IrrigationDone;
                    afamd.IrrigationImplementedBy = bizna.IrrigationImplementedBy;
                    afamd.KeepFarmRecords = bizna.KeepFarmRecords;
                    afamd.LimeOnSoil = bizna.LimeOnSoil;
                    afamd.MainExtensionProvider = bizna.MainExtensionProvider;
                    afamd.MainWaterSource = bizna.MainWaterSource;
                    afamd.PreferredLanguageAdvisories = bizna.PreferredLanguageAdvisories;
                    afamd.VernacularLanguage = bizna.PreferredLanguageAdvisoriesOther;
                    afamd.PreferredTimeAdvisories = bizna.PreferredTimeAdvisories;
                    afamd.SoilTesting = bizna.SoilTesting;
                    afamd.SourceFinancialLivehood = bizna.SourceFinancialLivehood;
                    afamd.SourceGAP = bizna.SourceGAP;
                    afamd.TotalAreaIrrigated = bizna.TotalAreaIrrigated;
                    afamd.TypeIrrigationProject = bizna.TypeIrrigationProject;
                    afamd.TypeOfIrrigation = bizna.TypeOfIrrigation;
                }

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    Db.SaveChanges();
                    ViewBag.Message = "Record Successfully Saved!!!";
                    //return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                //Land Management Lists
                List<SelectListItem> aLandMgtList = new List<SelectListItem>();
                List<STATIC_FARMER_LANDPRACTICE> cTypes = Db.STATIC_FARMER_LANDPRACTICE.ToList();
                cTypes.ForEach(x =>
                {
                    aLandMgtList.Add(new SelectListItem { Text = x.LandPractice, Value = x.PracticeID.ToString() });
                });
                if (mybiz.LandManagementPractices != null)
                {
                    var aLmgttrings = mybiz.LandManagementPractices.Split(',');
                    List<string> CurLMgtList = aLmgttrings != null ? aLmgttrings.ToList() : null;
                    if (CurLMgtList != null)
                    {
                        List<SelectListItem> selectedItems = aLandMgtList.Where(p => CurLMgtList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.LandManagementPracticesList = aLandMgtList;
                //Fertilizer Type List
                List<SelectListItem> aFertList = new List<SelectListItem>();
                List<STATIC_FARMER_FERTILIZER> cInputs = Db.STATIC_FARMER_FERTILIZER.ToList();
                cInputs.ForEach(x =>
                {
                    aFertList.Add(new SelectListItem { Text = x.FertilizerDesc, Value = x.FertilizerID.ToString() });
                });
                if (mybiz.FertilizerTypeUsed != null)
                {
                    var aqinputstrings = mybiz.FertilizerTypeUsed.Split(',');
                    List<string> CurAquInputsList = aqinputstrings != null ? aqinputstrings.ToList() : null;
                    if (CurAquInputsList != null)
                    {
                        List<SelectListItem> selectedItems = aFertList.Where(p => CurAquInputsList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.FertilizerTypeUsedList = aFertList;
                //Water Source to the Farm
                List<SelectListItem> aWaterSourceList = new List<SelectListItem>();
                List<STATIC_FARMER_WATERSOURCE> cWaterSources = Db.STATIC_FARMER_WATERSOURCE.ToList();
                cWaterSources.ForEach(x =>
                {
                    aWaterSourceList.Add(new SelectListItem { Text = x.WaterSource, Value = x.WaterSourceID.ToString() });
                });
                if (mybiz.MainWaterSource != null)
                {
                    var aqlevelstrings = mybiz.MainWaterSource.Split(',');
                    List<string> CurAquLevelsList = aqlevelstrings != null ? aqlevelstrings.ToList() : null;
                    if (CurAquLevelsList != null)
                    {
                        List<SelectListItem> selectedItems = aWaterSourceList.Where(p => CurAquLevelsList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.MainWaterSourceList = aWaterSourceList;

                //TypeOfIrrigationList
                List<SelectListItem> aIrrTypList = new List<SelectListItem>();
                List<STATIC_FARMER_IRRIGATIONTYPES> cIrr = Db.STATIC_FARMER_IRRIGATIONTYPES.ToList();
                cIrr.ForEach(x =>
                {
                    aIrrTypList.Add(new SelectListItem { Text = x.IrrigationType, Value = x.IrrigationID.ToString() });
                });
                if (mybiz.TypeOfIrrigation != null)
                {
                    var arrstrings = mybiz.TypeOfIrrigation.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aIrrTypList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.TypeOfIrrigationList = aIrrTypList;

                //TypeIrrigationProjectList
                List<SelectListItem> aIrrProjList = new List<SelectListItem>();
                List<STATIC_FARMER_IRRIGATIONPROJECT> cIrrProj = Db.STATIC_FARMER_IRRIGATIONPROJECT.ToList();
                cIrrProj.ForEach(x =>
                {
                    aIrrProjList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.TypeIrrigationProject != null)
                {
                    var arrstrings = mybiz.TypeIrrigationProject.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aIrrProjList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.TypeIrrigationProjectList = aIrrProjList;

                //IrrigationImplementedByList
                List<SelectListItem> aIrrByList = new List<SelectListItem>();
                List<STATIC_FARMER_IRRIGATIONBODY> cIrrBy = Db.STATIC_FARMER_IRRIGATIONBODY.ToList();
                cIrrBy.ForEach(x =>
                {
                    aIrrByList.Add(new SelectListItem { Text = x.BodyDescription, Value = x.BodyID.ToString() });
                });
                if (mybiz.IrrigationImplementedBy != null)
                {
                    var arrstrings = mybiz.IrrigationImplementedBy.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aIrrByList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.IrrigationImplementedByList = aIrrByList;

                //SourceFinancialLivehoodList
                List<SelectListItem> aSourceFinanceList = new List<SelectListItem>();
                List<STATIC_FARMER_FINANCIALS> cSourceFin = Db.STATIC_FARMER_FINANCIALS.ToList();
                cSourceFin.ForEach(x =>
                {
                    aSourceFinanceList.Add(new SelectListItem { Text = x.SourceDesc, Value = x.SourceID.ToString() });
                });
                if (mybiz.SourceFinancialLivehood != null)
                {
                    var arrstrings = mybiz.SourceFinancialLivehood.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aSourceFinanceList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.SourceFinancialLivehoodList = aSourceFinanceList;

                //FarmerGroupAffiliateList
                List<SelectListItem> aFGroupList = new List<SelectListItem>();
                List<STATIC_FARMER_GROUPS> cFGroup = Db.STATIC_FARMER_GROUPS.ToList();
                cFGroup.ForEach(x =>
                {
                    aFGroupList.Add(new SelectListItem { Text = x.GroupDesc, Value = x.GroupID.ToString() });
                });
                if (mybiz.FarmerGroupAffiliate != null)
                {
                    var arrstrings = mybiz.FarmerGroupAffiliate.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aFGroupList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.FarmerGroupAffiliateList = aFGroupList;

                //FinancialServicesList
                List<SelectListItem> aFServList = new List<SelectListItem>();
                List<STATIC_FARMER_FINANCESERVICES> cFServ = Db.STATIC_FARMER_FINANCESERVICES.ToList();
                cFServ.ForEach(x =>
                {
                    aFServList.Add(new SelectListItem { Text = x.FinanceService, Value = x.FinanceID.ToString() });
                });
                if (mybiz.FinancialServices != null)
                {
                    var arrstrings = mybiz.FinancialServices.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aFServList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.FinancialServicesList = aFServList;

                //SourceGAPList
                List<SelectListItem> aGAPList = new List<SelectListItem>();
                List<STATIC_FARMER_INFO_SOURCEGAP> cGAP = Db.STATIC_FARMER_INFO_SOURCEGAP.ToList();
                cGAP.ForEach(x =>
                {
                    aGAPList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.SourceGAP != null)
                {
                    var arrstrings = mybiz.SourceGAP.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aGAPList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.SourceGAPList = aGAPList;

                //MainExtensionProviderList
                List<SelectListItem> aExtList = new List<SelectListItem>();
                List<STATIC_FARMER_EXTENSIONPROVIDER> cExt = Db.STATIC_FARMER_EXTENSIONPROVIDER.ToList();
                cExt.ForEach(x =>
                {
                    aExtList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.MainExtensionProvider != null)
                {
                    var arrstrings = mybiz.MainExtensionProvider.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aExtList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.MainExtensionProviderList = aExtList;

                //FormatForAdvisoriesList
                List<SelectListItem> aFormatList = new List<SelectListItem>();
                List<STATIC_FARMER_INFOMEDIA> cFormat = Db.STATIC_FARMER_INFOMEDIA.ToList();
                cFormat.ForEach(x =>
                {
                    aFormatList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.FormatForAdvisories != null)
                {
                    var arrstrings = mybiz.FormatForAdvisories.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aFormatList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.FormatForAdvisoriesList = aFormatList;

                //PreferredTimeAdvisoriesList
                List<SelectListItem> aTmAdvList = new List<SelectListItem>();
                List<STATIC_FARMER_PREFERREDADVISORYPERIOD> cTmAdv = Db.STATIC_FARMER_PREFERREDADVISORYPERIOD.ToList();
                cTmAdv.ForEach(x =>
                {
                    aTmAdvList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.PreferredTimeAdvisories != null)
                {
                    var arrstrings = mybiz.PreferredTimeAdvisories.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aTmAdvList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.PreferredTimeAdvisoriesList = aTmAdvList;

                //PreferredLanguageAdvisoriesList
                List<SelectListItem> aPrefLangList = new List<SelectListItem>();
                List<STATIC_FARMER_PREFERREDADVISORYLANGUAGE> cPrefLang = Db.STATIC_FARMER_PREFERREDADVISORYLANGUAGE.ToList();
                cPrefLang.ForEach(x =>
                {
                    aPrefLangList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
                if (mybiz.PreferredTimeAdvisories != null)
                {
                    var arrstrings = mybiz.PreferredTimeAdvisories.Split(',');
                    List<string> CurArrList = arrstrings != null ? arrstrings.ToList() : null;
                    if (CurArrList != null)
                    {
                        List<SelectListItem> selectedItems = aPrefLangList.Where(p => CurArrList.Contains(p.Value)).ToList();
                        foreach (var selectedItem in selectedItems)
                        {
                            selectedItem.Selected = true;
                        }
                    }
                }
                mybiz2.PreferredLanguageAdvisoriesList = aPrefLangList;
            }

            return View("IFarmLandWaterFinances", mybiz2);
        }

        public PartialViewResult PartialFarmerMenu()
        {
            if (Session["id"] == null)
            {
                this.RedirectToAction("Index", "Home");
            }
            return PartialView();
        }
    }
}
