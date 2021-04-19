using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoAddress.Models
{
    [RoutePrefix("api/Hosp")]
    public class HealthController : ApiController
    {
        [Route("All/{mUser}")]
        [HttpGet]
        public IHttpActionResult GetAll(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

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
                              where r.Category == "H" && (myrole.RoleID == 1 || r.UserID == mUser)
                              select new
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
                    return Content(HttpStatusCode.BadRequest, "No Educational Institution Defined in KE Google Plus Platform!!");
                }
            }
        }

        [Route("{mUser}")]
        [HttpGet]
        public IHttpActionResult GetMyRecords(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

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
                              where r.Category == "H" && r.UserID == mUser
                              select new
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
                    return Content(HttpStatusCode.BadRequest, "No Educational Institution Defined in KE Google Plus Platform!!");
                }
            }
        }
    }
}
