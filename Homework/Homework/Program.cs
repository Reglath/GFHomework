using Homework.Contexts;
using Homework.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddScoped<Service>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSession();

var app = builder.Build();
app.UseSession();
app.UseStaticFiles();
app.MapControllers();

app.Run();