using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Data.Model;

namespace Data
{
    public class AzureRedisDbContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }

        public AzureRedisDbContext(DbContextOptions<AzureRedisDbContext> options)
            : base(options)
        {
        }
    }
}
