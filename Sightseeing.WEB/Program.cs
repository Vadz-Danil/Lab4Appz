using Microsoft.AspNetCore.Authentication.Cookies;
using Sightseeing.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSightSeeingDal(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddSightSeeingBll();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseRouting();
app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Run();