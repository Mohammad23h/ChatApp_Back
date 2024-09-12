using ChatApp.Api.Extentions;
using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using ChatApp.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PusherServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                 )
    );

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();

var pusherOptions = new PusherOptions
{
Cluster = builder.Configuration["Pusher:Cluster"],
Encrypted = true
};

var pusher = new Pusher(
    builder.Configuration["Pusher:AppId"],
    builder.Configuration["Pusher:Key"],
    builder.Configuration["Pusher:Secret"],
    pusherOptions
);

    builder.Services.AddSingleton(pusher);
    builder.Services.AddCustomJwtAuth(builder.Configuration);



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
