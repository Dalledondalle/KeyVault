using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeyVault.Data;
using KeyVault.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("KeyVaultContextConnection") ?? throw new InvalidOperationException("Connection string 'KeyVaultContextConnection' not found.");

//Instanciate the service for the Identity and MySql
builder.Services.AddDbContext<KeyVaultContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));


//at sætte det her til false er at man ikke skal confirm email
//I det her projekt gøres det fordi der ikke vil blive sat en mail server op
builder.Services.AddDefaultIdentity<KeyVaultUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<KeyVaultContext>();

// Add services to the container.
builder.Services.AddRazorPages();

//To make it accesible externally. Should be in config file or something like that, but for this project, IP is just hardcoded
var app = builder.Build();
//app.Urls.Add("http://62.141.38.222:9500");
//app.Urls.Add("http://localhost:9500");

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
