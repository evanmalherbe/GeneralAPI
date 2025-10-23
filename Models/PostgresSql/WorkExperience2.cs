using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("workexperience")]
	public class WorkExperience2
	{
		[Column("id")]
		public int ID { get; set; }
		public string? Position { get; set; }
		public string? Company { get; set; }
		[Column("datesofemployment")]
		public string? DatesOfEmployment { get; set; }
		public string? Description { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
	}
}
