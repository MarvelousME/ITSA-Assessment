using Microsoft.AspNetCore.Http;
using VetClinic.DAl.Core;
using VetClinic.DAL.DbContexts;

namespace VetClinic.DAL.UnitOfWork
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
        }
    }
}
