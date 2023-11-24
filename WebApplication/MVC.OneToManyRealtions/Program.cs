using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRealtions.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
string _connectinString = "Server=ELVIN_SARKAROV\\SQLEXPRESS;Database=AllUpDataBase;Trusted_Connection=True;";

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(_connectinString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
