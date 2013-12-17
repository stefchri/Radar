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
    public class CommentController : ApiController
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

        [GET("api/comments")]
        public List<Comment> Get()
        {
            List<Comment> comms = Adapter.CommentRepository.GetAll().OrderByDescending(c => c.CreatedDate).ToList();
            return comms;
        }

        [GET("api/comments/{id}")]
        public Comment One(int id)
        {
            Comment comm = Adapter.CommentRepository.GetByID(id);
            if (comm == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return comm;
        }

        [POST("api/comments")]
        [Authorize]
        public HttpStatusCode Insert(Comment comm)
        {
            if (ModelState.IsValid)
            {
                Adapter.CommentRepository.Insert(comm);
                Adapter.Save();
                return HttpStatusCode.Created;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [POST("api/comments/{id}")]
        [Authorize]
        public HttpStatusCode Update(int id, Comment comm)
        {
            if (ModelState.IsValid && id == comm.CommentId)
            {
                comm.ModifiedDate = DateTime.Now;
                Adapter.CommentRepository.Update(comm);
                Adapter.Save();
                return HttpStatusCode.OK;
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        [POST("api/comments/delete/{id}")]
        [Authorize]
        public HttpStatusCode Delete(int id)
        {
            Comment comm = Adapter.CommentRepository.GetByID(id);
            if (comm != null)
            {
                comm.DeletedDate = DateTime.Now;
                Adapter.CommentRepository.Update(comm);
                Adapter.Save();
            }
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
