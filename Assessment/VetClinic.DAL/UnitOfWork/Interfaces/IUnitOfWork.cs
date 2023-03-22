using VetClinic.DAl.Repositories.Interfaces;

namespace VetClinic.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }
        IOrdersRepository Orders { get; }


        int SaveChanges();
    }
}
