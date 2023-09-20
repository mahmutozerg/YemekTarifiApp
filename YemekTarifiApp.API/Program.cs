using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YemekTarifiApp.Auth.Service.Configurations;
using YemekTarifiApp.Auth.Service.Services;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;
using YemekTarifiApp.Repository;
using YemekTarifiApp.Repository.Repositories;
using YemekTarifiApp.Service.Services;
using UnitOfWork = YemekTarifiApp.Repository.UnitOfWorks.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(YemekTarifiApp.Service.Services.GenericService<>));

builder.Services.AddScoped<IRecipeService,RecipeService>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IUserAppRepository, UserAppRepository>();

builder.Services.AddScoped<IFavoriteService,FavoriteService>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

builder.Services.AddScoped<ICommentService,CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidateIssuer = true,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidAudience = tokenOptions.Audience[0],
        ValidateAudience = true,

        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
});
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