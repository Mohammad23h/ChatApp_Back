using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChatApp.EF
{
    

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Õœœ «·„”«— «·’ÕÌÕ ≈·Ï „·› appsettings.json ›Ì „‘—Ê⁄ chatapp.api
             IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../chatapp.api")) // „”«— «·‰”»Ì ≈·Ï appsettings.json
                 .AddJsonFile("appsettings.json")
                 .Build();
             /*
            IConfigurationBuilder configuration = new ConfigurationBuilder()
                .AddJsonFile("../ChatApp.EF/appsettings.json", false, true);
            IConfigurationRoot root = configuration.Build();*/
            //IConfigurationRoot configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
    }
}
 
}