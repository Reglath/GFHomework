namespace Homework.Models.DTOs
{
    public class ListResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ListViewDTO> Items { get; set; }

        public ListResponseDTO()
        {
            Items = new List<ListViewDTO>();
        }
    }
}
