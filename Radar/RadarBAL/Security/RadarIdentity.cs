using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Web.Security;
using RadarModels;
using RadarBAL.ORM;

namespace RadarBAL.Security
{
    [Serializable]
    public class RadarIdentity : MarshalByRefObject, IIdentity
    {
        #region PROPERTIES
        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
        #endregion

        #region IIDENTITY PROPERTIES
        public string AuthenticationType 
        { 
            get; 
            private set; 
        }

        public string Email
        { 
            get; 
            private set; 
        }
        #endregion

        #region CONSTRUCTOR
        public RadarIdentity(string email, string auhtenticationType)
        {
            Email = email;
            AuthenticationType = AuthenticationType;
            SetUser();
        }
        private void SetUser()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                this._user = unitOfWork.UserRepository.Single(u => u.Email.Equals(Email), null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public bool IsAuthenticated
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
