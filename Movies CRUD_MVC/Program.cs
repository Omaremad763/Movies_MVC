using Microsoft.EntityFrameworkCore;
using Movies_CRUD_MVC.Models;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// configre db 
var connectionstring = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<Movies_Context>(Options =>
Options.UseSqlServer(connectionstring));

builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar=true,
    PositionClass=ToastPositions.TopRight,
    PreventDuplicates=true,
    CloseButton=true
      
});

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
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
