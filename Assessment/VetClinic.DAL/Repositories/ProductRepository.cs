using Microsoft.EntityFrameworkCore;
using VetClinic.DAl.Models;
using VetClinic.DAl.Repositories.Interfaces;
using VetClinic.DAL.DbContexts;

namespace VetClinic.DAl.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        { }




        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
