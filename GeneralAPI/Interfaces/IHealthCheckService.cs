namespace GeneralAPI.Interfaces
{
	public interface IHealthCheckService
	{
		Task<bool> HealthCheckPing(); 
	}
}
