//using System;
//using System.Collections.Generic;
//using System.Text;
//using EMI.Customers.Domain.AllDbContext;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace EMI.Customers.Domain.AllDbContext
//{
//	public class CustomerBbFactory : IDesignTimeDbContextFactory<CustomerDbContext>
//	{
//		public CustomerDbContext CreateDbContext(string[] args)
//		{
//			var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
//			optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLlocaldb;Database=ASPNETApplicationDB;Trusted_Connection=True;MultipleActiveResultSets=true");

//			return new CustomerDbContext(optionsBuilder.Options);
//		}
//	}

//}
