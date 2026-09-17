using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WebApi_ASPNETCore.DataContext;

namespace WebApi_ASPNETCore.Tests.Integration;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(descriptor =>
                    descriptor.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>) ||
                    descriptor.ServiceType ==
                        typeof(DbContextOptions) ||
                    descriptor.ServiceType ==
                        typeof(IDbContextOptionsConfiguration<ApplicationDbContext>))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(
                    $"FuncionariosTestDb-{Guid.NewGuid()}");
            });
        });
    }
}
