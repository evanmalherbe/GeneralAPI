using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("framework")]
	public class Framework2
	{
		[Column("id")]
		public int ID { get; set; }
		[Column("name")]
		public string? Name { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
		[Column("order")]
		public int? Order { get; set; } = 0;
		[Column("iconclasspath")]
		public string? IconClassPath { get; set; }
		[Column("icontype")]
		public string? IconType { get; set; }
		[Column("backgroundcolour")]
		public string? BackgroundColour { get; set; }
	}
}
