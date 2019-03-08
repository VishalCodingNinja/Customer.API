using System;
using System.Collections.Generic;
using System.Text;
using EMI.Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMI.Customers.Domain.AllDbContext
{
	public interface ICustomerDbContext
	{
		DbSet<Customer> Customers { get; set; }
		DbSet<Item> Items { get; set; }
	}
}
