using Shared.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Indentity
{
    public interface ITokenService
    {
        TokenResponse GetToken(TokenRequest token);
    }
}
