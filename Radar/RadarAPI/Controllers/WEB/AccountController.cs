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
                            return Redirect(url + "?&login=success");
                        }
                        else
                            return RedirectToAction("Index", "Home", new { message = "login" });
                    }

                }
                else
                {
                    ViewBag.redirectUrl = redirectUrl;
                    ModelState.AddModelError("", "Het emailadres of het paswoord is niet geldig.");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            Register model = new Register();
            return View(model);
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterPost(Register model)
        {
            if (ModelState.IsValid)
            {
                var userModel = model;
                Location loc = new Location();
                loc.Street = model.Location.Street;
                loc.Number = model.Location.Number;
                loc.Box = model.Location.Box;
                loc.Zipcode = model.Location.Zipcode;
                loc.City = model.Location.City;
                loc.Country = model.Location.Country;
                loc.Latitude = model.Location.Latitude;
                loc.Longitude = model.Location.Longitude;
                Adapter.LocationRepository.Insert(loc);
                Adapter.Save();


                UserMembershipProvider mp = new UserMembershipProvider();

                MembershipCreateStatus status;

                UserMembershipUser mu = mp.CreateUserBetter(model.Username, model.Email, model.Gender?"m":"f", model.Password,model.DateOfBirth, model.Bio, loc.LocationId, out status) as UserMembershipUser;
                
                if (status == MembershipCreateStatus.DuplicateEmail)
                {
                    ModelState.AddModelError("", "Emailadres heeft al een account.");
                }
                else if (status == MembershipCreateStatus.Success)
                {
                    //string mail = MVCExtensions.getCurrentAdmin().Email;
                    //MailManager mm = new MailManager();
                    //mm.EmailBodyHTML = "<p style='font-size:12px;font-style:italic;'>Dit is een automatisch gegenereerd bericht. Gelieve niet te antwoorden daar er niet geantwoord zal worden.</p><h1>ArticulatieOnderzoek </h1><p>U bent geregistreerd door <a href='mailto:" + mail + "?Subject=ArticulatieOnderzoek%20Vraag'>" + HttpContext.User.Identity.Name + " </a></p>";
                    //MailAddress ma = new MailAddress("aonderzoek@gmail.com");
                    //mm.EmailFrom = ma;
                    //mm.EmailSubject = "ArticulatieOnderzoek - Registratie";
                    //MailAddress mas = new MailAddress(viewModel.Email);
                    //List<MailAddress> masses = new List<MailAddress>();
                    //masses.Add(mas);
                    //mm.EmailTos = masses;
                    //mm.SmtpHost = "smtp.gmail.com";
                    //mm.SmtpPort = 587;
                    //mm.IsSSL = true;
                    //mm.SmtpLogin = "aonderzoek@gmail.com";
                    //mm.SmtpPassword = "qwerty123!";
                    //mm.SendMail();
                    
                    return RedirectToAction("Index", "Home", new { message = "registered" });

                }
            }
            else
            {
                ModelState.AddModelError("", "De ingevulde gegevens zijn niet correct.");
            }
            return View(model);
        }
    }
}
