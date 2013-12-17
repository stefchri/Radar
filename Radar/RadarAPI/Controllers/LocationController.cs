using AttributeRouting.Web.Http;
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
    public class LocationController : ApiController
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

        [GET("api/locations")]
        public List<Location> Get()
        {
            List<Location> locs = Adapter.LocationRepository.GetAll().ToList();
            return locs;
        }


        [GET("api/locations/{id}")]
        public Location One(int id)
        {
            Location loc = Adapter.LocationRepository.GetByID(id);
            if (loc == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return loc;
        }



        [POST("api/locations")]
        [Authorize]
        public HttpStatusCode Insert(Location loc)
        {
            if (ModelState.IsValid)
            {
                Adapter.LocationRepository.Insert(loc);
                Adapter.Save();
                return HttpStatusCode.Created;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [POST("api/locations/{id}")]
        [Authorize]
        public HttpStatusCode Update(int id, Location loc)
        {
            if (ModelState.IsValid && id == loc.LocationId)
            {
                Adapter.LocationRepository.Update(loc);
                Adapter.Save();
                return HttpStatusCode.OK;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [POST("api/locations/delete/{id}")]
        [Authorize]
        public HttpStatusCode Delete(int id)
        {
            Location loc = Adapter.LocationRepository.GetByID(id);
            if (loc != null)
            {
                Adapter.LocationRepository.Delete(loc);
                Adapter.Save();
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }


        [GET("api/locations/{lat:decimal}/{lng:decimal}/{radius}")]
        public List<Location> Get(Decimal lat, Decimal lng, int radius)
        {
            //1degree +/- = 111 km
            var latkm = lat * 111; 
            var lngkm = lng * 111; 
            var latmin = (latkm - radius)/111;
            var lngmin = (lngkm - radius)/111;
            var latmax = (latkm + radius)/111;
            var lngmax = (lngkm + radius)/111;
            List<Location> locs = new List<Location>();
            locs = Adapter.LocationRepository.Find(l => l.Companies.Any(), "Companies").Where(l => l.Latitude > latmin && l.Latitude < latmax && l.Longitude > lngmin && l.Longitude < latmax).ToList();
            return locs;
        } 
    }
}
