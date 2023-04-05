using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.DAL.DbContexts;
using VetClinic.DAL.Models;
using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAL.Repositories
{
    public class AnimalTypeRepository : Repository<AnimalType>, IAnimalTypeRepository
    {
        public AnimalTypeRepository(DbContext context) : base(context)
        { }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
