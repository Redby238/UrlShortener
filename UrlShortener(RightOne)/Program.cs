using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortenerRightOne.Models;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(8, 3, 0));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();



var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Seed data
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    await SeedDataAsync(roleManager, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Program>()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<UserManager<IdentityUser>>();
                    services.AddScoped<RoleManager<IdentityRole>>();
                });
        });

 async Task SeedDataAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    await CreateRolesAsync(roleManager);
    await CreateUsersAsync(userManager);
    
}

 static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
    await roleManager.CreateAsync(new IdentityRole("User"));
}

static async Task CreateUsersAsync(UserManager<IdentityUser> userManager)
{
    var adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" };
    var ordinaryUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com" };
    await userManager.AddToRoleAsync(adminUser, "Admin");
    await userManager.AddToRoleAsync(ordinaryUser, "User");
    await userManager.CreateAsync(adminUser, "Admin123!");
    await userManager.CreateAsync(ordinaryUser, "User123!");
}


using (var serviceScope = app.Services.CreateScope())
{
   
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    Task.Run(async () => await SeedDataAsync(roleManager, userManager)).Wait();
}





using (MySqlConnection connection = new MySqlConnection(connectionString))
{
    try
    {
        connection.Open();
        Console.WriteLine("Connected to the database.");
    }
    catch (MySqlException ex)
    {
        Console.WriteLine("Error connecting to the database: " + ex.Message);
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
