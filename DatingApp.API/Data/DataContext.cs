using System;
using System.Linq;
using DatingApp.API.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }
        public DbSet<User> Users { get; set; }
        override protected void OnModelCreating (ModelBuilder modelBuilder) { }
    }
}