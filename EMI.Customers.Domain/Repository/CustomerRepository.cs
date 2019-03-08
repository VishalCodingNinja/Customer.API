using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMI.Customers.Domain.AllDbContext;
using EMI.Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMI.Customers.Domain.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly CustomerDbContext _context;

		public CustomerRepository(CustomerDbContext context)
		{
			_context = context;
		}


		public async Task<IEnumerable<Customer>> GetCustomers()
		{
			return await _context.Customers.OrderBy(c => c.Name).ToListAsync();
		}


		public async Task<Customer> GetCustomer(int customerId, bool includeItem)
		{	
			if (includeItem)
				return await _context.Customers.Include(c => c.ItemPurchase)
							   .Where(c => c.Id == customerId).FirstOrDefaultAsync();
			return await _context.Customers.FirstOrDefaultAsync(e => e.Id == customerId);
		}

		
		public async Task<Item> GetItemForCustomer(int customerId, int itemId) => await _context.Items.FirstOrDefaultAsync(p => p.CustomerId == customerId && p.Id == itemId);

		public async Task<IEnumerable<Item>> GetItemsForCustomer(int customerId)
		{
			return await _context.Items.Where(p => p.CustomerId == customerId).ToListAsync();
		}

		public async Task<bool> CustomerExists(int id)
		{
			return await _context.Customers.AnyAsync(c => c.Id == id);
		}

		public async void AddItemForCustomer(int customerId, Item item)
		{
			var customer = await GetCustomer(customerId, false);
				customer.ItemPurchase.Add(item);
		}

		public async Task<bool> SaveAsync()
		{
			return (await _context.SaveChangesAsync()>=0);
		}

		public void DeleteItem(Item item)
		{
			_context.Remove(item);
		}
	}
}
