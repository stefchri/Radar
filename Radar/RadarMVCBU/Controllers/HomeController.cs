using RadarModels;
using RadarBAL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadarMVC.Controllers
{
    public class HomeController : Controller
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

        public ActionResult Index()
        {
            Role role = new Role();
            role.Name = "Admin";
            //Adapter.RoleRepository.Insert(role);
            Adapter.Save();
            var roles = Adapter.RoleRepository.GetAll();
            return View();
        }

    }
}
