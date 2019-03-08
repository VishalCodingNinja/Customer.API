using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMI.Customers.Domain.Models
{
	public class ItemsForCreationDto
	{
		[Required(ErrorMessage = "You should provide name")]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		public double Cost { get; set; }
	}
}
