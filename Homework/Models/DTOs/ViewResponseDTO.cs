namespace Homework.Models.DTOs
{
    public class ViewResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public ViewViewDTO? View { get; set; }
    }
}
