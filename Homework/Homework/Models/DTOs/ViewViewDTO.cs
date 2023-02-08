using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class ViewViewDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string? Buyer { get; set; }
        public List<ViewBidDTO> Bids { get; set; }

        public ViewViewDTO(Item item)
        {
            Name = item.Name;
            Description = item.Description;
            Url = item.Url;
            if(item.Buyer != null)
                Buyer = item.Buyer;
            Bids = new List<ViewBidDTO>();
            foreach(var bid in item.Bids)
            {
                Bids.Add(new ViewBidDTO(bid));
            }
        }
    }
}
