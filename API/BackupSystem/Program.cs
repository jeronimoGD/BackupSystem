using BackupSystem.ApplicationSettings;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Interfaces.SignalR;
using BackupSystem.Common.Mappings;
using BackupSystem.Common.Repository;
using BackupSystem.Common.Services.DbManagementServices;
using BackupSystem.Common.Services.SignalR;
using BackupSystem.Data;
using BackupSystem.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Añadir security definition and requirements para autentificacion desde Swager
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
                      "Ejemplo: Bearer 123456abcder",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


// Añadir configuration para la autentification usando JSON Web Tokens (JWT)
var key = builder.Configuration.GetValue<string>("APISettings:JwtAuthFields:SecretKey");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Set aplication settings from apsettings.json
builder.Services.Configure<APISettings>(builder.Configuration.GetSection("APISettings"));

// Add data base conexion string for migrations
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbConexion"));
});

//identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Inject Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Insert entity services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAgentsService, AgentsService>();
builder.Services.AddTransient<IBackUpHistoryService, BackUpHistoryService>();
builder.Services.AddTransient<IBackUpConfigurationService, BackUpConfigurationService>();

// Inject Mapping Config
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Inject SignalR
builder.Services.AddSignalR();

// Inject SignalRConnectionManager
builder.Services.AddSingleton<ISignalRConnectionManager, SignalRConnectionManager>();
builder.Services.AddSingleton<ICheckAliveTimeoutsManager, CheckAliveTimeoutsManager>();
builder.Services.AddTransient<IAgentConfigurationHubService, AgentConfigurationHubService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dataContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

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
app.MapHub<AgentConfigurationHub>("/agentConfigurationHub");

InitialConfiguration(app, app.Environment);

app.Run();

void InitialConfiguration(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Configuraciones de producción...
    }

    InitialDbOperations(app);
}

void InitialDbOperations(IApplicationBuilder app)
{
    var serviceScope = app.ApplicationServices.CreateScope();
    var agentsService = serviceScope.ServiceProvider.GetRequiredService<IAgentsService>();
    agentsService.SetAllOnlineStatus(false);
}
