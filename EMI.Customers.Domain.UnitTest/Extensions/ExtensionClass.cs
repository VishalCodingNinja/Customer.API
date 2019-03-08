using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Remotion.Linq.Clauses;

namespace EMI.Customers.Domain.UnitTest.Extensions
{
	
		public static class MockDbSet
		{
			public static void SetSource<T>(this Mock<DbSet<T>> mockSet, IList<T> source) where T : class
			{
				var data = source.AsQueryable();
				mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
				mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
				mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
				mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			}
		}
	
}
