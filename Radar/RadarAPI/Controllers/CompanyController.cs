using Geocoding;
using Geocoding.Google;
using RadarAPI.Attributes;
using RadarBAL.ORM;
using RadarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RadarAPI.Controllers
{
    [RoutePrefix("api/companies")]
    public class CompanyController : ApiController
    {
        #region UNITOFWORK
        private UnitOfWork _adapter = null;
        protected UnitOfWork Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    _adapter = new UnitOfWork();
                }
                return _adapter;
            }
        }
        #endregion

        [Route("")]
        public List<Company> Get()
        {
            List<Company> comps = Adapter.CompanyRepository.GetAll().Where(c => c.DeletedDate == null).OrderBy(c => c.Name).ToList();
            return comps;
        }

        [Route("{id:int}")]
        public Company One(int id)
        {
            Company comp = Adapter.CompanyRepository.GetByID(id);
            if (comp == null || comp.DeletedDate != null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comp;
        }

        [Route("{name:alpha}")]
        public List<Company> Search(String name)
        {
            List<Company> comps = Adapter.CompanyRepository.Find(c => c.Name.ToLower().Contains(name.ToLower()) && c.DeletedDate == null, "").OrderBy(c => c.Name).ToList();
            if (comps == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comps;
        }


        [HttpPost, Route("")]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage Insert(Company comp)
        {
            var l = new RadarModels.Location();
            l.Street = comp.Location.Street;
            l.Number = comp.Location.Number;
            l.Box = comp.Location.Box;
            l.Zipcode = comp.Location.Zipcode;
            l.City = comp.Location.City;
            l.Country = comp.Location.Country;
            IGeocoder geocoder = new GoogleGeocoder();
            Address[] addresses = geocoder.Geocode(l.Street + " " + l.Number + ", " + l.Zipcode + " " + l.City + ", " + l.Country).ToArray();
            if (addresses.Length != 0 && addresses[0].Coordinates != null)
            {
                l.Latitude = Convert.ToDecimal(addresses[0].Coordinates.Latitude);
                l.Longitude = Convert.ToDecimal(addresses[0].Coordinates.Longitude);

                Adapter.LocationRepository.Insert(l);
                Adapter.Save();
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception("Address not found"));

            comp.Location = l;
            comp.CreatedDate = DateTime.Now;
            Adapter.CompanyRepository.Insert(comp);
            Adapter.Save();

            Employee emp = new Employee();
            emp.CompanyId = comp.CompanyId;
            emp.UserId = comp.UserId;
            emp.CreatedDate = DateTime.Now;
            emp.RoleId = Adapter.RoleRepository.GetByID(4).RoleId;
            Adapter.EmployeeRepository.Insert(emp);
            Adapter.Save();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut, Route("{id:int}")]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage Update(int id, [FromBody]Company comp)
        {
            if (ModelState.IsValid && id == comp.CompanyId)
            {
                var com = Adapter.CompanyRepository.GetByID(id);
                Adapter.CompanyRepository.UpdateValues(comp, com);
                IGeocoder geocoder = new GoogleGeocoder();
                Address[] addresses = geocoder.Geocode(com.Location.Street + " " + com.Location.Number + ", " + com.Location.Zipcode + " " + com.Location.City + ", " + com.Location.Country).ToArray();
                if (addresses.Length != 0 && addresses[0].Coordinates != null)
                {
                    com.Location.Latitude = Convert.ToDecimal(addresses[0].Coordinates.Latitude);
                    com.Location.Longitude = Convert.ToDecimal(addresses[0].Coordinates.Longitude);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception("Address not found"));
                com.ModifiedDate = DateTime.Now;
                Adapter.CompanyRepository.Update(com);
                Adapter.Save();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [HttpDelete, Route("{id:int}")]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage Delete(int id)
        {
            Company comp = Adapter.CompanyRepository.GetByID(id);
            if (comp != null)
            {
                comp.DeletedDate = DateTime.Now;
                Adapter.CompanyRepository.Update(comp);
                Adapter.Save();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        [HttpPost, Route("posts/create")]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage CreatePost([FromBody] Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;
                Adapter.PostRepository.Insert(post);
                Adapter.Save();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }
    }
}
