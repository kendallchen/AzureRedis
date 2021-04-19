using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data.Model;
using StackExchange.Redis;

namespace Data
{
    public class RedisCustomerData : ICustomerData
    {
        public AzureRedisDbContext DbContext { get; }

        public RedisCustomerData(AzureRedisDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Customer> Add(Customer customer)
        {
            DbContext.Add(customer);
            await DbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<int> Delete(int customerId)
        {
            Customer c = await this.GetCustomerById(customerId);
            if (c != null)
            {
                this.DbContext.Remove(c);
                await DbContext.SaveChangesAsync();
                return customerId;
            }
            return -1;
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("dotnetleadazureredis.redis.cache.windows.net:6380,password=,ssl=True,abortConnect=False");
            // select a database (by default, DB = 0)
            IDatabase db = connection.GetDatabase();
            // run a command, in this case a GET
            RedisValue myVal = db.StringGet("mykey");

            return await DbContext.Customer.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            Customer c = await this.DbContext.Customer.FindAsync(id);
            if (c != null)
                return c;
            return null;
        }

        public async Task<Customer> Update(Customer customer)
        {
            Customer c = await GetCustomerById(customer.CustomerId);
            if (c != null)
            {
                c.FirstName = customer.FirstName;
                c.LastName = customer.LastName;
                await DbContext.SaveChangesAsync();
                return c;
            }
            return null;
        }
    }
}
