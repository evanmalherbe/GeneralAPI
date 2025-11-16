using GeneralAPI.Interfaces;
using System.Collections.Concurrent;

namespace GeneralAPI.Services
{
	public class RateLimitingService : IRateLimitingService
	{
		private static readonly ConcurrentDictionary<string, DateTime> _submissionRecords = new();
		private readonly TimeSpan _cooldown = TimeSpan.FromSeconds(60);

		public bool IsRateLimitExceeded(string ipAddress)
		{
			DateTime now = DateTime.UtcNow;
			if (_submissionRecords.TryGetValue(ipAddress, out var lastSubmissionTime))
			{
				// Check if the difference is less than the cooldown period
				if (now - lastSubmissionTime < _cooldown)
				{
					return true;
				}

				// Update the time for a request that passed the limit
				_submissionRecords[ipAddress] = now;
				return false;
			}
			else
			{
				// First time submitter, add them to the records
				_submissionRecords.TryAdd(ipAddress, now);
				return false;
			}
		}
	}
}
