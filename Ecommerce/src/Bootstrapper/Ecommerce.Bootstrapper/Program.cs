using Ecommerce.Bootstrapper;
using Ecommerce.Shared.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
var assemblies = ModuleLoader.GetAssemblies();
var modules = ModuleLoader.LoadModules(assemblies);
builder.Host.ConfigureHost();
builder.Services.AddInfrastructure(assemblies, builder.Configuration);
foreach (var module in modules)
{
    module.Register(builder.Services, builder.Configuration);
}
var app = builder.Build();
app.UseInfrastructure();
//foreach (var module in modules)
//{
//    Console.WriteLine(module.Name);
//    module.Use(app);
//}
app.Run();
