using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using MuscleMealUI.Models;
using DynamicData.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
namespace MuscleMealUI
{
    public class MyDbContext : DbContext
    {
        public string DbPath { get; }
        public MyDbContext()
        {

            var folder = Environment.SpecialFolder.LocalApplicationData;
            //if you want the data to be in the root of the folder
            //var path = Directory.GetCurrentDirectory();
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "musclemeal.db");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //string? username = Environment.GetEnvironmentVariable("ORA_USER");
        //    //string? password = Environment.GetEnvironmentVariable("ORA_PASSWD");`
        //    //string? host = Environment.GetEnvironmentVariable("ORA_Host");
        //    //string? serviceName = Environment.GetEnvironmentVariable("sn");
        //    //string? serviceName = "pdbora19c.dawsoncollege.qc.ca";

        //    //optionsBuilder.UseOracle($"User Id={username}; Password={password}; Data Source = 198.168.52.211:1521/pdbora19c.dawsoncollege.qc.ca;");
           
        //}



        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        private static MyDbContext? _instance = null;
       
       
        public static MyDbContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MyDbContext();
            }
            return _instance;
        }

    }
}
// dotnet add package Oracle.ManagedDataAccess.Core --version 3.21.130
// dotnet add package Oracle.EntityFrameworkCore --version 7.21.13

// Every time we make a change to a class or the objects
// dotnet ef migrations add fiest
// dotnet ef database update
