using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EMI.Customers.Domain.Models;
using EMI.Customers.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMI.Customers.API.Controllers
{
	[Route("api/customers")]
	public class CustomerController : Controller
	{
		private readonly ICustomerService _customerService;

		// GET: /<controller>/
		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}
		[HttpGet]
		public async Task<IActionResult> GetCustomersAsync()
		{
			var customerEntities = await _customerService.GetCustomers();
			if (customerEntities == null) return NotFound();
			var results = Mapper.Map<IEnumerable<CustomerWIthoutItems>>(customerEntities);
			return Ok(results);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCustomerAsync(int id, bool includeItem = false)
		{
			var customer = await _customerService.GetCustomer(id, includeItem);
			if (customer == null) return NotFound();
			if (includeItem)
			{
				var customerResult = Mapper.Map<CustomerDto>(customer);
				return Ok(customerResult);
			}

			var customerWithoutItem = Mapper.Map<CustomerWIthoutItems>(customer);
			return Ok(customerWithoutItem); 
		}
	}
}
