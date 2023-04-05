using Microsoft.EntityFrameworkCore;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Models;
using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAL.Repositories
{
    public class VetRepository : Repository<Vet>, IVetRepository
    {
        public VetRepository(DbContext context) : base(context)
        { }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
