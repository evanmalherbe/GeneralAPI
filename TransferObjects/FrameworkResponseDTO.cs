namespace GeneralAPI.TransferObjects
{
	public class FrameworkResponseDTO
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int? Order { get; set; } = 0;
		public string IconClassPath { get; set; } = string.Empty;
		public string IconType { get; set; } = string.Empty;
		public string BackgroundColour { get; set; } = string.Empty;
	}
}
