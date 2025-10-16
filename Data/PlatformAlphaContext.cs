using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GeneralAPI.Models;

namespace GeneralAPI.Data
{
	public class PlatformAlphaContext : DbContext
	{
		public PlatformAlphaContext(DbContextOptions<PlatformAlphaContext> options)
				: base(options)
		{
		}

		public DbSet<Framework> Frameworks { get; set; } = default!;
		public DbSet<Education> Educations { get; set; } = default!;
		public DbSet<WorkExperience> WorkExperiences { get; set; } = default!;
		public DbSet<ContentSource> ContentSources { get; set; } = default!;
		public DbSet<Project> Projects { get; set; } = default!;
	}
}
