namespace GeneralAPI.Interfaces
{
	public interface ISecurityLoggingService
	{
		void LogSuccess(string emailAddress);
		void LogFailure(string emailAddress, string errors);
		void LogWarning(string emailAddress, string errors, int type);
	}
}
