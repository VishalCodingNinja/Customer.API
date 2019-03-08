using System.Threading.Tasks;
using EMI.Customers.API.Controllers;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Models;
using EMI.Customers.Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EMI.Customers.API.UnitTest
{
	public class ItemControllerTests
	{
		private readonly ItemController _itemController;
		private readonly Mock<ICustomerService> _customerService;

		
		public ItemControllerTests()
		{
			AutoMapperConfig.Initialize();
			_customerService = new Mock<ICustomerService>();
			_itemController = new ItemController(_customerService.Object);
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemsAsync_Int_OkObjectResult()
		{
			
			const int customerId = 1;
			//Act
			_customerService.Setup(u => u.CustomerExists(It.IsAny<int>())).Returns(Task.FromResult(true));
			_customerService.Setup(u => u.GetItemsForCustomer(customerId))
				.Returns(CustomerDataStore.GetItemsAsync());
			var okResult = await _itemController.GetItemsAsync(customerId);

			//Assert
			Assert.IsType<OkObjectResult>(okResult);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemsAsync_Int_NotFoundResult()
		{
			const int customerId = 1;
			_customerService.Setup(u => u.CustomerExists(It.IsAny<int>())).Returns(Task.FromResult(false));
			var notFound = await _itemController.GetItemsAsync(customerId);
			Assert.IsType<NotFoundResult>(notFound);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemAsync_IntAndInt_OkObjectResult()
		{
			
			const int customerId = 1;
			const int itemId = 1;
			_customerService.Setup(u => u.CustomerExists(It.IsAny<int>())).Returns(Task.FromResult(true));
			_customerService.Setup(u => u.GetItemForCustomer(customerId, itemId))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			var okResult = await _itemController.GetItemAsync(customerId, itemId);
			Assert.IsType<OkObjectResult>(okResult);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemAsync_Int_CustomerNotFoundResult()
		{
		
			const int customerId = 100;
			const int itemId = 100;
			_customerService.Setup(u => u.CustomerExists(customerId)).Returns(Task.FromResult(false));
			var notFoundResult = await _itemController.GetItemAsync(customerId, itemId);
			Assert.IsType<NotFoundResult>(notFoundResult);
		
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task GetItemAsync_Int_ItemNotFoundResult()
		{
		
			const int customerId = 100;
			const int itemId = 100;
			_customerService.Setup(u => u.CustomerExists(customerId)).Returns(Task.FromResult(true));
			_customerService.Setup(u => u.GetItemForCustomer(customerId, itemId))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			var notFoundResult = await _itemController.GetItemAsync(customerId, itemId);
			Assert.IsType<NotFoundResult>(notFoundResult);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task AddItemAsync_IntAndItem_CreatedAtRoute()
		{
			
			const int customerId = 1;
			var item = new ItemsForCreationDto()
			{
				Name = "RedMi4",
				Cost = 6000
			};
			_customerService.Setup(u => u.CustomerExists(customerId)).ReturnsAsync(true);
			_customerService.Setup(u => u.AddItemForCustomer(customerId, It.IsAny<Item>()));
			_customerService.Setup(u => u.SaveAsync()).ReturnsAsync(true);
			var resultRedirect = await _itemController.AddItemAsync(customerId, item);
			Assert.IsType<CreatedAtRouteResult>(resultRedirect);
		
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task AddItemAsync_IntAndItem_CustomerNotFound()
		{
			
			_customerService.Setup(u => u.CustomerExists(It.IsAny<int>())).Returns(Task.FromResult(false));
			_customerService.Setup(u => u.AddItemForCustomer(It.IsAny<int>(), It.IsAny<Item>()));
			_customerService.Setup(u => u.SaveAsync()).ReturnsAsync(true);
			var customerNotFoundResult =
				await _itemController.AddItemAsync(It.IsAny<int>(), It.IsAny<ItemsForCreationDto>());
			Assert.IsType<NotFoundResult>(customerNotFoundResult);
		
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task AddItemAsync_IntAndItem_InternalServerError()
		{
			
			_customerService.Setup(u => u.CustomerExists(It.IsAny<int>())).Returns(Task.FromResult(true));
			_customerService.Setup(u => u.AddItemForCustomer(It.IsAny<int>(), It.IsAny<Item>()));
			_customerService.Setup(u => u.SaveAsync()).ReturnsAsync(false);
			var customerNotFoundResult =
				await _itemController.AddItemAsync(It.IsAny<int>(), It.IsAny<ItemsForCreationDto>());
			Assert.Equal(500, ((ObjectResult) customerNotFoundResult).StatusCode);
			Assert.Equal("A Problem Happen While handling our request", ((ObjectResult) customerNotFoundResult).Value);
		
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task UpdateItemsAsync_intAndItemForUpdate_NoContent()
		{
		
			const int customerId = 1;
			const int itemId = 1;
			var itemForUpdate = new ItemsForUpDateDto()
			{
				Name = "Laptop",
				Cost = 500000
			};
			_customerService.Setup(s => s.CustomerExists(customerId))
				.ReturnsAsync(true);
			_customerService.Setup(s => s.GetItemForCustomer(customerId, itemId))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			var noContent = await _itemController.UpdateItemAsync(customerId, itemId, itemForUpdate);
			Assert.IsType<NoContentResult>(noContent);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task UpdateItemsAsync_intAndItemForUpdate_BadRequestForNullObject()
		{
			
			const int customerId = 1;
			const int itemId = 1;
			_customerService.Setup(s => s.CustomerExists(customerId))
				.ReturnsAsync(true);
			_customerService.Setup(s => s.GetItemForCustomer(customerId, itemId))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			var noContent = await _itemController.UpdateItemAsync(customerId, itemId, null);
			Assert.IsType<BadRequestResult>(noContent);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task PartiallyUpdateItemsDetailsAsync_WithCustomerIdAndItemIdAndPatchDocument_ReturnsNoContent()
		{
			
			const int customerId = 1;
			const int itemId = 1;
	
			_customerService.Setup(s => s.CustomerExists(customerId)).Returns(Task.FromResult(true));
			_customerService.Setup(s => s.GetItemForCustomer(customerId, itemId))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			var noContent = await _itemController.PartiallyUpdateItemsDetailsAsync(customerId,itemId, new JsonPatchDocument<ItemsForUpDateDto>());
			Assert.IsType<NoContentResult>(noContent);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task DeleteItemAsync_customerIdAndItemId_ReturnsNoContent()
		{
			
			const int customerId = 1;
			const int itemId = 1;
			_customerService.Setup(s => s.CustomerExists(It.IsAny<int>())).ReturnsAsync(true);
			_customerService.Setup(s => s.GetItemForCustomer(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			_customerService.Setup(s => s.DeleteItem(It.IsAny<Item>()));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			var noContent = await _itemController.DeleteItemAsync(It.IsAny<int>(), It.IsAny<int>());
			Assert.IsType<NoContentResult>(noContent);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task DeleteItemAsync_customerAndItemId_ReturnsNotFound()
		{
			
			const int itemId = 1000;
			_customerService.Setup(s => s.CustomerExists(It.IsAny<int>())).ReturnsAsync(true);
			_customerService.Setup(s => s.GetItemForCustomer(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(CustomerDataStore.GetItemAsync(itemId));
			_customerService.Setup(s => s.DeleteItem(It.IsAny<Item>()));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(true);
			var notFound = await _itemController.DeleteItemAsync(It.IsAny<int>(), It.IsAny<int>());
			Assert.IsType<NotFoundResult>(notFound);
			
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public async Task DeleteItemAsync_SaveFails_InternalError()
		{
		
			const int itemId = 1;
			_customerService.Setup(s => s.CustomerExists(It.IsAny<int>())).ReturnsAsync(true);
			_customerService.Setup(s => s.GetItemForCustomer(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(CustomerDataStore.GetItemAsync(1));
			_customerService.Setup(s => s.DeleteItem(It.IsAny<Item>()));
			_customerService.Setup(s => s.SaveAsync()).ReturnsAsync(false);
			var internalServerError = await _itemController.DeleteItemAsync(It.IsAny<int>(), It.IsAny<int>());
			Assert.Equal(500, ((ObjectResult)internalServerError).StatusCode);
			Assert.Equal("A Problem Happen While handling your request", ((ObjectResult)internalServerError).Value);
			
		}
	}
}