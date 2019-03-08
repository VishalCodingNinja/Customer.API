using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMI.Customers.Domain.Models
{
	public class ItemsForUpDateDto
	{
		[Required(ErrorMessage = "You should provide name")]
		[StringLength(maximumLength:10,ErrorMessage = "please enter valid length")]
		public string Name { get; set; }

		[Required]
		public double Cost { get; set; }
	}
}
