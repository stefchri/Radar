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
    [RoutePrefix("api/posts")]
    public class PostController : ApiController
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
        public List<Post> Get()
        {
            List<Post> users = Adapter.PostRepository.Find(p => p.DeletedDate != null, "").OrderByDescending(c => c.CreatedDate).ToList();
            return users;
        }

        [Route("{id:int}"), HttpGet()]
        public Post GetOne(int id)
        {
            Post user = Adapter.PostRepository.GetByID(id);
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return user;
        }

        [Route("{id:int}"), HttpPut()]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage UpdatePost(int id,[FromBody] Post post)
        {
            if (post == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            var u = Adapter.PostRepository.GetByID(id);
            if (id == post.PostId)
            {
                try
                {
                    u.Body = post.Body;
                    u.SubTitle = post.SubTitle;
                    u.Title = post.Title;
                    u.ModifiedDate = DateTime.Now;
                    Adapter.PostRepository.Update(u);
                    Adapter.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, u);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The provided Id did not mathc the posts id.");
        }

        [Route("{id:int}"), HttpDelete()]
        [MembershipHttpAuthorize()]
        public HttpResponseMessage DeletePost(int id)
        {
            var post = Adapter.PostRepository.GetByID(id);
            if (post != null)
            {
                post.ModifiedDate = DateTime.Now;
                post.DeletedDate = DateTime.Now;
                Adapter.PostRepository.Update(post);
                Adapter.Save();
                return Request.CreateResponse(HttpStatusCode.OK, post);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Post not found.");
        }
    }
}
