using GeneralAPI.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneralAPI.Services
{
	public class SecurityLoggingService : ISecurityLoggingService
	{
		private readonly ILogger<SecurityLoggingService> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SecurityLoggingService(ILogger<SecurityLoggingService> logger, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
		}
		public void LogFailure(string emailAddress, string errors)
		{
			string logFailureInformationEventString = "SecurityEvent: (Contact form) Bad Request for user with email {Email} on path {Path} from IP {IPAddress}. Errors: {Errors}.";
			_logger.LogInformation(logFailureInformationEventString,
				emailAddress,
			 _httpContextAccessor.HttpContext?.Request.Path,
			_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
			errors);
		}

		public void LogSuccess(string emailAddress)
		{
			string logSuccessInformationEventString = "SecurityEvent: (Contact form) SUCCESSFUL email sent for visitor with email {Email} on path {Path} from IP {IPAddress}.";
			_logger.LogInformation(logSuccessInformationEventString,
				emailAddress,
			 _httpContextAccessor.HttpContext?.Request.Path,
			_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString());
		}

		public void LogWarning(string emailAddress, string errors, int type)
		{
			string logWarningEventString = type == 1
					? "SecurityEvent: (Contact form) Rate limit exceeded by user with email {Email} on path {Path} from IP {IPAddress}. Errors: {Error}"
					: "SecurityEvent: (Contact form) Suspicious activity detected (Honeypot field filled) by user with email {Email} on path {Path} from IP {IPAddress}. Errors: {Error}";

			_logger.LogWarning(logWarningEventString,
					emailAddress,
					_httpContextAccessor.HttpContext?.Request.Path,
					_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
					errors);
		}
	}
}
