using GeneralAPI.Models;
using GeneralAPI.Models.PostgresSql;
using Microsoft.EntityFrameworkCore;

namespace GeneralAPI.Data
{
	public class RenderPlatformXContext : DbContext
	{
		public RenderPlatformXContext(DbContextOptions<RenderPlatformXContext> options)
				: base(options)
		{
		}

		public DbSet<Framework2> Framework { get; set; } = default!;
		public DbSet<Education2> Education { get; set; } = default!;
		public DbSet<WorkExperience2> WorkExperience { get; set; } = default!;
		public DbSet<ContentSource2> ContentSource { get; set; } = default!;
		public DbSet<Project2> Project { get; set; } = default!;
	}
}
