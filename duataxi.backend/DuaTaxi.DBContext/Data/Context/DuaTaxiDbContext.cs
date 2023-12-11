﻿using DuaTaxi.Entities.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DuaTaxi.DBContext.Data.Context
{
    public class DuaTaxiDbContext : IdentityDbContext<AppUser>
    {
        public DuaTaxiDbContext(DbContextOptions<DuaTaxiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { 
                Name = DuaTaxi.Entities.Core.Enums.Roles.Consumer,
                NormalizedName = DuaTaxi.Entities.Core.Enums.Roles.Consumer.ToUpper() });
        }
    }
}
