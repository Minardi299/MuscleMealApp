using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using MuscleMealUI.Models;
namespace MuscleMealUI
{
    public class MyDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? username = Environment.GetEnvironmentVariable("ORA_USER");
            string? password = Environment.GetEnvironmentVariable("ORA_PASSWD");
            string? host = Environment.GetEnvironmentVariable("ORA_Host");
            string? serviceName = Environment.GetEnvironmentVariable("sn");
            //string? serviceName = "pdbora19c.dawsoncollege.qc.ca";

            optionsBuilder.UseOracle($"User Id={username}; Password={password}; Data Source = 198.168.52.211:1521/pdbora19c.dawsoncollege.qc.ca;");

        }



        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

    }
}
// dotnet add package Oracle.ManagedDataAccess.Core --version 3.21.130
// dotnet add package Oracle.EntityFrameworkCore --version 7.21.13

// Every time we make a change to a class or the objects
// dotnet ef migrations add fiest
// dotnet ef database update
