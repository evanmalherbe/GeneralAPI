namespace GeneralAPI.TransferObjects
{
	public class AboutResponseDTO
	{
		public string? AboutText { get; set; }
		public List<EducationResponseDTO> Education { get; set; }
		public List<WorkExperienceResponseDTO> Work { get; set; }
	}
}
