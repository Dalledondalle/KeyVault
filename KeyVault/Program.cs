using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeyVault.Data;
using KeyVault.Areas.Identity.Data;
//using IdentityProject.Data;
//using IdentityProject.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("IdentityProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityProjectContextConnection' not found.");

//Instanciate the service for the Identity and MySql
builder.Services.AddDbContext<KeyVaultContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

builder.Services.AddDefaultIdentity<KeyVaultUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<KeyVaultContext>();


//To make it accesible externally. Should be in config file or something like that, but for this project, IP is just hardcoded
app.Urls.Add("http://62.141.38.222:9500");
app.Urls.Add("http://localhost:9500");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
