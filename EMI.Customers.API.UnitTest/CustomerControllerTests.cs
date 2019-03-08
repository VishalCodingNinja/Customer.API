using System.Linq;
using System.Threading.Tasks;
using EMI.Customers.API.Controllers;
using EMI.Customers.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EMI.Customers.API.UnitTest
{
	public class CustomerControllerTests
	{
		private readonly CustomerController _customerController;
		private readonly Mock<ICustomerService> _customerService;
		 public CustomerControllerTests()
		{
			AutoMapperConfig.Initialize();
			_customerService=new Mock<ICustomerService>();
			_customerController=new CustomerController(_customerService.Object);
		}

		[Fact]
		[Trait("Category","UnitTest")]
		public  async Task GetCustomersAsync_void_OkResult()
		{
			_customerService.Setup(e => e.GetCustomers()).Returns(Task.FromResult(CustomerDataStore.Customers));
			var returnResult=await _customerController.GetCustomersAsync();
			Assert.IsType<OkObjectResult>(returnResult);
			
		
		}

		[Fact]
		[Trait("Category","UnitTest")]
		public async Task GetCustomerAsync_void_OkResult()
		{

			const int customerId = 1;
			_customerService.Setup(e => e.GetCustomer(customerId, false))
				.Returns(Task.FromResult(CustomerDataStore.Customers.FirstOrDefault(e => e.Id == customerId)));
			var returnResult = await _customerController.GetCustomerAsync(customerId, false);
			Assert.IsType<OkObjectResult>(returnResult);
		
		}

		[Fact]
		[Trait("Category","UnitTest")]
		public async Task GetCustomerAsync_IdAndBool_NotFoundResult()
		{
		
			const int customerId = 100;
			_customerService.Setup(e => e.GetCustomer(customerId, false))
				.Returns(Task.FromResult(CustomerDataStore.Customers.FirstOrDefault(e => e.Id == customerId)));
			var returnResult = await _customerController.GetCustomerAsync(customerId, false);
			Assert.IsType<NotFoundResult>(returnResult);

		}
	}
}
