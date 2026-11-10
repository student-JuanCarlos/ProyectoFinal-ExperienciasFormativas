using Data.Infraestructure; 
using Microsoft.AspNetCore.Http;

namespace App_Web.Tenant
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TenantProvider(IHttpContextAccessor context)
        {
            _contextAccessor = context;
        }

        public int IdRestaurante
        {
            get
            {
                var claims = _contextAccessor.HttpContext?.User.FindFirst("IdRestaurante")?.Value;
                return int.TryParse(claims, out var id) ? id : 0;
            }
        }
    }
}
