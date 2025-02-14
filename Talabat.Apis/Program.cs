using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Apis.Errors;
using Talabat.Apis.Extensions;
using Talabat.Apis.Helpers;
using Talabat.Apis.Middlewares;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

var builder = WebApplication.CreateBuilder(args);


#region Configure service
// Add services to the container.
builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddApplicationServices();



#endregion



var app = builder.Build();

#region Update Database

// Group of services lifetime scopped
using var Scope = app.Services.CreateScope();
// Services its self
var Services = Scope.ServiceProvider;

var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
try
{

    // Ask CLR for creating object from DbContext Explicitly
    var dbContext = Services.GetRequiredService<StoreContext>();
    // Update-database
    await dbContext.Database.MigrateAsync();

    // Data Seeding
    await StoreContextSeed.SeedAsync(dbContext);

}
catch (Exception ex)
{
    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "An error occured during appling the migration");
}

//Scope.Dispose();
#endregion


#region Data Seeding
#endregion


#region Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSwaggerMiddlewares();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion



app.Run();
