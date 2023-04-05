using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Repositories;
using VetClinic.DAL.Repositories.Interfaces;
using VetClinic.DAL.UnitOfWork.Interfaces;

namespace VetClinic.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        ICustomerRepository _customers;
        IProductRepository _products;
        IOrdersRepository _orders;
        IPetOwnerRepository _petowners;
        IPetDetailRepository _petdetails;
        IVetRepository _vets;
        IVisitRepository _visits;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }



        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                    _customers = new CustomerRepository(_context);

                return _customers;
            }
        }



        public IProductRepository Products
        {
            get
            {
                if (_products == null)
                    _products = new ProductRepository(_context);

                return _products;
            }
        }



        public IOrdersRepository Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new OrdersRepository(_context);

                return _orders;
            }
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

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
