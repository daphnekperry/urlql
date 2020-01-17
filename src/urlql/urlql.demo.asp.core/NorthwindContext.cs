using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using urlql.demo.asp.core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace urlql.demo.asp.core
{
    public class NorthwindContext : DbContext
    {
        public static readonly ILoggerFactory LoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public DbSet<Customer> Customers { get; set; }

        public NorthwindContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory)  //tie-up DbContext with LoggerFactory object
                .EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite("Filename=./northwind.sqlite");
        }
    }
}

