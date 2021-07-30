using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {

        #region Variables

        private string _name;
        private string _connectionString;
        private string _systemCod;
        private string _siginMethod;
        private int _userPasswordType;
        private long? _document;
        private IEnumerable<ApplicationRole> _roles;

        #endregion

        #region Properties

        public string Name { get => _name; set => _name = value; }
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string SystemCod { get => _systemCod; set => _systemCod = value; }
        public string SiginMethod { get => _siginMethod; set => _siginMethod = value; }
        public int UserPasswordType { get => _userPasswordType; set => _userPasswordType = value; }
        public long? Document { get => _document; set => _document = value; }
        public IEnumerable<ApplicationRole> Roles { get => _roles; set => _roles = value; }

        #endregion

        #region Constructors

        private ApplicationUser()
        {

        }

        #endregion

        #region Methods

        public void AtribuirRole(ApplicationRole role)
        {

        }

        #endregion

        #region Factories

        public static class Factory
        {

            public static ApplicationUser Novo(string id,
                                               string name,
                                               string userName,
                                               string email,
                                               int userPasswordType,
                                               string securityStamp,
                                               bool lockoutEnabled = false,
                                               int accessFailedCount = -1,
                                               bool emailConfirmed = false,
                                               long? document = null
                                               /*IEnumerable<ApplicationRole> roles = null*/)
            {

                var novoApplicationUser = new ApplicationUser()
                {
                    Id = id,
                    _name = name,
                    UserName = userName,
                    _userPasswordType = userPasswordType,
                    Email = email,
                    SecurityStamp = securityStamp,
                    LockoutEnabled = lockoutEnabled,
                    AccessFailedCount = accessFailedCount,
                    EmailConfirmed = emailConfirmed,
                    _document = document
                };

                return novoApplicationUser;
            }

            public static ApplicationUser SignIn(string userName,
                                                 string passwordHash,
                                                 string siginMethod = "SHC",
                                                 string systemCod = null)
            {
                var loginModel = new ApplicationUser()
                {
                    UserName = userName,
                    PasswordHash = passwordHash,
                    _systemCod = systemCod,
                    _siginMethod = siginMethod
                };

                return loginModel;
            }
        }

        #endregion
    }
}
