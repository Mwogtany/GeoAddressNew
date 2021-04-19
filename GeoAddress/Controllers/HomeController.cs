using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// <copyright file="HomeController.cs" company="pluscodesafrica.org">  
// Copyright (c) Allow to distribute this code.   
// </copyright>  
// <author>Barnabas Sang</author>  
//----------------------------------------------------------------------- 
namespace GeoAddress.Controllers
{
    /// <summary>  
    /// Home controller class.   
    /// </summary>  
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        [Route("")]
        #region Index method.  
        /// <summary>  
        /// Index method.   
        /// </summary>  
        /// <returns>Returns - Index view</returns>  
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        #endregion

        [Route("pluscode")]
        #region WhatIsPluscode method.  
        /// <summary>  
        /// WhatIsPluscode method.   
        /// </summary>  
        /// <returns>Returns - WhatIsPluscode view</returns>  
        public ActionResult WhatIsPluscode()
        {
            ViewBag.Title = "Pluscodes";

            return View();
        }
        #endregion
    }
}
