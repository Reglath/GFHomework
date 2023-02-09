using Homework.Models.Entities;

namespace Homework.Models.DTOs
{
    public class SellViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double StartingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public bool Sold { get; set; }

        public SellViewDTO(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Url = item.Url;
            StartingPrice = item.Price;
            PurchasePrice = item.PurchasePrice;
            bool Sold = item.Sold;
        }
    }
}
