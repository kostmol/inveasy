using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Inveasy.Data;
using Inveasy.Services;
using Inveasy.Controllers;
using Inveasy.Models;
using Inveasy.Services.ProjectServices;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InveasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InveasyContext") ?? throw new InvalidOperationException("Connection string 'InveasyContext' not found.")));

// Add logging service
builder.Services.AddLogging(loggingBuilder => { });

// Add db services
builder.Services.AddDatabaseServices();

// Add services for status messages
builder.Services.AddStatusServices();

// Add a hosted service to run the trending score algorithm periodically
builder.Services.AddSingleton<IHostedService, TrendingProjectHostedService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Inveasy.Session";
    options.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
