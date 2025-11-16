namespace GeneralAPI.Interfaces
{
	public interface IRateLimitingService
	{
		bool IsRateLimitExceeded(string ipAddress);
	}
}
