using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Integration
{
    public class EcommerceTestApp : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
        private readonly FakeTimeProvider _fakeTimeProvider;
        public EcommerceTestApp()
        {
            _fakeTimeProvider = new FakeTimeProvider();
            _fakeTimeProvider.SetUtcNow(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsInterface && x != typeof(DbContext));

                foreach (var dbContextType in dbContextTypes)
                {
                    var dbContextOptionsType = typeof(DbContextOptions<>).MakeGenericType(dbContextType);

                    var descriptor = services.SingleOrDefault(s => s.ServiceType == dbContextOptionsType);
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    Type dbContextOptionsBuilderType = typeof(DbContextOptionsBuilder<>);
                    Type dbContextOptionsBuilderGenericType = dbContextOptionsBuilderType.MakeGenericType(dbContextType);
                    DbContextOptionsBuilder dbContextOptionsBuilderInstance = Activator.CreateInstance(dbContextOptionsBuilderGenericType) as DbContextOptionsBuilder ?? throw new NullReferenceException();
                    dbContextOptionsBuilderInstance.UseNpgsql(_dbContainer.GetConnectionString());
                    var options = dbContextOptionsBuilderInstance.Options;

                    services.AddScoped(dbContextOptionsType, _ => options);
                    services.AddScoped(dbContextType, sp =>
                    {
                        var options = sp.GetRequiredService(dbContextOptionsType);
                        return (DbContext?)Activator.CreateInstance(dbContextType, options, _fakeTimeProvider) ?? throw new NullReferenceException(); ;
                    });
                }
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}
