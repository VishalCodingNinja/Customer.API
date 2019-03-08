using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EMI.Customers.Domain.Models;
using Newtonsoft.Json;
using Xunit;

namespace EMI.Customers.API.IntegrationTesting
{
	public class IntegrationTest
	{
		[Fact]
		public async Task GetCustomersAsync_void_ReturnCustomers()
		{
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.GetAsync("https://localhost:5001/api/customers");

				response.EnsureSuccessStatusCode();

				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}
		}

		[Fact]
		public async Task GetCustomerAsync_customerIdAndBool_ReturnCustomer()
		{
			const int customerId = 1;
			const bool includeItem = false;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.GetAsync($"https://localhost:5001/api/customers?id={customerId}&includeItem={includeItem}");
				response.EnsureSuccessStatusCode();
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}
		}

		[Fact]
		public async Task GetItemsAsync_void_ReturnItems()
		{
			const int customerId = 1;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.GetAsync($"https://localhost:5001/api/customers/?customerId={customerId}/items");

				response.EnsureSuccessStatusCode();

				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}
		}

		[Fact]
		public async Task GetItemAsync_customerIdAndItemId_ReturnItem()
		{
			const int customerId = 1;
			const int itemId = 1;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.GetAsync($"https://localhost:5001/api/customers/?customerId={customerId}/item/?itemId={itemId}");

				response.EnsureSuccessStatusCode();

				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			}
		}

		[Fact]
		public async Task AddItemAsync_customerIdAndItem_RoutingUrl()
		{
			const int customerId = 1;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.PostAsync($"/api/customers/{customerId}/items"
					 ,new StringContent(
						JsonConvert.SerializeObject(new ItemsForCreationDto()
						{
							Name = "Wallet",
							Cost = 1000
						}),
						Encoding.UTF8,
						"application/json"));
				response.EnsureSuccessStatusCode();
				//var res = await client.GetAsync(response.Headers.Location.ToString());
				Assert.StartsWith("http",response.Headers.Location.AbsoluteUri);
			}
			
		}
		
		[Fact]
		public async Task UpdateItemAsync_customerIdAndItemIdAndItem_NoContent()
		{
			const int customerId = 1;
			const int itemId = 1;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.PutAsync($"/api/customers/{customerId}/items/{itemId}"
					, new StringContent(
						JsonConvert.SerializeObject(new ItemsForUpDateDto()
						{
							Name = "Specs",
							Cost = 1000
						}),
						Encoding.UTF8,
						"application/json"));
				response.EnsureSuccessStatusCode();
				//var res = await client.GetAsync(response.Headers.Location.ToString());
				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

		}

		[Fact]
		public async Task DeleteItemAsync_customerIdAndItemIdAndItem_NoContent()
		{
			const int customerId = 1;
			const int itemId = 1;
			using (var client = new TestClientProvider().Client)
			{
				var response = await client.DeleteAsync($"/api/customers/{customerId}/items/{itemId}");

				response.EnsureSuccessStatusCode();

				Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
			}

		}

	}
}
