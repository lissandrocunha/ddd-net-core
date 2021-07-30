using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Domain.Core.Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        string UserName { get; }
        string Email { get; }
        string Photo { get; }
        string System { get; }
        string CPF { get; }
        bool IsAuthenticated { get; }
        IEnumerable<Claim> ClaimsIdentity { get; }
        IEnumerable<string> Roles { get; }
        string RefreshToken { get; }
    }
}
