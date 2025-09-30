using System.Text;
using DTO;
using interfaces;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyDbContext;
using Service;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Info("Application starting");

    builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();
    builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }); ;

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "SampleApi", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6..."
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
        });
    });
    string connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

    builder.Services.Configure<EmailCredentialsDTO>(
        builder.Configuration.GetSection("EmailCredentials"));

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy("StaffOrManager", p => p.RequireRole("staff", "Manager"));
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowPolicy",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
    });

    var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite");


    if (useSqlite)
    {
        builder.Services.AddDbContext<MyAppDbContext, SqliteDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
    }
    else
    {
        builder.Services.AddDbContext<MyAppDbContext, PostgresDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
    }


    builder.Services.AddSingleton<EmailService>();
    builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
    builder.Services.AddScoped<IUserRolesService, UserRolesService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
    builder.Services.AddScoped<IStaffService, StaffService>();
    builder.Services.AddScoped<IManagerService, ManagerService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
    builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
    builder.Services.AddScoped<ILoanService, LoanService>();
    builder.Services.AddScoped<ILoanTypeService, LoanTypeService>();
    builder.Services.AddScoped<ICustomerSupportService, CustomerSupportService>();

    builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
    builder.Services.AddScoped<IUserRolesService, UserRolesService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
    builder.Services.AddScoped<IStaffService, StaffService>();
    builder.Services.AddScoped<IManagerService, ManagerService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
    builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
    builder.Services.AddScoped<ILoanService, LoanService>();
    builder.Services.AddScoped<ILoanTypeService, LoanTypeService>();
    builder.Services.AddScoped<ICustomerSupportService, CustomerSupportService>();





    var app = builder.Build();

        // if (app.Environment.IsDevelopment())
        // {
        app.UseSwagger();
        app.UseSwaggerUI();
        // }
    app.UseMiddleware<ResponseTimeLoggingMiddleware>();    app.UseMiddleware<ResponseTimeLoggingMiddleware>();
        app.UseHttpsRedirection();
    //     // app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AllowPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
    app.Run();


}

catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}