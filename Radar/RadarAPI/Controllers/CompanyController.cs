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

        [GET("api/companies")]
        public List<Company> Get()
        {
            List<Company> comps = Adapter.CompanyRepository.GetAll().OrderBy(c => c.Name).ToList();
            return comps;
        }

        [GET("api/companies/{id}")]
        public Company One(int id)
        {
            Company comp = Adapter.CompanyRepository.GetByID(id);
            if (comp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comp;
        }

        [GET("api/companies/search/{name}")]
        public List<Company> Search(String name)
        {
            List<Company> comps = Adapter.CompanyRepository.Find(c => c.Name.ToLower().Contains(name.ToLower()), "").OrderBy(c => c.Name).ToList();
            if (comps == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comps;
        }


        [POST("api/companies")]
        [Authorize]
        public HttpStatusCode Insert(Company comp)
        {
            if (ModelState.IsValid)
            {
                Adapter.CompanyRepository.Insert(comp);
                Adapter.Save();
                return HttpStatusCode.Created;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [POST("api/companies/{id}")]
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

        [POST("api/companies/delete/{id}")]
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
