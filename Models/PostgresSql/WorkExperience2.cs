using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("workexperience")]
	public class WorkExperience2
	{
		[Column("id")]
		public int ID { get; set; }
		[Column("position")]
		public string? Position { get; set; }
		[Column("company")]
		public string? Company { get; set; }
		[Column("datesofemployment")]
		public string? DatesOfEmployment { get; set; }
		[Column("description")]
		public string? Description { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
	}
}
