using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener_RightOne_.Models;

namespace UrlShortenerRightOne.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Url> Urls { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
    }
}
