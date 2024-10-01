using Devblog_Library.BLL;
using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Devblog_Library.Repositories;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
{
    options.LoginPath = "/";
    options.AccessDeniedPath = "/Login";
});

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeFolder("/admin");
    options.Conventions.AuthorizePage("/Index");
}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

builder.Services.AddSingleton<ILogin, Login>()
    .AddSingleton<IRepo<BlogPost>, BlogPostRepo>()
    .AddSingleton<IRepo<Review>, ReviewRepo>()
    .AddSingleton<IRepo<Project>, ProjectRepo>()
    .AddSingleton<IPersonRepo, PersonRepo>()
    .AddSingleton<IBlogView, BlogView>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "",
    OnPrepareResponse = ctx =>
    {
        if (ctx.Context.Request.Path.StartsWithSegments("/admin"))
        {
            // Block access to static files in the /admin folder
            ctx.Context.Response.StatusCode = StatusCodes.Status403Forbidden;
            ctx.Context.Response.ContentType = "text/plain";
            ctx.Context.Response.WriteAsync("Forbidden");
        }
    }
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();