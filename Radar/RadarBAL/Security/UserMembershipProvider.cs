using RadarBAL.ORM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Configuration;
using RadarModels;

namespace RadarBAL.Security
{
    public class UserMembershipProvider : MembershipProvider
    {
        #region VARIABLES
        private string _applicationName;
        private const bool _requiresUniqueEmail = true;
        private const bool _requiresUniqueUsername = true;
        private int _maxInvalidPasswordAttempts;
        private int _passwordAttemptWindow;
        private int _minRequiredPasswordLength;
        private int _minRequiredNonalphanumericCharacters;
        private bool _enablePasswordReset;
        private const bool _enablePasswordRetrieval = false;
        private string _passwordStrengthRegularExpression;
        private MembershipPasswordFormat _passwordFormat = MembershipPasswordFormat.Hashed;
        private string _connectionString;
        private string _providerName;
        #endregion

        #region PROPERTIES
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        public string ProviderName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonalphanumericCharacters; }
        }
        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }
        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }
        public bool RequiresUniqueUsername
        {
            get { return _requiresUniqueUsername; }
        }
        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
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
        private static string CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[32];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        private static string CreatePasswordHash(string password, string salt)
        {
            var i = 0;
            string hashed = password;
            while (i < 100)
            {
                string concat = string.Concat(hashed, salt);
                hashed = HashString(concat, "SHA512");
                i++;
            }
            return hashed;
        }
        public static string HashString(string inputString, string hashName)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create(hashName);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name", "hashName");
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return Convert.ToBase64String(hash);
        }
        #endregion

        #region INITIALIZE
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name)) name = "UserMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "UserMembershipProvider");
            }
            base.Initialize(name, config);

            string connectionStringName = GetConfigValue(config["connectionStringName"], "");
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;

            _applicationName = GetConfigValue(config["applicationName"],
                          System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(
                          GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(
                          GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonalphanumericCharacters = Convert.ToInt32(
                          GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(
                          GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _enablePasswordReset = Convert.ToBoolean(
                          GetConfigValue(config["enablePasswordReset"], "true"));
            _passwordStrengthRegularExpression = Convert.ToString(
                           GetConfigValue(config["passwordStrengthRegularExpression"], ""));

        }
        #endregion


        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            User user = Adapter.UserRepository.First(u => u.Email.Equals(email), null);
            if (user != null)
            {
                MembershipUser memUser = new UserMembershipUser("UserMembershipProvider",
                                               user.Email, null, user.Email,
                                               string.Empty, string.Empty,
                                               true, false, Convert.ToDateTime(user.CreatedDate),
                                               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, user);
                return memUser;
            }
            return null;
        } 

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string email, string password)
        {
            //1. GET USER BY EMAIL
            User user = Adapter.UserRepository.First(a => a.Email.Equals(email), null);
            if (user != null)
            {
                //2. CHECK IF USER IS NOT VIRTUAL DELETED
                if (user.DeletedDate != null) return false;
                //3. VALIDATE USER
                if (user.Password != CreatePasswordHash(password, user.Salt))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool ValidateUserEncoded(string email, string password)
        {
            //1. GET USER BY EMAIL
            User user = Adapter.UserRepository.First(a => a.Email.Equals(email), null);
            if (user != null)
            {
                //2. CHECK IF USER IS NOT VIRTUAL DELETED
                if (user.DeletedDate != null) return false;
                //3. VALIDATE USER
                if (user.Password != password)
                    return false;
                return true;
            }
            return false;
        }

        /**
         * NORMAL CREATE USER (NOT USED IN APP)
         **/
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUserBetter(string username, string email, string gender, string password, DateTime dob, string bio, long locationId, out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(email, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && EmailExists(email))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            if (RequiresUniqueUsername && UserNameExists(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            var u = GetUser(email, false);

            try
            {
                if (u == null)
                {
                    var salt = CreateSalt();

                    //CREATE NEW USER
                    var user = new User
                    {
                        Username = username,
                        Bio = bio,
                        Gender = gender,
                        Email = email,
                        Salt = salt,
                        Password = CreatePasswordHash(password, salt),
                        CreatedDate = DateTime.UtcNow,
                        DateOfBirth = dob
                    };
                    user.LocationId = locationId;
                    //CREATE USER VIA UNITOFWORK
                    Adapter.UserRepository.Insert(user);
                    Adapter.Save();

                    status = MembershipCreateStatus.Success;
                    return GetUser(email, true);
                }
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }

            status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        private bool UserNameExists(string username)
        {
            try
            {
                User user = Adapter.UserRepository.First(u => u.Username.Equals(username), null);
                if (user != null)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool EmailExists(string email)
        {
            try
            {
                User user = Adapter.UserRepository.First(u => u.Email.Equals(email), null);
                if (user != null)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
