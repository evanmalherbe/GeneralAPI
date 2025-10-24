using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("education")]
	public class Education2
	{
		[Column("id")]
		public int ID { get; set; }
		[Column("institution")]
		public string? Institution { get; set; }
		[Column("degreecourse")]
		public string? DegreeCourse { get; set; }
		[Column("yearcomplete")]
		public string? YearComplete { get; set; }
		[Column("link")]
		public string? Link { get; set; }
		[Column("linktext")]
		public string? LinkText { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
	}
}
