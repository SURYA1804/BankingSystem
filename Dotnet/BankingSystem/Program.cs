using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using MyDbContext;
using Service;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<EmailCredentialsDTO>(
    builder.Configuration.GetSection("EmailCredentials"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPolicy",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Services.AddDbContext<MyAppDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowPolicy");
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();


