using Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.CrossCutting.Identity.Repositories
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {

        #region Variables

        #endregion

        #region Constructors

        public ApplicationUserManager(IUserStore<ApplicationUser> store,
                                      IOptions<IdentityOptions> optionsAccessor,
                                      IPasswordHasher<ApplicationUser> passwordHasher,
                                      IEnumerable<IUserValidator<ApplicationUser>> userValidators,
                                      IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                                      ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                      IServiceProvider services,
                                      ILogger<UserManager<ApplicationUser>> logger)
            : base(store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
        }

        #endregion

        #region Methods

        public Task<SignInResult> CheckOracleConnectionAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {

                var user = (Store as ApplicationUserStore).FindByNameAsync(userName, cancellationToken);

                if (user == null) return Task.FromResult(new SignInResult());

                #region Verificar se a senha está criptografada

                if (user.Result.UserPasswordType == 1 && !string.IsNullOrWhiteSpace(password))
                    password = (Store as ApplicationUserStore).CryptoOraclePasswordAsync(password.ToUpper().Trim(), cancellationToken).Result;

                #endregion

                return (Store as ApplicationUserStore).CheckOracleConnectionAsync(userName, password, cancellationToken);
            }
            catch (Exception)
            {
                return Task.FromResult(new SignInResult());
            }
        }

        public Task<byte[]> GetPhotoAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return (Store as ApplicationUserStore).GetUserPhotoAsync(user, cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.FromResult<byte[]>(null);
            }

        }

        #endregion

    }
}
