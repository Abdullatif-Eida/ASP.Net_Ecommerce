using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Areas.Identity.Data
{
    public class EcommerceContext : IdentityDbContext<AppUser>
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Brand> Brands { get; set; }
        public DbSet<Models.Attachment> Attachments { get; set; }
        public DbSet<Models.Categories> Categories { get; set; }
        public DbSet<Models.Items> Items { get; set; }
        public DbSet<Models.Orders> Orders { get; set; }
        public DbSet<Models.ContactUs> ContactUs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
