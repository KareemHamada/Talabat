using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

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

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});


builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var Connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(Connection);

});
builder.Services.AddApplicationServices();
builder.Services.AddIdentiyServies(builder.Configuration);
builder.Services.AddCors(Options =>
{
    Options.AddPolicy("MyPolicy", options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
    });
});
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


    // for identity
    var IdentitydbContext = Services.GetRequiredService<AppIdentityDbContext>();
    await IdentitydbContext.Database.MigrateAsync();

    // Data Seeding
    var UserManager = Services.GetRequiredService<UserManager<AppUser>>();

    await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

    await StoreContextSeed.SeedAsync(dbContext);

}
catch (Exception ex)
{
    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "An error occured during appling the migration");
}

//Scope.Dispose();
#endregion


#region Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSwaggerMiddlewares();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseStaticFiles();
app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
#endregion



app.Run();
