using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMI.Customers.Domain.AllDbContext;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EMI.Customers.Domain.UnitTest
{
	public class CustomerRepositoryTest
	{ 
		private readonly DbContextOptions<CustomerDbContext> _options;

		public CustomerRepositoryTest()
		{
			_options = new DbContextOptionsBuilder<CustomerDbContext>()
				.UseInMemoryDatabase(databaseName: "Add_Customers_to_database")
				.Options;
			using (var context = new CustomerDbContext(_options))
			{
				if (context.Customers.Any()) return;
				context.AddRange(CustomerRepositoryObjectMock.CustomersWithItems);
				context.SaveChanges();
			}
		}
		

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_noInput_ResultCustomers()
		{
			Mapper.Reset();
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var result = await repository.GetCustomers();
				Assert.IsAssignableFrom<IEnumerable<Customer>>(result);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_customerId_ReturnCustomer()
		{
			const int customerId = 1;
			const bool includeItem = false;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var customer = await context.Customers.Include(c => c.ItemPurchase)
					.Where(c => c.Id == customerId).FirstOrDefaultAsync();
				var result = await repository.GetCustomer(customerId,includeItem);
				Assert.IsAssignableFrom<Customer>(result);
				Assert.IsType<Customer>(customer);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_customerId_ReturnNull()
		{
			const int customerId = 1000;
			const bool includeItem = false;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var customer = await context.Customers.Include(c => c.ItemPurchase)
					.Where(c => c.Id == customerId).FirstOrDefaultAsync();
				var result = await repository.GetCustomer(customerId, includeItem);
				Assert.Null(result);
				Assert.Null(customer);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_customerId_ReturnCustomerWithItem()
		{
			const int customerId = 1;
			const bool includeItem = true;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var customer = await context.Customers.Include(c => c.ItemPurchase)
					.Where(c => c.Id == customerId).FirstOrDefaultAsync();
				var result = await repository.GetCustomer(customerId, includeItem);
				Assert.IsAssignableFrom<Customer>(result);
				Assert.IsType<Customer>(customer);
				Assert.NotEqual(0,customer.ItemPurchase.Count);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemForCustomer_customerIdAndItemId_ReturnItem()
		{
			const int customerId = 1;
			const int itemId = 1;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var item = await context.Items.FirstOrDefaultAsync(p => p.CustomerId == customerId && p.Id == itemId);
				var result = await repository.GetItemForCustomer(customerId, itemId);
				Assert.IsAssignableFrom<Item>(result);
				Assert.IsType<Item>(item);
				Assert.Equal(item,result);
			}

		}


		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemsForCustomer_CustomerId_ReturnItems()
		{
			const int customerId = 1;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var items = await context.Items.Where(p => p.CustomerId == customerId).ToListAsync();
				var result = await repository.GetItemsForCustomer(customerId);
				Assert.IsAssignableFrom<IEnumerable<Item>>(result);
				Assert.IsAssignableFrom<IEnumerable<Item>>(items);
				Assert.Equal(items, result);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemsForCustomer_CustomerId_ReturnZeroCount()
		{
			const int customerId = 10000;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var items = await context.Items.Where(p => p.CustomerId == customerId).ToListAsync();
				var result = await repository.GetItemsForCustomer(customerId);
				Assert.Equal(items.Count(), result.Count());
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task CustomerExists_CustomerId_ReturnCustomer()
		{
			const int customerId = 1;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var items = await context.Customers.AnyAsync(c => c.Id == customerId);
				var result = await repository.CustomerExists(customerId);
				Assert.IsType<bool>(items);
				Assert.IsType<bool>(result);
				Assert.True(result);
				Assert.True(items);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task CustomerExists_CustomerId_ReturnFalse()
		{
			const int customerId = 1111;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var items = await context.Customers.AnyAsync(c => c.Id == customerId);
				var result = await repository.CustomerExists(customerId);
				Assert.IsType<bool>(items);
				Assert.IsType<bool>(result);
				Assert.False(result);
				Assert.False(items);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task SaveAsync_CustomerId_ReturnTrue()
		{
			const int customerId = 1000;
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var items = await context.Customers.AnyAsync(c => c.Id == customerId);
				var result = await repository.CustomerExists(customerId);
				Assert.IsType<bool>(items);
				Assert.IsType<bool>(result);
				Assert.False(result);
				Assert.False(items);
			}
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task SaveAsync_CustomerIdAndItem_ReturnTrue()
		{
			const int customerId = 1;
			var item=new Item()
			{
				Id = 100,
				Name = "HeadPhone",
				Cost = 40000
			};
			using (var context = new CustomerDbContext(_options))
			{
				var repository = new CustomerRepository(context);
				var customer = await context.Customers.Include(c => c.ItemPurchase)
					.Where(c => c.Id == customerId).FirstOrDefaultAsync();
				 customer.ItemPurchase.Add(item);
				var isSaved =  (await context.SaveChangesAsync() >= 0);
				 repository.AddItemForCustomer(customerId,item);
				var isSavedInRepo=await repository.SaveAsync();
				Assert.True(isSaved);
				Assert.True(isSavedInRepo);
			}
		}
	}
}
