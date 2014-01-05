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
            List<Company> comps = Adapter.CompanyRepository.GetAll().OrderBy(c => c.Name).ToList();
            return comps;
        }

        [Route("{id:int}")]
        public Company One(int id)
        {
            Company comp = Adapter.CompanyRepository.GetByID(id);
            if (comp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comp;
        }

        [Route("{name:alpha}")]
        public List<Company> Search(String name)
        {
            List<Company> comps = Adapter.CompanyRepository.Find(c => c.Name.ToLower().Contains(name.ToLower()), "").OrderBy(c => c.Name).ToList();
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
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut, Route("{id:int}")]
        [Authorize]
        public HttpStatusCode Update(int id, Company comp)
        {
            if (ModelState.IsValid && id == comp.CompanyId)
            {
                comp.ModifiedDate = DateTime.Now;
                Adapter.CompanyRepository.Update(comp);
                Adapter.Save();
                return HttpStatusCode.OK;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [HttpDelete, Route("{id:int}")]
        [Authorize]
        public HttpStatusCode Delete(int id)
        {
            Company comp = Adapter.CompanyRepository.GetByID(id);
            if (comp != null)
            {
                comp.DeletedDate = DateTime.Now;
                Adapter.CompanyRepository.Update(comp);
                Adapter.Save();
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
