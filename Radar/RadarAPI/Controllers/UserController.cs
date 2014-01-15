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
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        #region UNITOFWORK
        private UnitOfWork _adapter = null;
        protected UnitOfWork Adapter
        {
            get
            {
                if (_adapter == null)
                    _adapter = new UnitOfWork();
                return _adapter;
            }
        }
        #endregion

        [Route(""), HttpGet()]
        [MembershipHttpAuthorize()]
        public List<User> Get()
        {
            List<User> users = Adapter.UserRepository.GetAll().OrderBy(c => c.Email).ToList();
            return users;
        }

        [Route("{id:int}"), HttpGet()]
        [MembershipHttpAuthorize()]
        public User GetOne(int id)
        {
            User user = Adapter.UserRepository.GetByID(id);
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return user;
        }

        [Route("{email}"), HttpGet()]
        public User GetByEmail(string email)
        {
            User user = Adapter.UserRepository.Find(c => c.Email == email, "Roles").FirstOrDefault();
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return user;
        }

        [Route("{id:int}"), HttpPut()]
        [MembershipHttpAuthorize()]
        public bool UpdateProfile(int id,[FromBody] User user)
        {
            if (user == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            var u = Adapter.UserRepository.GetByID(id);
            if (user.Email == u.Email && id == user.UserId)
            {
                try
                {
                    u.Bio = user.Bio;
                    u.Username = user.Username;
                    u.DateOfBirth = user.DateOfBirth;
                    u.Gender = user.Gender;
                    u.ModifiedDate = DateTime.Now;
                    Adapter.UserRepository.Update(u);

                    var l = Adapter.LocationRepository.GetByID(user.Location.LocationId);
                    l.Street = user.Location.Street;
                    l.Number = user.Location.Number;
                    l.Box = user.Location.Box;
                    l.Zipcode = user.Location.Zipcode;
                    l.City = user.Location.City;
                    l.Country = user.Location.Country;
                    IGeocoder geocoder = new GoogleGeocoder();
                    Address[] addresses = geocoder.Geocode(l.Street + " " + l.Number + ", " + l.Zipcode + " " + l.City + ", " + l.Country).ToArray();
                    if (addresses.Length != 0 && addresses[0].Coordinates != null)
                    {
                        l.Latitude = Convert.ToDecimal(addresses[0].Coordinates.Latitude);
                        l.Longitude = Convert.ToDecimal(addresses[0].Coordinates.Longitude);
                    }
                    else
                        return false;
                    Adapter.LocationRepository.Update(l);

                    Adapter.Save();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
                return false;
        }

        [Route("track/{id:int}"), HttpPost()]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage TrackPerson(int id,[FromBody] User user)
        {
            User u = Adapter.UserRepository.Find(c => c.Email == user.Email, null).FirstOrDefault();
            if (u == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else
            {
                u.FollowingUsers.Add(Adapter.UserRepository.GetByID(id));
                Adapter.Save();
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("untrack/{id:int}"), HttpPost()]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage UnTrackPerson(int id, [FromBody] User user)
        {
            User u = Adapter.UserRepository.Find(c => c.Email == user.Email, null).FirstOrDefault();
            if (u == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (u.FollowingUsers.Contains(Adapter.UserRepository.GetByID(id)))
            {
                u.FollowingUsers.Remove(Adapter.UserRepository.GetByID(id));
                Adapter.Save();
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{id:int}/followers"), HttpGet()]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage GetFollowers(int id)
        {
            User u = Adapter.UserRepository.Find(c => c.UserId == id, null).FirstOrDefault();
            if (u == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else 
                return Request.CreateResponse(HttpStatusCode.OK, u.FollowingUsers);
        }

    }
}
