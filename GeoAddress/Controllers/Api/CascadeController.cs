using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoAddress.Models
{
    [RoutePrefix("api/Cascade")]
    public class CascadeController : ApiController
    {
        [HttpGet]
        [Route("Counties")]
        public IHttpActionResult GetCounties()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.COUNTies
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Counties Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("SubCounties/{id}")]
        public IHttpActionResult GetSubCounties(string id)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.SUB_COUNTY
                              where p.County_Code == id
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Sub-Counties for County Code =" + id + " Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("Constituencies/{id}")]
        public IHttpActionResult GetConstituencies(string id)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.CONSTITUENCies
                              where p.County_Code == id
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Constituencies for County Code =" + id + " Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("Wards/{id}")]
        public IHttpActionResult GetWards(string id)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.WARDs
                              where p.Constituency_Code == id
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Wards for Constituency Code =" + id + " Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("Levels")]
        public IHttpActionResult GetLevels()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_SCHOOL_LEVEL
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No School Levels in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("TrafficType")]
        public IHttpActionResult GetTrafficType()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_TRAFFIC_DEFINITION
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Traffic Definitions in KE Google Plus Platform!!");
                }
            }
        }
        
        [HttpGet]
        [Route("HospLevels")]
        public IHttpActionResult GetHospLevels()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_HOSPITAL_LEVEL
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Hospital Levels in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("AgriCategory")]
        public IHttpActionResult GetAgriCategory()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_AGRICULTURE_CATEGORY
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Agriculture Categories Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("AgriType")]
        public IHttpActionResult GetAgriType()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_AGRICULTURE_TYPE
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Agriculture Types Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("AgriItem/{id}")]
        public IHttpActionResult GetAgriItem(int id)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_AGRICULTURE_ITEM
                              where p.TypeID == id
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Agriculture Item for a given Type {"+id.ToString()+"} Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("FarmerServiceCategories")]
        public IHttpActionResult GetFarmerServiceCategories()
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_FARMER_SERVICES_CATEGORY
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Farmer Service Category Defined in KE Google Plus Platform!!");
                }
            }
        }

        [HttpGet]
        [Route("FarmerServices/{id}")]
        public IHttpActionResult GetFarmerService(int id)
        {
            using (KEGooglePlusEntities Db = new KEGooglePlusEntities())
            {
                var entity = (from p in Db.STATIC_FARMER_SERVICES
                              where p.category_id == id
                              select p).ToArray();
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
                    return Content(HttpStatusCode.BadRequest, "No Farmer Services Defined in KE Google Plus Platform!!");
                }
            }
        }
    }
}
