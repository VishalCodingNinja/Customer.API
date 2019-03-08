using EMI.Customers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Customers.Domain.Repository
{
	public interface ICustomerRepository
	{
		Task<bool> CustomerExists(int id);
		Task<IEnumerable<Customer>> GetCustomers();
		Task<Customer> GetCustomer(int customerId, bool includeItem);
		Task<Item> GetItemForCustomer(int customerId, int itemId);
		Task<IEnumerable<Item>> GetItemsForCustomer(int customerId);
		void AddItemForCustomer(int customerId, Item item);
		Task<bool> SaveAsync();
		void DeleteItem(Item item);
	}
}
