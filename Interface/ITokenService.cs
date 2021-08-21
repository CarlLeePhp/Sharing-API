using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
