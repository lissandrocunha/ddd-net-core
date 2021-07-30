using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Infra.CrossCutting.Identity.Models
{
    public class AspNetUser : IUser
    {
        #region Variables

        private readonly IHttpContextAccessor _accessor;

        #endregion

        #region Constructor

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        #endregion

        #region Properties

        public string Id => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value : null;
        //public string Id => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value : null;

        public string Name => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value : null; 

        public string UserName => IsAuthenticated ? _accessor.HttpContext.User.Identity.Name : null;

        public string Email => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value : null;

        public string Photo => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "photo")?.Value : null;

        public string System => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.System)?.Value : null;
        
        public string CPF => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CPF")?.Value : null;

        public bool IsAuthenticated => _accessor.HttpContext.User.Identity.IsAuthenticated;

        public IEnumerable<Claim> ClaimsIdentity => _accessor.HttpContext.User.Claims;

        public IEnumerable<string> Roles => IsAuthenticated ? _accessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)?.Select(r => r.Value) : null;

        public string RefreshToken => IsAuthenticated ? _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value : null;        

        #endregion

    }
}
