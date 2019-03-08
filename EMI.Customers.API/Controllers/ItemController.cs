using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Models;
using EMI.Customers.Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EMI.Customers.API.Controllers
{
	[Route("api/customers")]
	public class ItemController : Controller
	{
		private readonly ICustomerService _customerService;

		public ItemController(ICustomerService customerService)
		{
			_customerService = customerService;
		}
		[HttpGet("{customerId}/items")]
		public async Task<IActionResult> GetItemsAsync(int customerId)
		{
			var isExist = await _customerService.CustomerExists(customerId);
			if(!isExist) return NotFound();
			var itemsForCustomers = await _customerService.GetItemsForCustomer(customerId);
			var itemsForCityResult = Mapper.Map<IEnumerable<ItemDto>>(itemsForCustomers);
			return Ok(itemsForCityResult);
		}	

		[HttpGet("{customerId}/item/{itemId}",Name = "GetItemAsync")]
		public async Task<IActionResult> GetItemAsync(int customerId,int itemId)
		{
			var isExist = await _customerService.CustomerExists(customerId);
			if (!isExist) return NotFound();
			var item = await _customerService.GetItemForCustomer(customerId, itemId);
			if (item == null) return NotFound();
			var itemResult = Mapper.Map<ItemDto>(item);
			return Ok(itemResult);
		}

		[HttpPost("{customerId}/items")]
		public async Task<IActionResult> AddItemAsync(int customerId,[FromBody] ItemsForCreationDto item)
		{
			var isExist = await _customerService.CustomerExists(customerId);
			if (!isExist) return NotFound();
			var finalItem = Mapper.Map<Item>(item);
			_customerService.AddItemForCustomer(customerId,finalItem);
			var isSaved = await _customerService.SaveAsync();
			if (!isSaved) return StatusCode(500, "A Problem Happen While handling our request");
			var createdItemToReturn = Mapper.Map<ItemDto>(finalItem);
			return CreatedAtRoute("GetItemAsync",new {customerId, itemId = createdItemToReturn.Id}, createdItemToReturn);
		}

		[HttpPut("{customerId}/items/{itemId}")]
		public async Task<IActionResult> UpdateItemAsync(int customerId,int itemId,[FromBody] ItemsForUpDateDto itemToUpdate)
		{
			if (itemToUpdate == null||customerId==0||itemId==0) return BadRequest();
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var isExist = await _customerService.CustomerExists(customerId);
			if (!isExist) return NotFound();
			var itemFromDatabase = await _customerService.GetItemForCustomer(customerId, itemId);
			if (itemFromDatabase == null) return NotFound();
			Mapper.Map(itemToUpdate, itemFromDatabase);//after this line if we call save changes will effectively save to the database
			var isSaved = await _customerService.SaveAsync();
			if (!isSaved) return StatusCode(500, "A Problem Happen While handling our request");
			return NoContent();
		}

		[HttpPatch("{customerId}/items/{itemId}")]
		public async Task<IActionResult> PartiallyUpdateItemsDetailsAsync(int customerId, int itemId, [FromBody] JsonPatchDocument<ItemsForUpDateDto> patchItem)
		{
			if(patchItem==null) return BadRequest();
			var isExist = await _customerService.CustomerExists(customerId);
			if (!isExist) return NotFound();
			var itemEntity = await _customerService.GetItemForCustomer(customerId, itemId);
			if (itemEntity == null) return NotFound();
			var itemToPatch = Mapper.Map<ItemsForUpDateDto>(itemEntity);
			patchItem.ApplyTo(itemToPatch,ModelState);
			if (!ModelState.IsValid) return BadRequest();
			//TryValidateModel(itemToPatch);
			Mapper.Map(itemToPatch, itemEntity);
			var isSaved = await _customerService.SaveAsync();
			if (!isSaved) return StatusCode(500, "A Problem Happen While handling hour request");
			return NoContent();
		}

		[HttpDelete("{customerId}/items/{itemId}")]
		public async Task<IActionResult> DeleteItemAsync(int customerId, int itemId)
		{
			var isExist = await _customerService.CustomerExists(customerId);
			if (!isExist) return NotFound();
			var itemEntity = await _customerService.GetItemForCustomer(customerId, itemId);
			if (itemEntity == null) return NotFound();
			_customerService.DeleteItem(itemEntity);
			var isSaved = await _customerService.SaveAsync();
			if (!isSaved) return StatusCode(500, "A Problem Happen While handling your request");
			return NoContent();
		}
	}
}