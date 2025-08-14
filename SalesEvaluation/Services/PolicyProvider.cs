using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using SalesEvaluation.Model;

namespace SalesEvaluation.Services
{
    public class PolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DWUserInfo _userInfo;
        public PolicyProvider( IServiceProvider contextAccessor, DWUserInfo dWUser) 
        {
            _serviceProvider = contextAccessor;
            _userInfo = dWUser;

        }
         async Task<AuthorizationPolicy> IAuthorizationPolicyProvider.GetDefaultPolicyAsync()
        {
            var addPolicyAuthorizedUser =new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme)
                .Build();
            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            return addPolicyAuthorizedUser;
        }

        Task<AuthorizationPolicy?> IAuthorizationPolicyProvider.GetFallbackPolicyAsync()
        {
            throw new NotImplementedException();
        }

        Task<AuthorizationPolicy?> IAuthorizationPolicyProvider.GetPolicyAsync(string policyName)
        {
            throw new NotImplementedException();
        }
    }
}
