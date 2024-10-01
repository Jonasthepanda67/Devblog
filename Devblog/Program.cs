using Devblog_Library.BLL;
using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Devblog_Library.Repositories;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();