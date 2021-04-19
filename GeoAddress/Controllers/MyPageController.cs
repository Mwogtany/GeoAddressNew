using GeoAddress.Models;
using GeoAddress.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Controllers
{
    [Authorize]
    public class MyPageController : Controller
    {
        //public string localpath = "http://localhost:801/";ExtLocalPath
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        public ActionResult Index()
        {
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            // GET: MyPage - After login display own assets / info
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var muser = Session["user"].ToString();
            //General Statistics
            TopSummaryViewModel mytop = new TopSummaryViewModel();
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            mytop.Farmers = Db.vw_Farmers.Count();
            mytop.Schools = Db.vw_Schools.Count();
            mytop.Households = Db.vw_HouseHolds.Count();
            mytop.Businesses = Db.vw_Businesses.Count();
            mytop.Hospitals = Db.vw_Hospitals.Count();

            ViewBag.MyTop = mytop;

            MyRecordsViewModel myrecs = new MyRecordsViewModel();

            myrecs.Farmers = Db.vw_Farmers.Where(x => x.UserID == muser && x.Category == "F").Count();
            myrecs.Schools = Db.vw_Schools.Where(x => x.UserID == muser && x.Category == "S").Count();
            myrecs.Households = Db.vw_HouseHolds.Where(x => x.UserID == muser && x.Category == "P").Count();
            myrecs.Businesses = Db.vw_Businesses.Where(x => x.UserID == muser && x.Category == "B").Count();
            myrecs.Hospitals = Db.vw_Hospitals.Where(x => x.UserID == muser && x.Category == "H").Count();
            myrecs.FarmersCurService = Db.vw_Farmer_Service_Requests.Where(x => x.UserID == muser).ToList();
            myrecs.ActiveProvidedService = Db.vw_Service_Provider.Where(x => x.username == muser && x.Status == "1").Count();
            myrecs.InActiveProvidedService = Db.vw_Service_Provider.Where(x => x.username == muser && x.Status != "1").Count();

            ViewBag.MyRecs = myrecs;
            Session["servid"] = 0;
            Session["provservid"] = 0;

            var myFarmService = Db.vw_Farmers.Where(x => x.UserID == muser && x.Category == "F").SingleOrDefault();
            if (myFarmService != null)
            {
                Session["BaseID"] = myFarmService.BaseID;
            }
            else
            {
                Session["BaseID"] = 0;
            }

            return View();
        }

        public PartialViewResult PartialTopSummary()
        {
            // GET: MyPage - After login display own assets / info
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            //General Statistics
            TopSummaryViewModel mytop = new TopSummaryViewModel();

            mytop.Farmers = Db.vw_Farmers.Count();
            mytop.Schools = Db.vw_Schools.Count();
            mytop.Households = Db.vw_HouseHolds.Count();
            mytop.Businesses = Db.vw_Businesses.Count();
            mytop.Hospitals = Db.vw_Hospitals.Count();

            return PartialView(mytop);
        }

        public PartialViewResult PartialMyRecords()
        {
            // GET: MyPage - After login display own assets / info
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var muser = Session["user"].ToString();
            //General Statistics
            MyRecordsViewModel myrecs = new MyRecordsViewModel();

            myrecs.Farmers = Db.vw_Farmers.Where(x => x.UserID == muser && x.Category == "F").Count();
            myrecs.Schools = Db.vw_Schools.Where(x => x.UserID == muser && x.Category == "S").Count();
            myrecs.Households = Db.vw_HouseHolds.Where(x => x.UserID == muser && x.Category == "P").Count();
            myrecs.Businesses = Db.vw_Businesses.Where(x => x.UserID == muser && x.Category == "B").Count();
            myrecs.Hospitals = Db.vw_Hospitals.Where(x => x.UserID == muser && x.Category == "H").Count();

            return PartialView(myrecs);
        }

        public PartialViewResult PartialServiceRecords()
        {
            // GET: MyPage - After login display own assets / info
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var muser = Session["user"].ToString();
            //General Statistics
            MyRecordsViewModel myrecs = new MyRecordsViewModel();

            myrecs.Farmers = Db.vw_Farmers.Where(x => x.UserID == muser && x.Category == "F").Count();
            myrecs.Schools = Db.vw_Schools.Where(x => x.UserID == muser && x.Category == "S").Count();
            myrecs.Households = Db.vw_HouseHolds.Where(x => x.UserID == muser && x.Category == "P").Count();
            myrecs.Businesses = Db.vw_Businesses.Where(x => x.UserID == muser && x.Category == "B").Count();
            myrecs.Hospitals = Db.vw_Hospitals.Where(x => x.UserID == muser && x.Category == "H").Count();
            myrecs.FarmersCurService = Db.vw_Farmer_Service_Requests.Where(x => x.UserID == muser).ToList();
            myrecs.FarmersPrevService = Db.vw_Farmer_Services_Done.Where(x => x.UserID == muser).Count();
            
            //Session["id"] = 0;

            return PartialView(myrecs);
        }

        public ActionResult Farmers()
        {
            IEnumerable<FarmerViewModel> bizness = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            using (var client = new HttpClient())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Farmer/" + mUser.ToString());

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
            return View(bizness);
        }
        public ActionResult Schools()
        {
            IEnumerable<SkulViewModel> bizness = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            using (var client = new HttpClient())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Skul/" + mUser.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SkulViewModel>>();
                    readTask.Wait();

                    bizness = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bizness = Enumerable.Empty<SkulViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bizness);
        }
        public ActionResult Hospitals()
        {
            IEnumerable<HospViewModel> bizness = null;
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            using (var client = new HttpClient())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];
                string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Hosp/" + mUser.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HospViewModel>>();
                    readTask.Wait();

                    bizness = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bizness = Enumerable.Empty<HospViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bizness);
        }
        public ActionResult Households()
        {
            IEnumerable<HseViewModel> bizness = null;
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            using (var client = new HttpClient())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];
                string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Hse/" + mUser.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HseViewModel>>();
                    readTask.Wait();

                    bizness = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bizness = Enumerable.Empty<HseViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bizness);
        }
        public ActionResult Businesses()
        {
            IEnumerable<BizViewModel> bizness = null;
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            ViewData["ExtLocalPath"] = HttpContext.Application["ExtLocalPath"];
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

                var entity = (from p in Db.vw_Businesses
                              where p.UserID == mUser  //|| myrole.RoleID == 1
                              select new BizViewModel()
                              { // result selector 
                                  BaseID = p.BaseID,
                                  BusinessName = p.BusinessName,
                                  ContactPhone = p.ContactPhone,
                                  ContactEmail = p.ContactEmail,
                                  Website = p.Website,
                                  Building = p.Building,
                                  County_Code = p.County_Name,
                                  Constituency_Code = p.Constituency_Name,
                                  Sub_County_Code = p.Sub_County_Name,
                                  Ward_Code = p.Ward_Name,
                                  Latitude = p.Latitude,
                                  Longitude = p.Longitude,
                                  Pluscode = p.Pluscode,
                                  Address = p.Address
                              }).ToArray();

                if (entity != null)
                {
                    bizness = entity;
                }
                else
                {
                    bizness = Enumerable.Empty<BizViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(bizness);
        }

        public ActionResult NewFarmerService(int id)
        {
            NewServiceViewModel bizness = new NewServiceViewModel();
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            
            var entity = Db.vw_Farmer_Service_Requests.Where(p=> p.UserID == mUser).ToList();

            List<SelectListItem> aCategory = new List<SelectListItem>();
            List<SelectListItem> aServiceList = new List<SelectListItem>();
            List<STATIC_FARMER_SERVICES_CATEGORY> cCategories = Db.STATIC_FARMER_SERVICES_CATEGORY.ToList();
            cCategories.ForEach(x =>
            {
                aCategory.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });

            if (id.ToString() != "0")
            {
                var aService = Db.vw_Farmer_Service_Requests.Where(x => x.ServiceRunID == id).DefaultIfEmpty();
                if (aService != null)
                {
                    foreach (var item in aService)
                    {
                        bizness.ServiceRunID = item.ServiceRunID;
                        bizness.ServiceID = item.ServiceID;
                        bizness.Description = item.Description;
                        bizness.CategoryID = item.CategoryID;
                    }
                }
                List<STATIC_FARMER_SERVICES> cServices = Db.STATIC_FARMER_SERVICES.Where(x => x.category_id == bizness.CategoryID).ToList();
                cServices.ForEach(x =>
                {
                    aServiceList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
            }
            else
            {
                aServiceList.Add(new SelectListItem { Text = "<<Select Service>>", Value = "0" });
            }

            bizness.FarmerCurServiceList = entity;

            

            var baseid = (int)Session["BaseID"];
            bizness.BaseID = baseid;
            bizness.CategoryList = aCategory;
            bizness.ServiceList = aServiceList;

            return View(bizness);
        }

        [HttpPost]
        public ActionResult NewFarmerService(NewServiceViewModel model)
        {
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            NewServiceViewModel bizness = null;
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var basedid = model.BaseID;
            var servID = model.ServiceRunID;

            //update database

            var afamd = Db.FARMER_SERVICE_REQUEST.Where(s => s.BaseID == model.BaseID && s.ServiceRunID == model.ServiceRunID).FirstOrDefault();

            if (afamd != null)
            {
                afamd.Description = model.Description;
            }
            else
            {
                //Add new record into Database
                Db.FARMER_SERVICE_REQUEST.Add(new FARMER_SERVICE_REQUEST()
                {
                    BaseID = model.BaseID,
                    ServiceID = model.ServiceID,
                    Description = model.Description
                });
            }

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Saved!!!";
                return RedirectToAction("NewFarmerService", "MyPage", new { id = 0 });
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

            var entity = Db.vw_Farmer_Service_Requests.Where(p => p.UserID == mUser).ToList();
            
            bizness.FarmerCurServiceList = entity;

            List<SelectListItem> aCategory = new List<SelectListItem>();
            List<SelectListItem> aServiceList = new List<SelectListItem>();

            List<STATIC_FARMER_SERVICES_CATEGORY> cCategories = Db.STATIC_FARMER_SERVICES_CATEGORY.ToList();
            cCategories.ForEach(x =>
            {
                aCategory.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });

            aServiceList.Add(new SelectListItem { Text = "Select Service", Value = "0" });

            var baseid = (int)Session["BaseID"];
            bizness.BaseID = baseid;
            bizness.CategoryList = aCategory;
            bizness.ServiceList = aServiceList;

            return View(bizness);
        }

        //Delete a Farmer Service Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelFarmerService(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.FARMER_SERVICE_REQUEST.Where(s => s.ServiceRunID == id).FirstOrDefault();

                Db.FARMER_SERVICE_REQUEST.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }
        public PartialViewResult PartialMyService()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            var muser = Session["user"].ToString();
            
            var FarmersCurService = Db.vw_Farmer_Service_Requests.Where(x => x.UserID == muser.ToString()).ToList();

            //Session["id"] = 0;

            return PartialView(FarmersCurService);
        }
        public PartialViewResult PartialMyPrevService()
        {
            return PartialView();
        }
        public PartialViewResult PartialAdmin()
        {
            return PartialView();
        }
        public ActionResult NewProvideService(int id)
        {
            ProvideServiceViewModel bizness = new ProvideServiceViewModel();
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];

            var entity = Db.vw_Service_Provider.Where(p => p.username == mUser).ToList();

            List<SelectListItem> aCategory = new List<SelectListItem>();
            List<SelectListItem> aServiceList = new List<SelectListItem>();
            List<STATIC_FARMER_SERVICES_CATEGORY> cCategories = Db.STATIC_FARMER_SERVICES_CATEGORY.ToList();
            cCategories.ForEach(x =>
            {
                aCategory.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });

            if (id.ToString() != "0")
            {
                var aService = Db.vw_Service_Provider.Where(x => x.ProviderID == id).DefaultIfEmpty();
                if (aService != null)
                {
                    foreach (var item in aService)
                    {
                        bizness.category_id = item.category_id;
                        bizness.ProviderID = item.ProviderID;
                        bizness.ServiceID = item.ServiceID;
                        bizness.ServiceDescription = item.ServiceDescription;
                        bizness.Profession = item.Profession;
                        bizness.Qualification = item.Qualification;
                        bizness.Status = item.Status;
                        bizness.StatusChangedBy = item.StatusChangedBy;
                        bizness.StatusChangedOn = item.StatusChangedOn;
                        bizness.userid = item.userid;
                    }
                }
                else
                {
                    bizness.userid = (int)Session["userid"];  //Current Logged In user
                }

                List<STATIC_FARMER_SERVICES> cServices = Db.STATIC_FARMER_SERVICES.Where(x => x.category_id == bizness.category_id).ToList();
                cServices.ForEach(x =>
                {
                    aServiceList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
            }
            else
            {
                aServiceList.Add(new SelectListItem { Text = "<<Select Service>>", Value = "0" });
            }

            bizness.ServicesProvidedList = entity;
            
            var baseid = (int)Session["BaseID"];
            bizness.CategoryList = aCategory;
            bizness.ServiceList = aServiceList;

            return View(bizness);
        }

        [HttpPost]
        public ActionResult NewProvideService(ProvideServiceViewModel model)
        {
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            int mUserID = (int)Session["userid"];
            var id = model.ProviderID;
            ProvideServiceViewModel bizness = null;
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            //update database

            var afamd = Db.SERVICE_PROVIDER.Where(s => s.ProviderID == model.ProviderID).FirstOrDefault();

            if (afamd != null)
            {
                afamd.ServiceID = model.ServiceID;
                afamd.Profession = model.Profession;
                afamd.Qualification = model.Qualification;
                afamd.ServiceDescription = model.ServiceDescription;
                afamd.StatusChangedBy = mUser;
                afamd.StatusChangedOn = DateTime.Now;
                afamd.ModifiedOn = DateTime.Now;
            }
            else
            {
                //Add new record into Database
                Db.SERVICE_PROVIDER.Add(new SERVICE_PROVIDER()
                {
                    ProviderID = model.ProviderID,
                    ServiceID = model.ServiceID,
                    ServiceDescription = model.ServiceDescription,
                    Profession = model.Profession,
                    Qualification = model.Qualification,
                    Status = "0",
                    StatusChangedBy = mUser,
                    StatusChangedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    userid = mUserID
                });
            }

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Saved!!!";
                return RedirectToAction("NewProvideService", "MyPage", new { id = 0 });
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
            
            var entity = Db.vw_Service_Provider.Where(p => p.username == mUser).ToList();

            List<SelectListItem> aCategory = new List<SelectListItem>();
            List<SelectListItem> aServiceList = new List<SelectListItem>();
            List<STATIC_FARMER_SERVICES_CATEGORY> cCategories = Db.STATIC_FARMER_SERVICES_CATEGORY.ToList();
            cCategories.ForEach(x =>
            {
                aCategory.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
            });

            if (id.ToString() != "0")
            {
                var aService = Db.vw_Service_Provider.Where(x => x.ProviderID == id).DefaultIfEmpty();
                if (aService != null)
                {
                    foreach (var item in aService)
                    {
                        bizness.category_id = item.category_id;
                        bizness.ProviderID = item.ProviderID;
                        bizness.ServiceID = item.ServiceID;
                        bizness.ServiceDescription = item.ServiceDescription;
                        bizness.Profession = item.Profession;
                        bizness.Qualification = item.Qualification;
                        bizness.Status = item.Status;
                        bizness.StatusChangedBy = item.StatusChangedBy;
                        bizness.StatusChangedOn = item.StatusChangedOn;
                        bizness.userid = item.userid;
                    }
                }
                else
                {
                    bizness.userid = (int)Session["userid"];  //Current Logged In user
                }

                List<STATIC_FARMER_SERVICES> cServices = Db.STATIC_FARMER_SERVICES.Where(x => x.category_id == bizness.category_id).ToList();
                cServices.ForEach(x =>
                {
                    aServiceList.Add(new SelectListItem { Text = x.Description, Value = x.id.ToString() });
                });
            }
            else
            {
                aServiceList.Add(new SelectListItem { Text = "<<Select Service>>", Value = "0" });
            }

            bizness.ServicesProvidedList = entity;

            var baseid = (int)Session["BaseID"];
            bizness.CategoryList = aCategory;
            bizness.ServiceList = aServiceList;

            return View(bizness);
        }

        //Delete a Farmer Service Record
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DelProvideService(int id)
        {
            //update database
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var afamd = Db.SERVICE_PROVIDER.Where(s => s.ProviderID == id).FirstOrDefault();

                Db.SERVICE_PROVIDER.Remove(afamd);
                Db.SaveChanges();
                ViewBag.Message = "Record Successfully Deleted!!!";

                return Json(afamd);
            }

            return new EmptyResult();
        }
    }
}