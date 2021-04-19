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
    public class SkulController : Controller
    {
        //public string localpath = "http://localhost:801/";
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        // GET: Skul
        public ActionResult Index()
        {
            IEnumerable<SkulViewModel> skuls = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                //LoginByUsernamePassword_Result objectName = (LoginByUsernamePassword_Result)Session["user"];
                var mUser = (string)Session["user"];
                client.BaseAddress = new Uri(localpath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync( ExtLocalPath + "api/Skul/All/" + mUser.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SkulViewModel>>();
                    readTask.Wait();

                    skuls = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    skuls = Enumerable.Empty<SkulViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(skuls);
            //return View();
        }

        // GET: Skul/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Skul/Create
        public ActionResult Create()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            SkulViewModel2 ViewModel = new SkulViewModel2();

            List<SelectListItem> levelNames = new List<SelectListItem>();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

            List<STATIC_SCHOOL_LEVEL> clevel = Db.STATIC_SCHOOL_LEVEL.ToList();
            clevel.ForEach(x =>
            {
                levelNames.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
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
        }

        // POST: Skul/Create
        [HttpPost]
        public ActionResult Create(SkulViewModel2 model)
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
                        Category = "S",
                        UserID = mUser
                    });

                    Db.SCHOOLs.Add(new SCHOOL()
                    {
                        BaseID = maxValue,
                        NEMIS_CODE = askul.NEMIS_CODE,
                        INSTITUTION_NAME = askul.INSTITUTION_NAME,
                        Level_Code = askul.Level_Code,
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
            }
            catch
            {
                return View();
            }
        }

        // GET: Skul/Edit/5
        public ActionResult Edit(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.SCHOOLs
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          where p.BaseID == id
                          select new SkulViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              NEMIS_CODE = p.NEMIS_CODE,
                              INSTITUTION_NAME = p.INSTITUTION_NAME,
                              Level_Code = p.Level_Code,
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

            List<STATIC_SCHOOL_LEVEL> clevel = Db.STATIC_SCHOOL_LEVEL.ToList();
            clevel.ForEach(x =>
            {
                levelNames.Add(new SelectListItem { Text = x.Level_Name, Value = x.Level_Code.ToString() });
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
        }

        // POST: Skul/Edit/5
        [HttpPost]
        public ActionResult Edit(SkulViewModel myskul)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                    return View();
                if (Session["user"] == null)
                {
                    this.RedirectToAction("LogOff", "Account");
                }
                var mUser = (string)Session["user"];

                using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
                {
                    var bizna = myskul;
                    var abizna = Db.BaseTables.Where(s => s.BaseID == myskul.BaseID).FirstOrDefault();
                    var mcreator = abizna.UserID;
                    Db.BaseTables.Remove(abizna);

                    var askuld = Db.SCHOOLs.Where(s => s.BaseID == myskul.BaseID).FirstOrDefault();
                    Db.SCHOOLs.Remove(askuld);

                    int maxValue = myskul.BaseID;
                    
                    Db.BaseTables.Add(new BaseTable()
                    {
                        BaseID = maxValue,
                        Latitude = bizna.Latitude,
                        Longitude = bizna.Longitude,
                        Pluscode = bizna.Pluscode,
                        Address = bizna.Address,
                        Category = "S",
                        UserID = mcreator
                    });

                    Db.SCHOOLs.Add(new SCHOOL()
                    {
                        BaseID = maxValue,
                        NEMIS_CODE = bizna.NEMIS_CODE,
                        INSTITUTION_NAME = bizna.INSTITUTION_NAME,
                        Level_Code = bizna.Level_Code,
                        County_Code = bizna.County_Code,
                        Constituency_Code = bizna.Constituency_Code,
                        Sub_County_Code = bizna.Sub_County_Code,
                        Ward_Code = bizna.Ward_Code
                    });
                    
                    try
                    {
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
            }
            catch
            {
                return View();
            }
        }

        // GET: Skul/Delete/5
        public ActionResult Delete(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.SCHOOLs
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          join w in Db.STATIC_SCHOOL_LEVEL on p.Level_Code equals w.Level_Code
                          join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                          join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                          join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                          join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                          from cty in ctydb.DefaultIfEmpty()
                          from scty in sctydb.DefaultIfEmpty()
                          from cons in consdb.DefaultIfEmpty()
                          from wds in wdsdb.DefaultIfEmpty()
                          where p.BaseID == id && r.Category == "S"
                          select new SkulViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              NEMIS_CODE = p.NEMIS_CODE,
                              INSTITUTION_NAME = p.INSTITUTION_NAME,
                              Level_Code = w.Level_Name,
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
        }

        // POST: Skul/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                if (id <= 0)
                    return RedirectToAction("Index");

                using (var ctx = new KEGooglePlusEntities())
                {
                    var abizna = ctx.BaseTables
                        .Where(s => s.BaseID == id)
                        .FirstOrDefault();

                    ctx.Entry(abizna).State = System.Data.Entity.EntityState.Deleted;

                    var abisd = ctx.SCHOOLs
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
