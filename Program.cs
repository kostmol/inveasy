using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Inveasy.Data;
using Inveasy.Services;
using Inveasy.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InveasyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InveasyContext") ?? throw new InvalidOperationException("Connection string 'InveasyContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add service for users
//builder.Services.AddScoped<UsersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
