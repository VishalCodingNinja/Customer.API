using AutoMapper;
using EMI.Customers.Domain.AllDbContext;
using EMI.Customers.Domain.Entities;
using EMI.Customers.Domain.Models;
using EMI.Customers.Domain.Repository;
using EMI.Customers.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace EMI.Customers.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = "EMI.Customer.API",
					Description = "OnlineCartApplication",
					TermsOfService = "None",
					Contact = new Contact() { Name = "EuroMonitorInternational", Email = "Vishal.Singh@Euromonitor.com", Url = "www.Euromonitorinternational.com" }
				});
			});
			var connectionSql = "Server=(localdb)\\MSSQLlocaldb;Database=ASPNETApplicationDB;Trusted_Connection=True;MultipleActiveResultSets=true";
			//services.AddDbContext<CustomerDbContext>(
			//	options => options.UseSqlServer(Configuration.GetConnectionString("EMI.Customer.API"),b=>b.MigrationsAssembly("EMI.Customers.Domain"))
			//);
			services.AddDbContext<CustomerDbContext>(
				options => options.UseSqlServer(connectionSql)
			);
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<ICustomerService, CustomerService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
			CustomerDbContext customerDbContext)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseStatusCodePages();
			app.UseHttpsRedirection();
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMI.Customer.API FirstAPI");
			});
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();
			AutoMapper.Mapper.Initialize(cfg =>
			{
				Mapper.Reset();
				cfg.CreateMap<Customer,CustomerWIthoutItems>();
				cfg.CreateMap<Customer, CustomerDto>();
				cfg.CreateMap<Item, ItemDto>();
				cfg.CreateMap<ItemsForCreationDto, Item>();
				cfg.CreateMap<ItemsForUpDateDto, Item>();
				cfg.CreateMap<Item, ItemsForUpDateDto>(); 
			});

		}
	}
}
