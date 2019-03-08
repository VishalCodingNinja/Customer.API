using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EMI.Customers.Domain.Enums;
using EMI.Customers.Domain.Models;

namespace EMI.Customers.Domain.Entities
{
	public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]//it will generated on every insert
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MaxLength(500)]
		public string Address { get; set; }
		[Required]
		public CustomerType CustomerType { get; set; }
		public ICollection<Item> ItemPurchase { get; set; } = new List<Item>();
	}
}
