using EMI.Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMI.Customers.Domain.AllDbContext
{
	public class CustomerDbContext : DbContext,ICustomerDbContext
	{

		public CustomerDbContext()
		{
			
		}
		public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
		{

		}

		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Item> Items { get; set; }

	}
}