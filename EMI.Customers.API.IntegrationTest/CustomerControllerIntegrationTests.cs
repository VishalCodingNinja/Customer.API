using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EMI.Customers.API.IntegrationTest
{
	public class CustomerControllerIntegrationTests
		: IClassFixture<WebApplicationFactory<EMI.Customers.API.Startup>>
	{
		private readonly WebApplicationFactory<EMI.Customers.API.Startup> _factory;

		public CustomerControllerIntegrationTests(WebApplicationFactory<EMI.Customers.API.Startup> factory)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData("/api/customers")]
		
		public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync(url);

			// Assert
			response.EnsureSuccessStatusCode(); // Status Code 200-299
			Assert.Equal(200,(int)response.StatusCode);
		}
	}
}