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
    public class HseController : Controller
    {
        //public string localpath = "http://localhost:801/";
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        // GET: Hse
        public ActionResult Index()
        {
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            IEnumerable<HseViewModel> bizness = null;
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];
            

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Hse/All/" + mUser.ToString());
                
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
            //return View();
        }

        // GET: Hse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hse/Create
        public ActionResult Create()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            HseViewModel2 ViewModel = new HseViewModel2();
            List<SelectListItem> hseTypes = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

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

            return View(ViewModel);
            //return View();
        }

        // POST: Hse/Create
        [HttpPost]
        public ActionResult Create(HseViewModel2 model)
        {
            if (!ModelState.IsValid)
                return View();
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

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode,
                    Address = bizna.Address,
                    Category = "P",
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
                    NoWorkers = bizna.NoWorkers
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

            return View();
        }

        // GET: Hse/Edit/5
        public ActionResult Edit(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          where p.BaseID == id
                          select new HseViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              HouseHoldName = p.HouseHoldName,
                              HouseHoldTypeId = p.HouseHoldTypeId.ToString(),
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
                              Address = r.Address
                          }).SingleOrDefault();

            List<SelectListItem> hseTypes = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();
            List<STATIC_HOUSEHOLD_TYPE> ctypes = Db.STATIC_HOUSEHOLD_TYPE.ToList();
            ctypes.ForEach( x=>
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

        // POST: Hse/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HseViewModel mybiz)
        {
            if (!ModelState.IsValid)
                return View();
            if (Session["user"] == null)
            {
                this.RedirectToAction("LogOff", "Account");
            }
            var mUser = (string)Session["user"];

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var bizna = mybiz;
                //var query = (from p in Db.BaseTables
                //            select p).ToList();
                //var highestId = query.Any() ? query.Max(x => x.ID) : 0;

                var abizna = Db.BaseTables.Where(s => s.BaseID == id).FirstOrDefault();
                var mcreator = abizna.UserID;
                Db.BaseTables.Remove(abizna);

                var abizd = Db.HOUSEHOLDS.Where(s => s.BaseID == id).FirstOrDefault();
                Db.HOUSEHOLDS.Remove(abizd);

                int maxValue = mybiz.BaseID;

                //maxValue += 1;

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode,
                    Address = bizna.Address,
                    Category = "P",
                    UserID = mcreator
                });

                Db.HOUSEHOLDS.Add(new HOUSEHOLD()
                {
                    BaseID = maxValue,
                    HouseHoldTypeId =Int32.Parse(bizna.HouseHoldTypeId),
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
                    NoWorkers = bizna.NoWorkers
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

        // GET: Hse/Delete/5
        public ActionResult Delete(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.HOUSEHOLDS
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.STATIC_HOUSEHOLD_TYPE on p.HouseHoldTypeId equals w.HouseHoldTypeID
                          join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                          join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                          join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                          join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                          from cty in ctydb.DefaultIfEmpty()
                          from scty in sctydb.DefaultIfEmpty()
                          from cons in consdb.DefaultIfEmpty()
                          from wds in wdsdb.DefaultIfEmpty()
                          where p.BaseID == id
                          select new HseViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              HouseHoldName = p.HouseHoldName,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              Village = p.Village,
                              LandRegistration = p.LandRegistration,
                              NoWorkers = (int)p.NoWorkers,
                              HouseHoldTypeId = w.HouseHoldType,
                              County_Code = cty.County_Name,
                              Constituency_Code = cons.Constituency_Name,
                              Sub_County_Code = scty.Sub_County_Name,
                              Ward_Code = wds.Ward_Name,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode
                          }).SingleOrDefault();

            return View(entity);
        }

        // POST: Hse/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
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
