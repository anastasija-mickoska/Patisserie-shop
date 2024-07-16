using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Patisserie.Areas.Identity.Data;
using Patisserie.Models;

namespace Patisserie.Data
{
    public class PatisserieContext : IdentityDbContext<PatisserieUser>
    {
        public PatisserieContext (DbContextOptions<PatisserieContext> options)
            : base(options)
        {
        }

        public DbSet<Patisserie.Models.Product> Product { get; set; } = default!;
        public DbSet<Patisserie.Models.Flavour> Flavour { get; set; } = default!;
        public DbSet<Patisserie.Models.Review> Review { get; set; } = default!;
        public DbSet<Patisserie.Models.Category> Category { get; set; } = default!;
        public DbSet<Patisserie.Models.ProductFlavour> ProductFlavours { get; set; } = default!;
        public DbSet<Patisserie.Models.UserProduct> UserProducts { get; set; } = default!;
    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductFlavour>()
            .HasOne<Flavour>(p => p.Flavour)
            .WithMany(p => p.ProductFlavours)
            .HasForeignKey(p => p.FlavourId);

            builder.Entity<ProductFlavour>()
            .HasOne<Product>(p => p.Product)
            .WithMany(p => p.ProductFlavours)
            .HasForeignKey(p => p.ProductId);

            builder.Entity<Review>()
            .HasOne<Product>(p => p.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(p => p.ProductId);

            builder.Entity<Product>()
            .HasOne<Category>(p => p.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.CategoryId);

            builder.Entity<UserProduct>()
            .HasOne<Product>(p => p.Product)
            .WithMany(p => p.UserProducts)
            .HasForeignKey(p => p.ProductId);

            builder.Entity<UserProduct>()
            .HasOne(p => p.User)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.UserId);
        }

    }
}
