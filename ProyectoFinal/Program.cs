using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();//agregado para Login


//Configura Nuestro arranque de servicios con la conexion de la BD
builder.Services.AddDbContext<AutoCarDBcontext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion"));
    }
    );

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AutoCarDBcontext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();//login


app.Run();
