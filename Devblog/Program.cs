using Devblog_Library.BLL;
using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Devblog_Library.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add JSON options to support incoming JSON
builder.Services.AddControllers();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Error";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Works for HTTP in development
        options.Cookie.SameSite = SameSiteMode.Lax;  // Allow cross-page navigation
        options.Cookie.Path = "/";  // Ensure cookie is valid for the entire site
    });

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeFolder("/admin");
}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

builder.Services.AddSingleton<IRepo<BlogPost>, BlogPostRepo>()
    .AddSingleton<IRepo<Review>, ReviewRepo>()
    .AddSingleton<IRepo<Project>, ProjectRepo>()
    .AddSingleton<IPersonRepo, PersonRepo>()
    .AddSingleton<IBlogView, BlogView>()
    .AddSingleton<ITagRepo, TagRepo>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        // log out any authenticated users when the application starts
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    await next.Invoke();
});

app.Use(async (context, next) =>
{
    // Ensure cookies have SameSite=Lax and are HttpOnly
    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,
        Secure = CookieSecurePolicy.SameAsRequest == CookieSecurePolicy.Always,
        SameSite = SameSiteMode.Lax,
        Path = "/"
    };

    // Apply the correct cookie options to any existing cookies
    if (context.Request.Cookies.ContainsKey(".AspNetCore.Cookies"))
    {
        var cookieValue = context.Request.Cookies[".AspNetCore.Cookies"];
        context.Response.Cookies.Append(".AspNetCore.Cookies", cookieValue, cookieOptions);
    }

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed errors during development
}
else
{
    app.UseExceptionHandler("/Error"); // Custom error handler for production
    app.UseHsts(); // Use HTTPS in production
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