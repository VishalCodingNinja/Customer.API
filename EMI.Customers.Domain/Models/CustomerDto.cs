using System.Collections.Generic;
using EMI.Customers.Domain.Enums;

namespace EMI.Customers.Domain.Models
{
	public class CustomerDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public CustomerType CustomerType { get; set; }
		public ICollection<ItemDto> ItemPurchase { get; set; }=new List<ItemDto>();
	}
}
