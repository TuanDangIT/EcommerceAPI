using Ecommerce.Modules.Carts.Core.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using System;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Integration
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
                //var descriptor = services
                //    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<CartsDbContext>));

                //if (descriptor is not null)
                //{
                //    services.Remove(descriptor);
                //}
                //services.AddDbContext<CartsDbContext>(options =>
                //    options.UseNpgsql(_dbContainer.GetConnectionString()));
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
