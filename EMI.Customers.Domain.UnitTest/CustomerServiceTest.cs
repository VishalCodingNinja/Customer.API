using System.Collections.Generic;
using System.Threading.Tasks;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Repository;
using EMI.Customers.Domain.Services;
using Moq;
using Xunit;

namespace EMI.Customers.Domain.UnitTest
{
	public class CustomerServiceTest
	{
		private readonly ICustomerService _customerService;
		private readonly Mock<ICustomerRepository> _customerRepository;

		public CustomerServiceTest()
		{
			_customerRepository = new MockRepository(MockBehavior.Default).Create<ICustomerRepository>();
			_customerService = new CustomerService(_customerRepository.Object);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task CustomerExists_Id_ReturnTrue()
		{
			const int customerId = 1;
			_customerRepository.Setup(s => s.CustomerExists(customerId)).ReturnsAsync(true);
			var isSaved = await _customerService.CustomerExists(customerId);
			Assert.True(isSaved);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task CustomerExists_Id_ReturnFalse()
		{
			const int customerId = 1;
			_customerRepository.Setup(s => s.CustomerExists(customerId)).ReturnsAsync(false);
			var isSaved = await _customerService.CustomerExists(customerId);
			Assert.True(!isSaved);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_void_ReturnCustomers()
		{
			_customerRepository.Setup(s => s.GetCustomers()).ReturnsAsync(CustomerServiceObjectMock.Customers);
			var customers = await _customerService.GetCustomers();
			Assert.IsAssignableFrom<IEnumerable<Customer>>(customers);
		}


		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomers_CustomerId_true_ReturnCustomer()
		{
			const int customerId = 1;
			_customerRepository.Setup(s => s.GetCustomer(customerId, true))
				.Returns(CustomerServiceObjectMock.GetCustomer(customerId, true));
			var customer = await _customerService.GetCustomer(customerId, true);
			Assert.IsAssignableFrom<Customer>(customer);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetCustomer_CustomerId_true_ReturnNull()
		{
			const int customerId = 10000;
			_customerRepository.Setup(s => s.GetCustomer(customerId, true))
				.Returns(CustomerServiceObjectMock.GetCustomer(customerId, true));
			var customer = await _customerService.GetCustomer(customerId, true);
			Assert.Null(customer);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemForCustomer_customerItAndItemId_ReturnItem()
		{
			const int customerId = 1;
			const int itemId = 1;
			_customerRepository.Setup(s => s.GetItemForCustomer(customerId,itemId))
				.Returns(CustomerServiceObjectMock.GetItemCustomer(customerId,itemId));
			var item = await _customerService.GetItemForCustomer(customerId, itemId);
			Assert.NotNull(item);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task SaveAsync_TryToSaveContext_ReturnTrue()
		{
			_customerRepository.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			bool isSaved = await _customerService.SaveAsync();
			Assert.True(isSaved);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task SaveAsync_TryToSaveContext_ReturnFalse()
		{
			_customerRepository.Setup(s => s.SaveAsync()).ReturnsAsync(false);
			bool isSaved = await _customerService.SaveAsync();
			Assert.False(isSaved);
		}

	}
}