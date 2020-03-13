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
        public DbSet<Search> Searches { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}