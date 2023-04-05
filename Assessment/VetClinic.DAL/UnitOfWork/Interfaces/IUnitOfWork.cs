using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }
        IOrdersRepository Orders { get; }
        IPetOwnerRepository PetOwners { get; }
        IPetDetailRepository PetDetails { get; }
        IVetRepository Vets { get; }
        IVisitRepository Visits { get; }



        int SaveChanges();
    }
}
