using System.Net.Http.Headers;
using System.Security.Claims;

namespace HRCoreSuite.Frontend.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var tokenClaim = user.FindFirst("access_token");

                if (tokenClaim != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim.Value);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}