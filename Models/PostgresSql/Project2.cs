using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("project")]
	public class Project2
	{
		[Column("id")]
		public int ID { get; set; }
		public string? Name { get; set; }
		[Column("githublink")]
		public string? GithubLink { get; set; }
		[Column("livelink")]
		public string? LiveLink { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
		public int Order { get; set; }
		public string? Description { get; set; }
		public string? Technologies { get; set; }
		[Column("imagepath")]
		public string? ImagePath { get; set; }
	}
}
