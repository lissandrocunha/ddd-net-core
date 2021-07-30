using Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.CrossCutting.Identity.Repositories
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {

        #region Variables

        #endregion

        #region Constructors

        public ApplicationSignInManager(UserManager<ApplicationUser> userManager, 
                                        IHttpContextAccessor contextAccessor, 
                                        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, 
                                        IOptions<IdentityOptions> optionsAccessor, 
                                        ILogger<SignInManager<ApplicationUser>> logger, 
                                        IAuthenticationSchemeProvider schemes) 
            : base(userManager, 
                   contextAccessor, 
                   claimsFactory, 
                   optionsAccessor, 
                   logger, 
                   schemes)
        {
        }

        #endregion

        #region Methods

        #endregion

    }
}