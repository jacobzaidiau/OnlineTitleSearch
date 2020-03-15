using OnlineTitleSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineTitleSearch.Context
{
    public class SearchDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Search> Searches { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}