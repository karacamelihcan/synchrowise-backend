using Synchrowise.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.Services.UserServices;
using Synchrowise.Database.Repositories.UserRepositories;
using Synchrowise.Database.Repositories.GroupRepositories;
using Synchrowise.Services.Services.GroupServices;
using NLog.Web;
using NLog;
using Synchrowise.Database.Repositories.UserAvatarRepositories;
using Synchrowise.Database.Repositories.GroupFileRepositories;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //DbContext

    builder.Services.AddDbContext<SynchrowiseDbContext>(options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"),sqlOptions =>{
            sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(SynchrowiseDbContext)).GetName().Name);
        });
    });

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IUserRepository,UserRepository>();
    builder.Services.AddScoped<IUserService,UserService>();

    builder.Services.AddScoped<IGroupRepository,GroupRepository>();
    builder.Services.AddScoped<IGroupService,GroupService>();

    builder.Services.AddScoped<IUserAvatarRepository,UserAvatarRepository>();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddScoped<IGroupFileRepository,GroupFileRepository>();

    builder.Services.AddControllers().AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseHttpLogging();
    
    if(!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error/500");
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","v1");
        options.RoutePrefix = string.Empty;
    });


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseStaticFiles();

    app.MapControllers();

    app.Run();
}
catch (System.Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}