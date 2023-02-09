using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class ListViewDTO
    {
        public string Name { get; set; }
        public string PhotoURL { get; set; }
        public double CurrentBid { get; set; }

        public ListViewDTO(Item item)
        {
            Name = item.Name;
            PhotoURL = item.Url;
            if(item.Bids.Count> 0 )
                CurrentBid = item.Bids.Last().Price;
        }
    }
}
