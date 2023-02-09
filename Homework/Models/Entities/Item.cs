namespace Homework.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
        public double PurchasePrice { get; set; }
        public bool Sold { get; set; }
        public User User { get; set; }
        public string? Buyer { get; set; }
        public List<Bid> Bids { get; set; }

        public Item()
        {
            Bids = new List<Bid>();
        }
    }
}
