using GeoAddress.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace GeoAddress
{
    public class Global : System.Web.HttpApplication
    {
        private string _extLocalPath; // the global private variable
        internal string ExtLocalPath // the global controlled variable
        {
            get
            {
                if (String.IsNullOrEmpty(_extLocalPath))
                {
                    string ExtLocalPath = Settings.Default.ExtLocalPath;
                    _extLocalPath = ExtLocalPath;
                }
                return _extLocalPath;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["ExtLocalPath"] = ExtLocalPath;// the global variable's bed

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Fixing claim issue. https://stack247.wordpress.com/2013/02/22/antiforgerytoken-a-claim-of-type-nameidentifier-or-identityprovider-was-not-present-on-provided-claimsidentity/   
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}