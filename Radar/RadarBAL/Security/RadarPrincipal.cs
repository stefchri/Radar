using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace RadarBAL.Security
{
    [Serializable]
    public class RadarPrincipal : MarshalByRefObject, IPrincipal
    {
        #region IPrincipal Members
        private RadarIdentity _identity;
        public IIdentity Identity
        {
            get { return _identity; }
            set { _identity = (RadarIdentity)value; }
        }
        public string[] GetRoles(string userName)
        {
            try
            {
                return Roles.GetRolesForUser(userName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public bool IsInRole(string role)
        {
            try
            {
                return Roles.IsUserInRole(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        IIdentity IPrincipal.Identity { get { return this.Identity; } }
        #endregion
        #region CONSTRUCTOR
        public RadarPrincipal(RadarIdentity cIndent)
        {
            this.Identity = cIndent;
        }
        #endregion
    }
}
