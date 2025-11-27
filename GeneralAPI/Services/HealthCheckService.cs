using GeneralAPI.Data;
using GeneralAPI.Interfaces;
using GeneralAPI.Models.PostgresSql;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeneralAPI.Services
{
	public class HealthCheckService : IHealthCheckService
	{
		private readonly RenderPlatformXContext _context;
		private readonly ILogger<HealthCheckService> _logger;

		public HealthCheckService(RenderPlatformXContext context, ILogger<HealthCheckService> logger)
		{
			_context = context;
			_logger = logger;
		}
		public async Task<bool> HealthCheckPing()
		{
			try
			{
				await _context.Framework.AnyAsync(i => i.ID == 1);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Database ping failed.");
				return false;
			}
		}
	}
}
