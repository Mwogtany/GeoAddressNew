using GeoAddress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoAddress.Controllers.Api
{
    [RoutePrefix("api/Farmer")]
    public class FarmerController : ApiController
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

                var entity = (from p in Db.HOUSEHOLDS
                              join r in Db.BaseTables on p.BaseID equals r.BaseID
                              join z in Db.FARMERs on p.BaseID equals z.BaseID
                              join w in Db.STATIC_HOUSEHOLD_TYPE on p.HouseHoldTypeId equals w.HouseHoldTypeID
                              join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                              join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                              join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                              join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                              join m in Db.STATIC_AGRICULTURE_CATEGORY on z.CategoryID equals m.CategoryID
                              join n in Db.STATIC_AGRICULTURE_TYPE on z.TypeID equals n.TypeID
                              join k in Db.STATIC_AGRICULTURE_ITEM on z.ItemID equals k.ItemID
                              join s in Db.STATIC_FARMER_LAND_REGISTRATION on p.LandRegistration equals s.StatusID.ToString()
                              from cty in ctydb.DefaultIfEmpty()
                              from scty in sctydb.DefaultIfEmpty()
                              from cons in consdb.DefaultIfEmpty()
                              from wds in wdsdb.DefaultIfEmpty()
                              where r.Category == "F" && (r.UserID == mUser || myrole.RoleID == 1)
                              select new
                              { // result selector 
                                  BaseID = p.BaseID,
                                  HouseHoldName = p.HouseHoldName,
                                  HouseHoldTypeId = w.HouseHoldType,
                                  ContactPhone = p.ContactPhone,
                                  ContactEmail = p.ContactEmail,
                                  Website = p.Website,
                                  Building = p.Building,
                                  County_Code = cty.County_Name,
                                  Constituency_Code = cons.Constituency_Name,
                                  Sub_County_Code = scty.Sub_County_Name,
                                  Ward_Code = wds.Ward_Name,
                                  Village = p.Village,
                                  LandRegistration = p.LandRegistration,
                                  NoWorkers = p.NoWorkers,
                                  Latitude = r.Latitude,
                                  Longitude = r.Longitude,
                                  Pluscode = r.Pluscode,
                                  Address = r.Address,
                                  CategoryID = m.Category,
                                  TypeID = n.TypeDescription,
                                  ItemID = k.ItemDescription,
                                  LandRegistrationDesc = s.StatusDescription
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
                    return Content(HttpStatusCode.BadRequest, "No Farmer Defined in KE Google Plus Platform!!");
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

                var entity = (from p in Db.HOUSEHOLDS
                              join r in Db.BaseTables on p.BaseID equals r.BaseID
                              join z in Db.FARMERs on p.BaseID equals z.BaseID
                              join w in Db.STATIC_HOUSEHOLD_TYPE on p.HouseHoldTypeId equals w.HouseHoldTypeID
                              join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                              join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                              join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                              join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                              join m in Db.STATIC_AGRICULTURE_CATEGORY on z.CategoryID equals m.CategoryID
                              join n in Db.STATIC_AGRICULTURE_TYPE on z.TypeID equals n.TypeID
                              join k in Db.STATIC_AGRICULTURE_ITEM on z.ItemID equals k.ItemID
                              join s in Db.STATIC_FARMER_LAND_REGISTRATION on p.LandRegistration equals s.StatusID.ToString()
                              from cty in ctydb.DefaultIfEmpty()
                              from scty in sctydb.DefaultIfEmpty()
                              from cons in consdb.DefaultIfEmpty()
                              from wds in wdsdb.DefaultIfEmpty()
                              where r.Category == "F" && r.UserID == mUser
                              select new
                              { // result selector 
                                  BaseID = p.BaseID,
                                  HouseHoldName = p.HouseHoldName,
                                  HouseHoldTypeId = w.HouseHoldType,
                                  ContactPhone = p.ContactPhone,
                                  ContactEmail = p.ContactEmail,
                                  Website = p.Website,
                                  Building = p.Building,
                                  County_Code = cty.County_Name,
                                  Constituency_Code = cons.Constituency_Name,
                                  Sub_County_Code = scty.Sub_County_Name,
                                  Ward_Code = wds.Ward_Name,
                                  Village = p.Village,
                                  LandRegistration = p.LandRegistration,
                                  NoWorkers = p.NoWorkers,
                                  Latitude = r.Latitude,
                                  Longitude = r.Longitude,
                                  Pluscode = r.Pluscode,
                                  Address = r.Address,
                                  CategoryID = m.Category,
                                  TypeID = n.TypeDescription,
                                  ItemID = k.ItemDescription,
                                  LandRegistrationDesc = s.StatusDescription
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
                    return Content(HttpStatusCode.BadRequest, "No Farmer Defined in KE Google Plus Platform!!");
                }
            }
        }
    }
}
