using AniwalkServerDBFirstTest.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AniwalkContext>(Options =>
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionStrings")));
builder.Services.AddDbContext<AniwalkContext_Extend>(Options =>
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionStrings")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
