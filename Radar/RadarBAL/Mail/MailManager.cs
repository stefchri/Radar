using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RadarBAL.mail
{
    public class MailManager : Object
    {
        #region VARIABLES
        private string _smtpHost;
        private int _smtpPort = 80;
        private string _smtpLogin;
        private string _smtpPassword;
        private Boolean _isSSL = false;
        private string _emailSubject;
        private string _emailBodyPlain;
        private string _emailBodyHTML;
        private MailAddress _emailFrom;
        private List<MailAddress> _emailTos;
        private List<Attachment> _emailAttachements;
        private List<LinkedResource> _emailLinkedResources;
        #endregion

        #region PROPERTIES
        public MailAddress EmailFrom
        {
            get { return _emailFrom; }
            set { _emailFrom = value; }
        }
        public List<MailAddress> EmailTos
        {
            get
            {
                if (_emailTos == null)
                {
                    _emailTos = new List<MailAddress>();
                }
                return _emailTos;
            }
            set { _emailTos = value; }
        }
        public string EmailSubject
        {
            get { return _emailSubject; }
            set { _emailSubject = value; }
        }
        public string EmailBodyPlain
        {
            get { return _emailBodyPlain; }
            set { _emailBodyPlain = value; }
        }
        public string EmailBodyHTML
        {
            get { return _emailBodyHTML; }
            set { _emailBodyHTML = value; }
        }
        public string SmtpHost
        {
            get { return _smtpHost; }
            set { _smtpHost = value; }
        }
        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }
        public string SmtpLogin
        {
            get { return _smtpLogin; }
            set { _smtpLogin = value; }
        }
        public string SmtpPassword
        {
            get { return _smtpPassword; }
            set { _smtpPassword = value; }
        }
        public Boolean IsSSL
        {
            get { return _isSSL; }
            set { _isSSL = value; }
        }
        public List<Attachment> EmailAttachements
        {
            get
            {
                if (_emailAttachements == null)
                {
                    _emailAttachements = new List<Attachment>();
                }
                return _emailAttachements;
            }
            set { _emailAttachements = value; }
        }
        public List<LinkedResource> EmailLinkedResources
        {
            get
            {
                if (_emailLinkedResources == null)
                {
                    _emailLinkedResources = new List<LinkedResource>();
                }
                return _emailLinkedResources;
            }
            set { _emailLinkedResources = value; }
        }
        #endregion

        #region CONSTRUCTOR
        public MailManager()
            : base()
        { }
        #endregion

        #region METHODS
        public void SendMail()
        {
            try
            {
                //MAIL MESSAGE
                //===========================================================
                //1. CREATE MAIL MESSAGE
                MailMessage mailMessage = new MailMessage();

                //2. FROM
                mailMessage.From = EmailFrom;

                //3. TO
                if (EmailTos != null)
                {
                    foreach (MailAddress addr in EmailTos)
                    {
                        mailMessage.To.Add(addr);
                    }
                }

                //3bis. ATTACHEMENTS
                if (EmailAttachements != null)
                {
                    foreach (Attachment attach in EmailAttachements)
                    {
                        mailMessage.Attachments.Add(attach);
                    }
                }

                //4. SUBJECT
                mailMessage.Subject = EmailSubject;

                //5. BODY
                if (EmailBodyPlain != null)
                {
                    AlternateView viewPlain = AlternateView.CreateAlternateViewFromString(EmailBodyPlain, null, "text/plain");
                    mailMessage.AlternateViews.Add(viewPlain);
                }
                if (EmailBodyHTML != null)
                {
                    AlternateView viewHTML = AlternateView.CreateAlternateViewFromString(EmailBodyHTML, null, "text/html");
                    //LINKED RESOURCES
                    if (EmailLinkedResources != null)
                    {
                        foreach (LinkedResource res in EmailLinkedResources)
                        {
                            viewHTML.LinkedResources.Add(res);
                        }
                    }
                    mailMessage.AlternateViews.Add(viewHTML);
                }

                //SMTP CLIENT
                //===========================================================
                //1. CREATE SMTP CLIENT
                SmtpClient smtpClient = new SmtpClient();
                //2. HOST
                smtpClient.Host = SmtpHost;
                smtpClient.Port = SmtpPort;
                //3. CREDENTIALS
                if (SmtpLogin != null)
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(SmtpLogin, SmtpPassword);
                }
                //4. SSL OR NOT
                smtpClient.EnableSsl = IsSSL;
                //5. SEND EMAIL
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region OVERRIDE METHODS
        public override string ToString()
        {
            return this.GetType().ToString();
        }
        #endregion
    }
}