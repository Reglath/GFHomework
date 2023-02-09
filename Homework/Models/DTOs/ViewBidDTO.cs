using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class ViewBidDTO
    {
        public string Bidder { get; set; }
        public double Bid { get; set; }

        public ViewBidDTO(Bid bid)
        {
            Bidder = bid.Bidder;
            Bid = bid.Price;
        }
    }
}
