using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RadarAPI.Models;
using RadarModels;
using RadarBAL.Security;
using System.Web.Security;
using RadarBAL.ORM;
using Geocoding;
using Geocoding.Google;
using RadarBAL.mail;
using System.Net.Mail;
using System.Web.Configuration;

namespace RadarAPI.Controllers.WEB
{
    public class AccountController : Controller
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

        public ActionResult Login(string redirectUrl = "")
        {
            Login model = new Login();
            ViewBag.redirectUrl = redirectUrl;
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPost(string redirectUrl, Login model)
        {
            ViewBag.redirectUrl = redirectUrl;
            if (ModelState.IsValid)
            {
                UserMembershipProvider mp = new UserMembershipProvider();
                if (mp.ValidateUser(model.Email, model.Password))
                {
                    System.Web.HttpContext.Current.Session["Email"] = model.Email;

                    var users = Adapter.UserRepository.Find(a => a.Email == model.Email, null);
                    if (users != null && users.Any())
                    {
                        User user = users.First();
                        if (user.ApprovedDate == null)
                        {
                            ModelState.AddModelError("", "Je hebt je profiel nog niet geactiveerd met de activatielink in de e-mail.");
                            return View(model);
                        }
                        if (user.LockedDate != null)
                        {
                            ModelState.AddModelError("", "Een administrator heeft je profiel gelockt. Gelieve contact op te nemen met onze support.");
                            return View(model);
                        }

                        user.CreatedDate = DateTime.UtcNow;
                        Adapter.UserRepository.Update(user);
                        Adapter.Save();


                        HttpCookie cookie = new HttpCookie("RadarEmail", model.Email);
                        this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        HttpCookie cookieP = new HttpCookie("RadarPassword", user.Password);
                        this.ControllerContext.HttpContext.Response.Cookies.Add(cookieP);

                        if (!String.IsNullOrEmpty(redirectUrl))
                        {
                            byte[] b = Convert.FromBase64String(redirectUrl);
                            string url = System.Text.Encoding.UTF8.GetString(b);
                            return Redirect(url + "?&message=login");
                        }
                        else
                            return Redirect("http://localhost:4911/Radar/app/?message=login" );
                    }

                }
                else
                    ModelState.AddModelError("", "Het emailadres of het paswoord is niet geldig.");
            }
            return View(model);
        }

        public ActionResult Register(string redirectUrl = "")
        {
            ViewBag.redirectUrl = redirectUrl;
            Register model = new Register();
            return View(model);
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterPost(Register model, string redirectUrl)
        {
            ViewBag.redirectUrl = redirectUrl;
            if (ModelState.IsValid)
            {
                var userModel = model;
                RadarModels.Location loc = new RadarModels.Location();
                loc.Street = model.Location.Street;
                loc.Number = model.Location.Number;
                loc.Box = model.Location.Box;
                loc.Zipcode = model.Location.Zipcode;
                loc.City = model.Location.City;
                loc.Country = model.Location.Country;
                IGeocoder geocoder = new GoogleGeocoder();
                Address[] addresses = geocoder.Geocode(loc.Street + " " + loc.Number + ", " + loc.Zipcode + " " + loc.City + ", " + loc.Country).ToArray();
                if (addresses.Length != 0 && addresses[0].Coordinates != null)
                {
                    loc.Latitude = Convert.ToDecimal(addresses[0].Coordinates.Latitude);
                    loc.Longitude = Convert.ToDecimal(addresses[0].Coordinates.Longitude);
                    Adapter.LocationRepository.Insert(loc);
                    Adapter.Save();
                }
                else
                {
                    ModelState.AddModelError("", "Het adres kon niet worden gevonden.");
                    return View(model);
                }


                UserMembershipProvider mp = new UserMembershipProvider();

                MembershipCreateStatus status;

                UserMembershipUser mu = mp.CreateUserBetter(model.Username, model.Email, model.Gender?"m":"f", model.Password,model.DateOfBirth, model.Bio, loc.LocationId, out status) as UserMembershipUser;
                
                if (status == MembershipCreateStatus.DuplicateEmail)
                    ModelState.AddModelError("", "Emailadres heeft al een account.");
                else if(status == MembershipCreateStatus.InvalidPassword)
                    ModelState.AddModelError("", "Paswoord is niet sterk genoeg. Moet minimum 5 karakters zijn.");
                else if (status == MembershipCreateStatus.Success)
                {
                    SendMail(userModel);

                    if (!String.IsNullOrEmpty(redirectUrl))
                    {
                        byte[] b = Convert.FromBase64String(redirectUrl);
                        string url = System.Text.Encoding.UTF8.GetString(b);
                        return Redirect(url + "?&message=registered");
                    }
                    else
                        return Redirect("http://localhost:4911/Radar/app/?message=registered");
                }
            }
            else
            {
                ModelState.AddModelError("", "De ingevulde gegevens zijn niet correct.");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Logoff()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("RadarPassword"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["RadarPassword"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("RadarEmail"))
            {
                HttpCookie cookieE = this.ControllerContext.HttpContext.Request.Cookies["RadarEmail"];
                cookieE.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookieE);
            }
            return Json(new { message = "logoff" }, JsonRequestBehavior.DenyGet);
        }


        public ActionResult Activate(string activationkey)
        {
            try 
	        {
                byte[] b = Convert.FromBase64String(activationkey);
                string email = System.Text.Encoding.UTF8.GetString(b);
                var users = Adapter.UserRepository.Find(u => u.Email == email, null);
                var user = users.First();
                if (user.ApprovedDate != null)
                {
                    ViewBag.ErrorMsg = "Je account is reeds geactiveerd.";
                    return View();
                }
                user.ApprovedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                Adapter.UserRepository.Update(user);
                Adapter.Save();
                return RedirectToAction("Login");
	        }
	        catch (Exception ex)
	        {
                return View();
	        }                
        }

        [HttpPost, ActionName("Activate")]
        public ActionResult ActivatePost(string email)
        {
            var u = new Models.Register { Email = email };
            SendMail(u);
            ViewBag.Sent = true;
            return View();
        }

        private void SendMail(Models.Register userModel)
        {
            var b = System.Text.Encoding.UTF8.GetBytes(userModel.Email);
            string v = Convert.ToBase64String(b);
            MailManager mm = new MailManager();
            mm.EmailBodyHTML = @"
                <p style='font-size:12px;font-style:italic;'>
                    Dit is een automatisch gegenereerd bericht. Gelieve niet te antwoorden daar er niet geantwoord zal worden.
                </p>
                <h1>RADAR </h1>
                <p>U hebt u geregistreerd op Radar, Welkom!</p> 
                <p> Klik op de activatielink op je profiel te activeren: <a href='" + Url.Action("Activate", "Account", new { activationkey = v }, Request.Url.Scheme) + "'>Activeer je account bij RADAR</a>.</p>";
            MailAddress ma = new MailAddress("stefaan.ch+Radar@gmail.com");
            mm.EmailFrom = ma;
            mm.EmailSubject = "Radar - Registratie";
            MailAddress mas = new MailAddress(userModel.Email);
            List<MailAddress> masses = new List<MailAddress>();
            masses.Add(mas);
            mm.EmailTos = masses;
            mm.SmtpHost = WebConfigurationManager.AppSettings["SMTPHost"];
            mm.SmtpPort = Convert.ToInt32(WebConfigurationManager.AppSettings["SMTPPort"]);
            mm.IsSSL = true;
            mm.SmtpLogin = WebConfigurationManager.AppSettings["SMTPEmail"];
            mm.SmtpPassword = WebConfigurationManager.AppSettings["SMTPPassword"]; 
            mm.SendMail();
        }
    }
}
