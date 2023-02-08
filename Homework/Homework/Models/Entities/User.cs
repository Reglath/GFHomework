namespace Homework.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Money { get; set; }
        public List<Item> Items { get; set; }
        public List<Bid> Bids { get; set; }

        public User()
        {
            Items = new List<Item>();
            Bids = new List<Bid>();
        }
    }
}
