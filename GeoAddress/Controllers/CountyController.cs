using GeoAddress.Models;
using GeoAddress.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace GeoAddress.Controllers
{
    public class CountyController : Controller
    {
        // GET: County
        string ExtLocalPath = Settings.Default.ExtLocalPath;
        public ActionResult Index()
        {
            IEnumerable<COUNTY> county = null;
            string localpath = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localpath + ExtLocalPath + "api/cascade/Counties");
                //HTTP GET
                var responseTask = client.GetAsync("Counties");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<COUNTY>>();
                    readTask.Wait();

                    county = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    county = Enumerable.Empty<COUNTY>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(county);
            //return View();
        }

        // GET: County/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: County/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: County/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: County/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: County/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: County/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: County/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
