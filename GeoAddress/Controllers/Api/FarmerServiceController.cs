using GeoAddress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoAddress.Controllers.Api
{

    [RoutePrefix("api/FarmerService")]
    public class FarmerServiceController : ApiController
    {

        [HttpGet]
        [Route("{mUser}")]
        public IHttpActionResult GetMyRecords(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

                var entity = (from p in Db.vw_Farmer_Service_Requests
                              where p.UserID == mUser
                              select new
                              { // result selector 
                                  BaseID = p.BaseID,
                                  ServiceRunID = p.ServiceRunID,
                                  ServiceID = p.ServiceID,
                                  UserID = p.UserID,
                                  Description = p.Description,
                                  CreatedOn = p.CreatedOn,
                                  ServiceSolved = p.ServiceSolved,
                                  Published = p.Published,
                                  ServiceDescription = p.ServiceDescription,
                                  CategoryID = p.CategoryID,
                                  CategoryDescription = p.CategoryDescription
                              }).ToArray();

                if (entity != null)
                {
                    try
                    {
                        // Respond to JSON requests
                        return Content(HttpStatusCode.OK, entity, Configuration.Formatters.JsonFormatter);
                    }
                    catch
                    {
                        return Content(HttpStatusCode.BadRequest, entity);
                    }
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "No Farmer Service Defined in this Platform!!");
                }
            }
        }
    }
}
