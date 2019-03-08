using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMI.Customers.Domain.Entities
{
	public class Item
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[Required]
		public double Cost { get; set; }
		[ForeignKey("CustomerId")]
		public Customer Customer { get; set; }
		public int CustomerId { get; set; }
	}
}
