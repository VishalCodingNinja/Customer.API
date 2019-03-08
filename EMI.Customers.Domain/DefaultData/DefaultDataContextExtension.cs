using System;
using System.Collections.Generic;
using System.Text;
using EMI.Customers.Domain.AllDbContext;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Enums;
using EMI.Customers.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace EMI.Customers.Domain.DefaultData
{
	public static class DefaultDataContextExtension
	{
		public static void GetSeedData(this CustomerDbContext context)
		{
			if (context.Customers.Any())
			{
				return;
			}
			else
			{
				var customers = new List<Customer>()
				{
					new Customer()
					{
						
						Name = "Vishal",
						Address = "Bareilly",
						CustomerType = CustomerType.DiamondCustomer,
						ItemPurchase = new List<Item>()
						{
							new Item()
							{
								
								Name = "Pants",
								Cost = 10000
							}
						}
					},
					new Customer()
					{
						
						Name = "Sumit",
						Address = "Bihar",
						CustomerType = CustomerType.GoldCustomer,
						ItemPurchase = new List<Item>()
						{
							new Item()
							{
								
								Name = "TShirts",
								Cost = 10000
							}
						}
					},
					new Customer()
					{
						
						Name = "Prasobh",
						Address = "Wayanad",
						CustomerType = CustomerType.SilverCustomer,
						ItemPurchase = new List<Item>()
						{
							new Item()
							{
								
								Name = "Shirts",
								Cost = 20000
							}
						}
					}
				};
				context.Customers.AddRange(customers);
				context.SaveChanges();
			}
		}
	}
}
