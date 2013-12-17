using RadarModels;
using RadarBAL.ORM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace RadarBAL.Security
{
    class RadarRoleProvider : RoleProvider
    {
        #region VARIABLES
        private string _applicationName;
        #endregion

        #region PROPERTIES
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        #endregion

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

        #region CUSTOM METHODS
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
        private string[] ConvertRoleCollectionToStringArray(ICollection<Role> roles)
        {
            string[] rolesArray = new string[roles.Count];
            int i = 0;
            foreach (Role role in roles)
            {
                rolesArray[i] = role.Name;
                i++;
            }
            return rolesArray;
        }
        private string[] ConvertUserCollectionToStringArray(ICollection<User> users)
        {
            string[] usersArray = new string[users.Count];
            int i = 0;
            foreach (User user in users)
            {
                usersArray[i] = user.Username;
                i++;
            }
            return usersArray;
        }
        #endregion

        #region INITALIZE
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name)) name = "RadarRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Radar Role Provider");
            }

            base.Initialize(name, config);

            _applicationName = GetConfigValue(config["applicationName"],
                          System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }
        #endregion

        #region OVERRIDE METHODS
        public override bool RoleExists(string roleName)
        {
            try
            {
                return (Adapter.RoleRepository.Find(r => r.Name.Equals(roleName), null) == null) ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string[] GetAllRoles()
        {
            try
            {
                ICollection<Role> roles = Adapter.RoleRepository.GetAll().ToList();
                //CREATE STRING ARRAY
                return ConvertRoleCollectionToStringArray(roles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void CreateRole(string roleName)
        {
            try
            {
                if (RoleExists(roleName))
                {
                    throw new Exception();
                }
                else
                {
                    Adapter.RoleRepository.Insert(new Role
                    {
                        Name = roleName
                    });
                    //SAVE CHANGES
                    Adapter.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            try
            {
                if (RoleExists(roleName))
                {
                    //FIND ROLE
                    Role role = Adapter.RoleRepository.Single(r => r.Name.Equals(roleName), null);
                    //TRY TO DELETE
                    Adapter.RoleRepository.Delete(role);
                    //SAVE CHANGES
                    Adapter.Save();
                    //SUCCEED
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                User user = Adapter.UserRepository.Single(u => u.Username.Equals(username), null);
                //CREATE STRING ARRAY
                return ConvertRoleCollectionToStringArray(user.Roles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            try
            {
                Role role = Adapter.RoleRepository.Single(r => r.Name.Equals(roleName), "Users");
                //CREATE STRING ARRAY
                return ConvertUserCollectionToStringArray(role.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            try
            {
                Role role = Adapter.RoleRepository.Single(r => r.Name.Equals(roleName), "Users");
                //CREATE STRING ARRAY
                return ConvertUserCollectionToStringArray(role.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                string[] users = GetUsersInRole(roleName);
                bool present = false;
                int i = 0;
                while (!present && i < users.Length)
                {
                    if (users[i] == username)
                    {
                        present = true;
                    }
                    i++;
                }
                return present;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                Role role = Adapter.RoleRepository.Single(r => r.Name.Equals(roleName), "Users");
                foreach (string userName in usernames)
                {
                    User user = Adapter.UserRepository.Single(u => u.Username.Equals(userName), null);
                    try
                    {
                        if (RoleExists(roleName) && !IsUserInRole(userName, roleName))
                        {
                            role.Users.Add(user);
                            Adapter.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
