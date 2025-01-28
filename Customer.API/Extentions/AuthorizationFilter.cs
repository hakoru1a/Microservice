using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Customer.API.Extensions
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context) => true;
        
    }
}
