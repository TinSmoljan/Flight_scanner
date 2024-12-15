using System.Reflection;

namespace FlightScanner.Common.Api.Models
{
	public class PagingResult<T> where T : class
	{
		public IEnumerable<T> Results { get; set; }
		public int Count { get; set; }
		public int Page { get; set; }
		public int PerPage { get; set; }
		public string Sort { get; set; }
		public string SortDirection { get; set; }

		public PagingResult(int count, int page, int perPage, string sort, string sortDirection, IEnumerable<T> results)
		{
			Count = count;
			Page = page;
			PerPage = perPage;
			Sort = sort;
			SortDirection = sortDirection;

			if (!string.IsNullOrWhiteSpace(sort))
			{
				results = ApplySorting(results, sort, sortDirection);
			}
			Results = results.Skip((page - 1) * perPage).Take(perPage).ToList();
		}

		private IEnumerable<T> ApplySorting(IEnumerable<T> results, string sort, string sortDirection)
		{
			var propertyInfo = typeof(T).GetProperty(sort);

			if (propertyInfo == null)
			{
				throw new ArgumentException($"Invalid sort property: {sort}");
			}

			if (string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase))
			{
				return results.OrderByDescending(x => propertyInfo.GetValue(x, null));
			}
			else
			{
				return results.OrderBy(x => propertyInfo.GetValue(x, null));
			}
		}
	}
}
