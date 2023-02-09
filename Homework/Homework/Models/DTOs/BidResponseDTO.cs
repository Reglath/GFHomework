namespace Homework.Models.DTOs
{
    public class BidResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public BidViewDTO? Item { get; set; }
    }
}
