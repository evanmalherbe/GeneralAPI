using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralAPI.Models.PostgresSql
{
	[Table("contentsource")]
	public class ContentSource2
	{
		[Column("id")]
		public int ID { get; set; }
		[Column("contentname")]
		public string? ContentName { get; set; }
		[Column("contentbody")]
		public string? ContentBody { get; set; }
		[Column("isdisplay")]
		public bool IsDisplay { get; set; }
	}
}
