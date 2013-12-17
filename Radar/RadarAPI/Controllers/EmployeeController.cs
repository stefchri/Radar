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
    public class EmployeeController : ApiController
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

        [GET("api/employees")]
        public List<Employee> Get()
        {
            List<Employee> emps = Adapter.EmployeeRepository.GetAll().OrderBy(c => c.User.Username).ToList();
            return emps;
        }


        [GET("api/employees/{id}")]
        public Employee One(int id)
        {
            Employee emp = Adapter.EmployeeRepository.GetByID(id);
            if (emp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return emp;
        }



        [POST("api/employees")]
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

        [POST("api/employees/{id}")]
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

        [POST("api/employees/delete/{id}")]
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



#region COMPANY EMPLOYEES
        [GET("api/companies/{companyId}/employees")]
        public List<Employee> Get(int companyId)
        {
            List<Employee> emps = Adapter.EmployeeRepository.Find(e => e.CompanyId == companyId, "").OrderBy(c => c.User.Username).ToList();
            return emps;
        }

        [GET("api/companies/{companyId}/employees/search/{name}")]
        public List<Employee> Search(int companyId, String name)
        {
            List<Employee> emps = Adapter.EmployeeRepository.Find(e => e.CompanyId == companyId, "").Where(c => c.User.Username.ToLower().Contains(name.ToLower())).OrderBy(c => c.User.Username).ToList();
            if (emps == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return emps;
        }
#endregion

    }
}
