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
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
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
        public List<Category> Get()
        {
            List<Category> cats = Adapter.CategoryRepository.GetAll().OrderBy(c => c.Name).ToList();
            return cats;
        }

        [Route("{id:int}")]
        public Category One(int id)
        {
            Category cat = Adapter.CategoryRepository.GetByID(id);
            if (cat == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return cat;
        }

        [Route("search/{name}")]
        public List<Category> Search(String name)
        {
            List<Category> cats = Adapter.CategoryRepository.Find(c => c.Name.ToLower().Contains(name.ToLower()), "Companies").OrderBy(c => c.Name).ToList();
            if (cats == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return cats;
        }

        [HttpPost, Route("search/{name}")]
        public HttpStatusCode Insert(Category cat)
        {
            if (ModelState.IsValid)
            {
                Adapter.CategoryRepository.Insert(cat);
                Adapter.Save();
                return HttpStatusCode.Created;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [HttpPut, Route("{id:int}")]
        public HttpStatusCode Update(int id, Category cat)
        {
            if (ModelState.IsValid && id == cat.CategoryId)
            {
                Adapter.CategoryRepository.Update(cat);
                Adapter.Save();
                return HttpStatusCode.OK;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [HttpDelete, Route("{id:int}")]
        [Authorize(Roles="Admin")]
        public HttpStatusCode Delete(int id)
        {
            Category cat = Adapter.CategoryRepository.GetByID(id);
            if (cat != null)
            {
                Adapter.CategoryRepository.Delete(cat);
                Adapter.Save();
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
