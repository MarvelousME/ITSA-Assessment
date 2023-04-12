using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Repositories;
using VetClinic.DAL.Repositories.Interfaces;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        IPetOwnerRepository _petowners;
        IPetDetailRepository _petdetails;
        IVetRepository _vets;
        IVisitRepository _visits;
        IAnimalTypeRepository _animaltypes;
        IBreedRepository _breeds;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPetOwnerRepository PetOwners
        {
            get
            {
                if (_petowners == null)
                    _petowners = new PetOwnerRepository(_context);

                return _petowners;
            }
        }

        public IPetDetailRepository PetDetails
        {
            get
            {
                if (_petdetails == null)
                    _petdetails = new PetDetailRepository(_context);

                return _petdetails;
            }
        }

        public IVetRepository Vets
        {
            get
            {
                if (_vets == null)
                    _vets = new VetRepository(_context);

                return _vets;
            }
        }

        public IVisitRepository Visits
        {
            get
            {
                if (_visits == null)
                    _visits = new VisitRepository(_context);

                return _visits;
            }
        }

        public IAnimalTypeRepository AnimalTypes
        {
            get
            {
                if (_animaltypes == null)
                    _animaltypes = new AnimalTypeRepository(_context);

                return _animaltypes;
            }
        }

        public IBreedRepository Breeds
        {
            get
            {
                if (_breeds == null)
                    _breeds = new BreedRepository(_context);

                return _breeds;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
