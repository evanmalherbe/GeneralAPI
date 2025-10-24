using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GeneralAPI.Models;

namespace GeneralAPI.Data
{
	public class PlatformXContext : DbContext
	{
		public PlatformXContext(DbContextOptions<PlatformXContext> options)
				: base(options)
		{
		}

		public DbSet<Framework> Framework { get; set; } = default!;
		public DbSet<Education> Education { get; set; } = default!;
		public DbSet<WorkExperience> WorkExperience { get; set; } = default!;
		public DbSet<ContentSource> ContentSource { get; set; } = default!;
		public DbSet<Project> Project { get; set; } = default!;
	}
}
