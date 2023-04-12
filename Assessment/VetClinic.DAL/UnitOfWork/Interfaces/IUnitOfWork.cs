using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IPetOwnerRepository PetOwners { get; }
        IPetDetailRepository PetDetails { get; }
        IVetRepository Vets { get; }
        IVisitRepository Visits { get; }
        IBreedRepository Breeds { get; }
        IAnimalTypeRepository AnimalTypes { get; }


        int SaveChanges();
    }
}
