namespace Homework.Models.Entities
{
    public class Bid
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Bidder { get; set; }
        public Item Item { get; set; }
    }
}
