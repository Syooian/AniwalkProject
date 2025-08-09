using AniwalkServer;
using AniwalkServer.Controllers;
using AniwalkServer.Data;
using AniwalkServer.Filters;
using AniwalkServer.Models;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LogFilter>();
});

// Add services to the container.
builder.Services.AddControllersWithViews(Options =>
{
    //Options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); //啟用防止CSRF攻擊
    //Options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter()); //註冊驗證過濾器，所有頁面都需要登入
});

builder.Services.AddDbContext<AniwalkDBContext>(Options =>
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionStrings")));

#region 自訂Services
builder.Services.AddScoped<VisitsServices>();
builder.Services.AddScoped<PhotoServices>();
builder.Services.AddScoped<MembersServices>();
#endregion

//註冊 Cookie Authentication
builder.Services.AddAuthentication(LoginController.AuthenticationScheme).AddCookie(LoginController.AuthenticationScheme, Options =>
{
    Options.LoginPath = "/Login/Login"; // 設定登入頁面路徑(若需登入而未登入時則強制導到此路徑)
    Options.LogoutPath = "/Login/Logout"; // 設定登出頁面路徑
    Options.AccessDeniedPath = "/Home/Index"; // 設定存取拒絕頁面路徑(若已登入但角色權限不符則強制導到此路徑)
});

var app = builder.Build();

//建立SeedData初始化
using (var Scope = app.Services.CreateScope())
{
    var Service = Scope.ServiceProvider;

    SeedData.Initialize(Service);
}

#region 錯誤Handler
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
//app.UseStatusCodePagesWithReExecute("/Home/Error"); //直接顯示錯誤頁面，不導向
//app.UseStatusCodePagesWithRedirects("/Home/Error"); //重新導向到錯誤頁面
#endregion

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
