using Microsoft.EntityFrameworkCore;
using VetClinic.DAl.Models;
using VetClinic.DAl.Repositories.Interfaces;
using VetClinic.DAL.DbContexts;

namespace VetClinic.DAl.Repositories
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        public OrdersRepository(DbContext context) : base(context)
        { }




        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
