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
    public class HospitalController : Controller
    {
        //public string localpath = "http://localhost:801/";
        string ExtLocalPath = Settings.Default.ExtLocalPath;

        // GET: Hospital
        public ActionResult Index()
        {
            IEnumerable<HospViewModel> Hosp = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                if(Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Hosp/All/" + mUser.ToString());
                
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HospViewModel>>();
                    readTask.Wait();

                    Hosp = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    Hosp = Enumerable.Empty<HospViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(Hosp);
        }

        // GET: Hospital/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hospital/Create
        public ActionResult Create()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
           HospViewModel2 ViewModel = new HospViewModel2();

            List<SelectListItem> levelNames = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

            List<STATIC_HOSPITAL_LEVEL> clevel = Db.STATIC_HOSPITAL_LEVEL.ToList();
            clevel.ForEach(x =>
            {
                levelNames.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_ID.ToString() });
            });
            ViewModel.Levels = levelNames;
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

            return View(ViewModel);
            //return View();
        }

        // POST: Hospital/Create
        [HttpPost]
        public ActionResult Create(HospViewModel2 model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                    return View();
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
                {
                    var askul = model;
                    var maxId = Db.BaseTables.Max(x => (int?)x.BaseID) ?? 0;

                    int maxValue = maxId + 1;

                    Db.BaseTables.Add(new BaseTable()
                    {
                        BaseID = maxValue,
                        Latitude = askul.Latitude,
                        Longitude = askul.Longitude,
                        Pluscode = askul.Pluscode,
                        Address = askul.Address,
                        Category = "H",
                        UserID = mUser
                    });

                    Db.HEALTH_FACILITY.Add(new HEALTH_FACILITY()
                    {
                        BaseID = maxValue,
                        FacilityName = askul.FacilityName,
                        ContactPhone = askul.ContactPhone,
                        ContactEmail = askul.ContactEmail,
                        Website = askul.Website,
                        Building = askul.Building,
                        Level_ID = askul.Level_ID,
                        County_Code = askul.County_Code,
                        Constituency_Code = askul.Constituency_Code,
                        Sub_County_Code = askul.Sub_County_Code,
                        Ward_Code = askul.Ward_Code
                    });


                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        Db.SaveChanges();
                        return RedirectToAction("Index");
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
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hospital/Edit/5
        public ActionResult Edit(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HEALTH_FACILITY
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          where p.BaseID == id
                          select new HospViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              FacilityName = p.FacilityName,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              Level_ID = p.Level_ID,
                              County_Code = p.County_Code,
                              Constituency_Code = p.Constituency_Code,
                              Sub_County_Code = p.Sub_County_Code,
                              Ward_Code = p.Ward_Code,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode,
                              Address = r.Address
                          }).SingleOrDefault();

            List<SelectListItem> levelNames = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

            List<STATIC_HOSPITAL_LEVEL> clevels = Db.STATIC_HOSPITAL_LEVEL.ToList();
            clevels.ForEach(x =>
            {
                levelNames.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_ID.ToString() });
            });
            entity.Levels = levelNames;
            List<COUNTY> cties = Db.COUNTies.ToList();
            cties.ForEach(x =>
            {
                countyNames.Add(new SelectListItem { Text = x.County_Name, Value = x.County_Code.ToString() });
            });
            entity.CountyNames = countyNames;
            List<SUB_COUNTY> scties = (from p in Db.SUB_COUNTY
                                       select p).ToList();
            scties.ForEach(x =>
            {
                scountyNames.Add(new SelectListItem { Text = x.Sub_County_Name, Value = x.Sub_County_Code.ToString() });
            });
            entity.SubCountyNames = scountyNames;
            List<CONSTITUENCY> cons = (from p in Db.CONSTITUENCies
                                       select p).ToList();
            cons.ForEach(x =>
            {
                consNames.Add(new SelectListItem { Text = x.Constituency_Name, Value = x.Constituency_Code.ToString() });
            });
            entity.Constituencies = consNames;
            List<WARD> wads = (from p in Db.WARDs
                               select p).ToList();
            wads.ForEach(x =>
            {
                wardNames.Add(new SelectListItem { Text = x.Ward_Name, Value = x.Ward_Code.ToString() });
            });
            entity.Wards = wardNames;

            return View(entity);
            //return View();
        }

        // POST: Hospital/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HospViewModel myhosp)
        {
            try
            {
                // TODO: Add update logic here
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
                {
                    var bizna = myhosp;
                    //var query = (from p in Db.BaseTables
                    //            select p).ToList();
                    //var highestId = query.Any() ? query.Max(x => x.ID) : 0;

                    var abizna = Db.BaseTables.Where(s => s.BaseID == myhosp.BaseID).FirstOrDefault();
                    var mcreator = abizna.UserID;
                    Db.BaseTables.Remove(abizna);

                    var abizd = Db.HEALTH_FACILITY.Where(s => s.BaseID == myhosp.BaseID).FirstOrDefault();
                    Db.HEALTH_FACILITY.Remove(abizd);

                    int maxValue = myhosp.BaseID;

                    //maxValue += 1;

                    Db.BaseTables.Add(new BaseTable()
                    {
                        BaseID = maxValue,
                        Latitude = bizna.Latitude,
                        Longitude = bizna.Longitude,
                        Pluscode = bizna.Pluscode,
                        Address = bizna.Address,
                        Category = "H",
                        UserID = mcreator
                    });

                    Db.HEALTH_FACILITY.Add(new HEALTH_FACILITY()
                    {
                        BaseID = maxValue,
                        FacilityName = bizna.FacilityName,
                        Level_ID = bizna.Level_ID,
                        ContactPhone = bizna.ContactPhone,
                        ContactEmail = bizna.ContactEmail,
                        Website = bizna.Website,
                        Building = bizna.Building,
                        County_Code = bizna.County_Code,
                        Constituency_Code = bizna.Constituency_Code,
                        Sub_County_Code = bizna.Sub_County_Code,
                        Ward_Code = bizna.Ward_Code
                    });


                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        Db.SaveChanges();
                        return RedirectToAction("Index");
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
                }
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hospital/Delete/5
        public ActionResult Delete(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HEALTH_FACILITY
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.STATIC_HOSPITAL_LEVEL on p.Level_ID equals w.Level_ID
                          join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                          join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                          join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                          join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                          from cty in ctydb.DefaultIfEmpty()
                          from scty in sctydb.DefaultIfEmpty()
                          from cons in consdb.DefaultIfEmpty()
                          from wds in wdsdb.DefaultIfEmpty()
                          where p.BaseID == id
                          select new HospViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              FacilityName = p.FacilityName,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              Level_ID = w.Level_Name,
                              County_Code = cty.County_Name,
                              Constituency_Code = cons.Constituency_Name,
                              Sub_County_Code = scty.Sub_County_Name,
                              Ward_Code = wds.Ward_Name,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode,
                              Address = r.Address
                          }).SingleOrDefault();
            
            return View(entity);
            //return View();
        }

        // POST: Hospital/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // TODO: Add delete logic here
            
            try
            {
                // TODO: Add delete logic here
                using (var ctx = new KEGooglePlusEntities())
                {
                    var abizna = ctx.BaseTables
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();

                    ctx.Entry(abizna).State = System.Data.Entity.EntityState.Deleted;

                    var abisd = ctx.HEALTH_FACILITY
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();
                    ctx.Entry(abisd).State = System.Data.Entity.EntityState.Deleted;

                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
