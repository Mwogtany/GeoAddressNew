using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GeoAddress.Models
{
    [RoutePrefix("api/Biz")]
    public class BizController : ApiController
    {
        [HttpGet]
        [Route("{mUser}")]
        public IHttpActionResult GetMyRecord(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

                var entity = (from p in Db.vw_Businesses
                              where p.UserID == mUser //|| myrole.RoleID == 1
                              select new
                              { // result selector 
                                  BaseID = p.BaseID,
                                  BusinessName = p.BusinessName,
                                  ContactPhone = p.ContactPhone,
                                  ContactEmail = p.ContactEmail,
                                  Website = p.Website,
                                  Building = p.Building,
                                  County_Code = p.County_Name,
                                  Constituency_Code = p.Constituency_Name,
                                  Sub_County_Code = p.Sub_County_Name,
                                  Ward_Code = p.Ward_Name,
                                  Latitude = p.Latitude,
                                  Longitude = p.Longitude,
                                  Pluscode = p.Pluscode,
                                  Address = p.Address
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
                    return Content(HttpStatusCode.BadRequest, "No Businesses Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("All/{mUser}")]
        public IHttpActionResult GetAll(string mUser)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var myrole = (from m in Db.UserRoleAssignments
                              where m.UserID == mUser
                              select m).SingleOrDefault();

                //var entity = (from p in Db.BUSINESSes
                //              join r in Db.BaseTables on p.BaseID equals r.BaseID
                //              join cty in Db.COUNTies on p.County_Code equals cty.County_Code into ctydb
                //              join scty in Db.SUB_COUNTY on p.Sub_County_Code equals scty.Sub_County_Code into sctydb
                //              join cons in Db.CONSTITUENCies on p.Constituency_Code equals cons.Constituency_Code into consdb
                //              join wds in Db.WARDs on p.Ward_Code equals wds.Ward_Code into wdsdb
                //              from cty in ctydb.DefaultIfEmpty()
                //              from scty in sctydb.DefaultIfEmpty()
                //              from cons in consdb.DefaultIfEmpty()
                //              from wds in wdsdb.DefaultIfEmpty()
                //              where r.Category == "B" && (myrole.RoleID == 1 || r.UserID == mUser)
                //              select new 
                //            { // result selector 
                //                BaseID = p.BaseID,
                //                BusinessName = p.BusinessName,
                //                ContactPhone = p.ContactPhone,
                //                ContactEmail = p.ContactEmail,
                //                Website = p.Website,
                //                Building = p.Building,
                //                County_Code = cty.County_Name,
                //                Constituency_Code = cons.Constituency_Name,
                //                Sub_County_Code = scty.Sub_County_Name,
                //                Ward_Code = wds.Ward_Name,
                //                Latitude = r.Latitude,
                //                Longitude = r.Longitude,
                //                  Pluscode = r.Pluscode,
                //                  Address = r.Address
                //              }).ToArray();

                var entity = (from p in Db.vw_Businesses
                              where p.UserID == mUser || myrole.RoleID == 1
                              select new
                              { // result selector 
                                  BaseID = p.BaseID,
                                  BusinessName = p.BusinessName,
                                  ContactPhone = p.ContactPhone,
                                  ContactEmail = p.ContactEmail,
                                  Website = p.Website,
                                  Building = p.Building,
                                  County_Code = p.County_Name,
                                  Constituency_Code = p.Constituency_Name,
                                  Sub_County_Code = p.Sub_County_Name,
                                  Ward_Code = p.Ward_Name,
                                  Latitude = p.Latitude,
                                  Longitude = p.Longitude,
                                  Pluscode = p.Pluscode,
                                  Address = p.Address
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
                    return Content(HttpStatusCode.BadRequest, "No Businesses Defined in KE Google Plus Platform!!");
                }
            }
        }

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //}
        [HttpPost]
        [Route("NewBusiness")]
        public IHttpActionResult PostNewBusiness(BizViewModel bizna)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {

                var maxValue = Db.BaseTables.Max(x => x.BaseID);
                maxValue += 1;

                Db.BaseTables.Add(new BaseTable()
                {
                    BaseID = maxValue,
                    Latitude = bizna.Latitude,
                    Longitude = bizna.Longitude,
                    Pluscode = bizna.Pluscode
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

                Db.SaveChanges();
            }

            return Ok();
        }

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{

        //}
        public IHttpActionResult Put(BUSINESS abizna)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var existingBiz = Db.BUSINESSes.Where(s => s.BaseID == abizna.BaseID).FirstOrDefault<BUSINESS>();

                if (existingBiz != null)
                {
                    existingBiz.BusinessCategory = abizna.BusinessCategory;
                    existingBiz.BusinessName = abizna.BusinessName;
                    existingBiz.ContactPhone = abizna.ContactPhone;
                    existingBiz.ContactEmail = abizna.ContactEmail;
                    existingBiz.Website = abizna.Website;
                    existingBiz.Building = abizna.Building;
                    existingBiz.County_Code = abizna.County_Code;
                    existingBiz.Constituency_Code = abizna.Constituency_Code;
                    existingBiz.Sub_County_Code = abizna.Sub_County_Code;
                    existingBiz.Ward_Code = abizna.Ward_Code;

                    Db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok();
        }
        // DELETE api/values/5
        //public void Delete(int id)
        //{

        //}
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Business id");

            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var bizna = Db.BUSINESSes
                    .Where(s => s.BaseID == id)
                    .FirstOrDefault();

                Db.Entry(bizna).State = System.Data.Entity.EntityState.Deleted;
                Db.SaveChanges();
            }

            return Ok();
        }
    }
}
