using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class BidViewDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string? Buyer { get; set; }

        public BidViewDTO(Item item)
        {
            Name = item.Name;
            Description = item.Description;
            Url = item.Url;
            if (item.Buyer != null)
                Buyer = item.Buyer;
        }
    }
}
