using AttributeRouting.Web.Http;
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
    //[HttpHeader("Access-Control-Allow-Origin", "*")]     
    public class RoleController : ApiController
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

        [GET("api/roles")]
        public List<Role> Get()
        {
            List<Role> roles = Adapter.RoleRepository.GetAll().ToList();
            return roles;
        }


        [MembershipHttpAuthorize()]
        //[Authorize]
        [GET("api/roles/{id}")]
        public Role Get(int id)
        {
            var role = Adapter.RoleRepository.GetByID(id);
            if (role == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return role;
        }

        [POST("api/roles/{id}")]
        public HttpResponseMessage Update(int id, Role role)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("api/roles")]
        public HttpStatusCode Update(Role role)
        {
            if (role != null && ModelState.IsValid)
            {
                Adapter.RoleRepository.Insert(role);
                Adapter.Save();
                return HttpStatusCode.Created;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

    }
}
