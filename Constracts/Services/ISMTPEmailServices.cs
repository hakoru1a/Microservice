using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Services
{
    public interface ISMTPEmailServices : IEmailServices<MailRequest>
    {
    }
}
