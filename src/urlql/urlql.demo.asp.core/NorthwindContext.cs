using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using urlql.demo.asp.core.Entities;

namespace urlql.demo.asp.core
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public NorthwindContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./northwind.sqlite");
        }
    }
}

