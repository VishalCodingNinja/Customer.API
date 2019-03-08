using System.Collections.Generic;
using System.Threading.Tasks;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Repository;

namespace EMI.Customers.Domain.Services
{
	public class CustomerService:ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;

		public CustomerService(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<bool> CustomerExists(int id)
		{
			return await _customerRepository.CustomerExists(id);
		}

		public async Task<IEnumerable<Customer>> GetCustomers()
		{
			return await _customerRepository.GetCustomers();
		}

		public async Task<Customer> GetCustomer(int customerId, bool includeItem)
		{
			return await _customerRepository.GetCustomer(customerId, includeItem);
		}

		public async Task<Item> GetItemForCustomer(int customerId, int itemId)
		{
			return await _customerRepository.GetItemForCustomer(customerId, itemId);
		}

		public async Task<IEnumerable<Item>> GetItemsForCustomer(int customerId)
		{
			return await _customerRepository.GetItemsForCustomer(customerId);
		}

		public void AddItemForCustomer(int customerId, Item item)
		{
			 _customerRepository.AddItemForCustomer(customerId, item);
		}

		public async Task<bool> SaveAsync()
		{
			return await ( _customerRepository.SaveAsync() );
		}

		public void DeleteItem(Item item)
		{
			_customerRepository.DeleteItem(item);
		}
	}
}
