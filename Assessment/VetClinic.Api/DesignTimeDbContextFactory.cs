using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VetClinic.DAL.DbContexts;

namespace VetClinic.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.Staging.json", optional: true)
                .AddJsonFile("appsettings.Production.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("VetClinic.Api"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
