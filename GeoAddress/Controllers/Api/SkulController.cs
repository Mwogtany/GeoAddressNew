using GeoAddress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoAddress.Controllers.Api
{
    [RoutePrefix("api/Skul")]
    public class SkulController : ApiController
    {
        [HttpGet]
        [Route("All/{mUser}")]
        public IHttpActionResult GetAll(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

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
                              where r.Category == "S" && (myrole.RoleID == 1 || r.UserID == mUser)
                              select new
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

        [HttpGet]
        [Route("{mUser}")]
        public IHttpActionResult GetMyRecords(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

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
                              where r.Category == "S" &&  r.UserID == mUser
                              select new
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

        [HttpPost]
        [Route("NewSchool")]
        public IHttpActionResult PostNewSchool(SkulViewModel bizna)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var askuld = Db.SCHOOLs.Where(s => s.NEMIS_CODE == bizna.NEMIS_CODE).FirstOrDefault();
                if (askuld != null)
                {
                    var CurID = askuld.BaseID;
                    var abizna = Db.BaseTables.Where(s => s.BaseID == CurID).FirstOrDefault();

                    abizna.Latitude = bizna.Latitude;
                    abizna.Longitude = bizna.Longitude;
                    abizna.Pluscode = bizna.Pluscode;
                    abizna.Address = bizna.Address;

                    askuld.INSTITUTION_NAME = bizna.INSTITUTION_NAME;
                    askuld.County_Code = bizna.County_Code;
                    askuld.Sub_County_Code = bizna.Sub_County_Code;
                    askuld.Ward_Code = bizna.Ward_Code;
                    askuld.Constituency_Code = bizna.Constituency_Code;
                    askuld.Level_Code = bizna.Level_Code;
                }
                else
                {
                    var maxValue = Db.BaseTables.Max(x => x.BaseID);
                    maxValue += 1;

                    Db.BaseTables.Add(new BaseTable()
                    {
                        BaseID = maxValue,
                        Latitude = bizna.Latitude,
                        Longitude = bizna.Longitude,
                        Pluscode = bizna.Pluscode,
                        Address = bizna.Address,
                        Category = "S"
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
                }

                Db.SaveChanges();
            }

            return Ok();
        }


    }
}
