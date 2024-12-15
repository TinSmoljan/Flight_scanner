namespace FlightScanner.Services.Api.Services
{
	public interface ICacheService
	{
		Task<bool> SetItem(string key, object value, TimeSpan? slidingExpirtion = null, DateTimeOffset? absoluteExpiration = null);
		Task<T> GetItem<T>(string key);
		Task<bool> RemoveItem(string key);
	}
}
