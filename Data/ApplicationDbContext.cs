﻿using Microsoft.EntityFrameworkCore;
using MintCartWebApi.DBModels;

namespace MintCartWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
