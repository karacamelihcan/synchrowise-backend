using Synchrowise.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.Services.UserServices;
using Synchrowise.Database.Repositories.UserRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//DbContext
var con = builder.Configuration.GetConnectionString("PostgreSql");
builder.Services.AddDbContext<SynchrowiseDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"),sqlOptions =>{
        sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(SynchrowiseDbContext)).GetName().Name);
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
