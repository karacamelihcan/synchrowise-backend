using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Database;
using Synchrowise.Database.Repositories.GenericRepositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.Services.GenericServices;
using Synchrowise.Services.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SynchrowiseDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"),sqlOptions =>{
        sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(SynchrowiseDbContext)).GetName().Name);
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
