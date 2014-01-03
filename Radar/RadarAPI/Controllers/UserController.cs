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
        public List<User> Get()
        {
            List<User> users = Adapter.UserRepository.GetAll().OrderBy(c => c.Email).ToList();
            return users;
        }

        [Route("{id:int}"), HttpGet()]
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

    }
}
