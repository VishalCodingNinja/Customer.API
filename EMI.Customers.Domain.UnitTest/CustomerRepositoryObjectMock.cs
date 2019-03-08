using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Enums;
using EMI.Customers.Domain.Models;

namespace EMI.Customers.Domain.UnitTest
{
	public class CustomerRepositoryObjectMock
	{

		public static IEnumerable<Customer> CustomersWithItems { get; set; } = new List<Customer>()
		{
			new Customer()
			{
				Id = 1,
				Name = "Vishal",
				Address = "Bareilly",
				CustomerType = CustomerType.DiamondCustomer,
				ItemPurchase = new List<Item>()
				{
					new Item()
					{
						Id = 1,
						Name = "bottle",
						Cost = 8900
					},
					new Item()
					{
						Id = 2,
						Name = "TShirt",
						Cost = 10000
					}
				}
			},
			new Customer()
			{
				Id = 2,
				Name = "Sumit",
				Address = "Bihar",
				CustomerType = CustomerType.GoldCustomer,
				ItemPurchase = new List<Item>()
				{
					new Item()
					{
						Id = 3,
						Name = "Shirts",
						Cost = 20000
					}
				}
			},
			new Customer()
			{
				Id = 3,
				Name = "Prasobh",
				Address = "Wayanad",
				CustomerType = CustomerType.SilverCustomer,
				ItemPurchase = new List<Item>()
				{
					new Item()
					{
						Id = 4,
						Name = "WaterBottles",
						Cost = 400
					}
				}
			}

		};
		public static List<Item> Items { get; set; } = new List<Item>
		{

			new Item()
			{
				Id = 1,
				Name = "bottle",
				Cost = 8900
			},
			new Item()
			{
				Id = 2,
				Name = "TShirt",
				Cost = 10000
			},
			new Item()
			{
				Id = 3,
				Name = "Shirts",
				Cost = 20000
			},


			new Item()
			{
				Id = 4,
				Name = "WaterBottles",
				Cost = 400
			}
		};

		public static List<ItemDto> ItemsDto { get; set; } = new List<ItemDto>
		{

			new ItemDto()
			{
				Id = 1,
				Name = "NokiaLumia",
				Cost = 8900
			},
			new ItemDto()
			{
				Id = 2,
				Name = "TShirt",
				Cost = 10000
			},
			new ItemDto()
			{
				Id = 2,
				Name = "Shirts",
				Cost = 20000
			},


			new ItemDto()
			{
				Id = 3,
				Name = "WaterBottles",
				Cost = 400
			}
		};


		public static IEnumerable<Customer> Customers { get; set; } = new List<Customer>()
		{
			new Customer()
			{
				Id = 1,
				Name = "Vishal",
				Address = "Bareilly",
				CustomerType = CustomerType.DiamondCustomer
			},
			new Customer()
			{
				Id = 2,
				Name = "Sumit",
				Address = "Bihar",
				CustomerType = CustomerType.GoldCustomer
			},
			new Customer()
			{
				Id = 3,
				Name = "Prasobh",
				Address = "Wayanad",
				CustomerType = CustomerType.SilverCustomer
			}
		};

		internal static Task<Customer> GetItemCustomer(int customerId, bool v)
		{
			throw new NotImplementedException();
		}

		public static async Task<IEnumerable<Item>> GetItemsAsync()
		{
			return Items;
		}

		public static async Task<Item> GetItemAsync(int itemId)
		{
			return Items.FirstOrDefault(i => i.Id == itemId);
		}
		public static async Task<Customer> GetCustomer(int customerId, bool includePointOfInterest)
		{
			if (customerId == 0 || Customers.FirstOrDefault(s => s.Id == customerId) == null) return null;
			if (includePointOfInterest)
				return CustomersWithItems.FirstOrDefault(e => e.Id == customerId);
			return Customers.FirstOrDefault(e => e.Id == customerId);
		}
	}
}
