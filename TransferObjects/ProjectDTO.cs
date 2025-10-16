namespace GeneralAPI.TransferObjects
{
	public class ProjectDTO
	{
		public int ID { get; set; }
		public string? Name { get; set; } = string.Empty;
		public string? GithubLink { get; set; } = string.Empty;
		public string? LiveLink { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty;
		public string? Technologies { get; set; } = string.Empty;
		public string? ImagePath { get; set; } = string.Empty;
	}
}
