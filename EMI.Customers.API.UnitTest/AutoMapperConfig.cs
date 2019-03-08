using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Models;

namespace EMI.Customers.API.UnitTest
{
	public static class AutoMapperConfig
	{
		private static readonly object MapperLock = new object();
		public static void Initialize()
		{
			lock (MapperLock)
			{
				Mapper.Reset();
				AutoMapper.Mapper.Initialize(cfg =>
				{
					
					cfg.CreateMap<Customer, CustomerWIthoutItems>();
					cfg.CreateMap<Customer, CustomerDto>();
					cfg.CreateMap<Item, ItemDto>();
					cfg.CreateMap<ItemsForCreationDto, Item>();
					cfg.CreateMap<ItemsForUpDateDto, Item>();
					cfg.CreateMap<Item, ItemsForUpDateDto>();
				});
			}
		}
	}
}
