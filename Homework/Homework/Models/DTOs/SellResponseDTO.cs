using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class SellResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Item? Item { get; set; }
    }
}
