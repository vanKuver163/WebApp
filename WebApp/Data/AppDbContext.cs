using Microsoft.EntityFrameworkCore;
using WebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Album>Album { get; set; }
        public DbSet<UserCollection>UserCollection { get; set; }
    }
}
