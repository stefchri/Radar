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
    [RoutePrefix("api/roles")]    
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

        [Route("")]
        public List<Role> Get()
        {
            List<Role> roles = Adapter.RoleRepository.GetAll().ToList();
            return roles;
        }


        [MembershipHttpAuthorize()]
        [Route("{id:int}")]
        public Role Get(int id)
        {
            var role = Adapter.RoleRepository.GetByID(id);
            if (role == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return role;
        }

        [HttpPut, Route("{id:int}")]
        public HttpStatusCode Update(int id, Role role)
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
