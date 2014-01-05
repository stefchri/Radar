using RadarBAL.ORM;
using RadarModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RadarAPI.Controllers
{
    [RoutePrefix("api/files")]
    public class FileController : ApiController
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

        [Route("upload"), HttpPost()]
        public async Task<HttpResponseMessage> Get()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Radar/app/img/avatars");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                var name =  Guid.NewGuid().ToString() + "." + provider.FileData[0].Headers.ContentType.MediaType.Split(new char[] {'/'})[1];
                var newPath = root + "\\" + name;
                File.Move(provider.FileData.First().LocalFileName, newPath);
                
                int userId = Convert.ToInt32(provider.FormData["user"]);

                var user = Adapter.UserRepository.GetByID(userId);
                user.Avatar = name;
                Adapter.UserRepository.Update(user);
                Adapter.Save();

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
