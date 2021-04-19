using GeoAddress.Models;
using GeoAddress.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GeoAddress.Controllers
{
    [Authorize]
    public class BizController : Controller
    {
        //public string localpath = "http://localhost:801/";
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        // GET: Biz
        public ActionResult Index()
        {
            IEnumerable<BizViewModel> bizness = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
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
                var responseTask = client.GetAsync( ExtLocalPath + "api/Biz/All/" + mUser.ToString());
                
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BizViewModel>>();
                    readTask.Wait();

                    bizness = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bizness = Enumerable.Empty<BizViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bizness);
        }

        public ActionResult Create()
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();
            BizViewModel2 ViewModel = new BizViewModel2();
            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

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


        [HttpPost]
        public ActionResult Create(BizViewModel2 model)
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
                //var query = (from p in Db.BaseTables
                //            select p).ToList();
                //var highestId = query.Any() ? query.Max(x => x.ID) : 0;
                var maxId = Db.BaseTables.Max(x => (int?)x.BaseID) ?? 0;

                int maxValue = maxId + 1;

                //maxValue += 1;

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode,
                    Address = bizna.Address,
                    Category = "B",
                    UserID = mUser
                });

                Db.BUSINESSes.Add(new BUSINESS()
                {
                    BaseID = maxValue,
                    BusinessCategory = 0,
                    BusinessName = bizna.BusinessName,
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

            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            KEGooglePlusEntities Db = new KEGooglePlusEntities();

            var entity = (from p in Db.BUSINESSes
                          join r in Db.BaseTables on p.BaseID equals r.BaseID
                          where p.BaseID == id
                          select new BizViewModel
                          { // result selector 
                              BaseID = p.BaseID,
                              BusinessName = p.BusinessName,
                              ContactPhone = p.ContactPhone,
                              ContactEmail = p.ContactEmail,
                              Website = p.Website,
                              Building = p.Building,
                              County_Code = p.County_Code,
                              Constituency_Code = p.Constituency_Code,
                              Sub_County_Code = p.Sub_County_Code,
                              Ward_Code = p.Ward_Code,
                              Latitude = r.Latitude,
                              Longitude = r.Longitude,
                              Pluscode = r.Pluscode,
                              Address = r.Address
                          }).SingleOrDefault();

            List<SelectListItem> countyNames = new List<SelectListItem>();
            List<SelectListItem> scountyNames = new List<SelectListItem>();
            List<SelectListItem> consNames = new List<SelectListItem>();
            List<SelectListItem> wardNames = new List<SelectListItem>();

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
        [HttpPost]
        public ActionResult Edit(BizViewModel mybiz)
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

                var abizna = Db.BaseTables.Where(s => s.BaseID == mybiz.BaseID).FirstOrDefault();
                var mcreator = abizna.UserID;
                Db.BaseTables.Remove(abizna);

                var abizd = Db.BUSINESSes.Where(s => s.BaseID == mybiz.BaseID).FirstOrDefault();
                Db.BUSINESSes.Remove(abizd);

                int maxValue = mybiz.BaseID;

                //maxValue += 1;

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode,
                    Address = bizna.Address,
                    Category = "B",
                    UserID = mcreator
                });

                Db.BUSINESSes.Add(new BUSINESS()
                {
                    BaseID = maxValue,
                    BusinessCategory = 0,
                    BusinessName = bizna.BusinessName,
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
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            using (var ctx = new KEGooglePlusEntities())
            {
                var abizna = ctx.BaseTables
                    .Where(s => s.BaseID == id)
                    .FirstOrDefault();

                ctx.Entry(abizna).State = System.Data.Entity.EntityState.Deleted;

                var abisd = ctx.BUSINESSes
                    .Where(s => s.BaseID == id)
                    .FirstOrDefault();
                ctx.Entry(abisd).State = System.Data.Entity.EntityState.Deleted;

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}