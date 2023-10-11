using Application.IoC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Entities.Concrete;
using Infrastructer.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//AddDb
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr_Api")));

//AddIdentity
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddRoles<AppRole>()
  .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => {
    builder.RegisterModule(new DependecyResolver());
});

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

//CORS.
app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyOrigin()
                  .AllowAnyMethod());

////RunOnce
//var scope = app.Services.CreateScope();
//var userManagers = (UserManager<AppUser>)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>));
//ForLogin.AddSuperUserAsync(userManagers);

app.Run();