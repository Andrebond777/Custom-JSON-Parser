using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrust_Test_Task.Entities;

namespace DevTrust_Test_Task.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ApplicationDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DevTrustDb4;Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
