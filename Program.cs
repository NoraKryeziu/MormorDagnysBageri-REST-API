using Microsoft.Extensions.Options;
using mormordagnysbageri_del1_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddControllers();

var app = builder.Build();

using var scope = app.Services.CreateScope();
try
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DataContext>();

    await context.Database.MigrateAsync();

    await Seed.LoadProducts(context);
    await Seed.LoadSuppliers(context);
    await Seed.LoadSupplierProducts(context);
   
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}


app.MapControllers();

app.Run();

