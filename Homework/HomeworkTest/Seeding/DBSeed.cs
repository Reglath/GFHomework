using Homework.Contexts;
using Homework.Models.Entities;

namespace HomeworkTest.Seeding
{
    public class DBSeed
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user1 = new User() { Username = "username1", Password = "password1", Money = 100 };
            var item1 = new Item() { Name = "item1", Description = "description1", Price= 2, PurchasePrice = 20, Url = "https://google.com", User = user1};
            var bid1 = new Bid() { Price = 5, Bidder = "username1", Item = item1 };

            var user2 = new User() { Username = "username2", Password = "password2", Money = 5 };
            var item2 = new Item() { Name = "item1", Description = "description1", Price = 2, PurchasePrice = 20, Url = "https://google.com", User = user2, Sold = true, Buyer = user2.Username };
            var bid2 = new Bid() { Price = 20, Bidder = "username1", Item = item2 };

            context.Users.AddRange(user1,user2);
            context.Items.AddRange(item1, item2);
            context.Bids.AddRange(bid1, bid2);

            context.SaveChanges();
        }
    }
}
