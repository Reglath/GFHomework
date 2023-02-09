using Homework.Contexts;
using Homework.Models.DTOs;
using Homework.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework.Services
{
    public class ItemService : IItemService
    {
        private ApplicationDbContext context;

        public ItemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public SellResponseDTO Sell(SellRequestDTO sellRequestDTO)
        {
            if (sellRequestDTO == null)
                return new SellResponseDTO() { Status = 400, Message = "invalid input" };
            if (sellRequestDTO.SellDTO.Name == null || sellRequestDTO.SellDTO.Name.Length == 0)
                return new SellResponseDTO() { Status = 400, Message = "item name required" };
            if (sellRequestDTO.SellDTO.Description == null || sellRequestDTO.SellDTO.Description.Length == 0)
                return new SellResponseDTO() { Status = 400, Message = "description required" };
            if (sellRequestDTO.SellDTO.StartingPrice == null || sellRequestDTO.SellDTO.StartingPrice <= 0)
                return new SellResponseDTO() { Status = 400, Message = "starting price higher than 0 required" };
            if (sellRequestDTO.SellDTO.PurchasePrice == null || sellRequestDTO.SellDTO.PurchasePrice <= 0)
                return new SellResponseDTO() { Status = 400, Message = "purchase price higher than 0 required" };
            if (!Uri.IsWellFormedUriString(sellRequestDTO.SellDTO.PhotoURL, UriKind.Absolute))
                return new SellResponseDTO() { Status = 400, Message = "not a valid photo URL" };

            return new SellResponseDTO() { Status = 201, Message = "auction created", Item = AddNewItem(sellRequestDTO) };
        }

        public ListResponseDTO List(int n)
        {
            if (n < 0)
                return new ListResponseDTO() { Status = 400, Message = "invalid page number" };

            var all = context.Items.Include(i => i.Bids).Where(i => i.Sold.Equals(false)).ToList();
            var items = new List<Item>();
            for (int i = n * 20; i < 20 + (n * 20); i++)
            {
                if (i < all.Count)
                    items.Add(all[i]);
                else break;
            }

            var result = new List<ListViewDTO>();
            foreach (var item in items)
                result.Add(new ListViewDTO(item));

            return new ListResponseDTO() { Status = 200, Message = "items found", Items = result };
        }

        public ViewResponseDTO ViewSpecific(int id)
        {
            if (id < 0)
                return new ViewResponseDTO() { Status = 400, Message = "invalid item id" };
            if (!context.Items.Any(i => i.Id == id))
                return new ViewResponseDTO() { Status = 400, Message = "invalid item id" };

            var result = new ViewViewDTO(context.Items.Include(i => i.Bids).Include(i => i.User).FirstOrDefault(i => i.Id == id));

            return new ViewResponseDTO() { Status = 200, Message = "item found", View = result };
        }

        public BidResponseDTO Bid(BidRequestDTO bidRequestDTO)
        {
            if (bidRequestDTO.BidDTO.Id < 0)
                return new BidResponseDTO() { Status = 400, Message = "invalid item id" };
            if (!context.Items.Any(i => i.Id == bidRequestDTO.BidDTO.Id))
                return new BidResponseDTO() { Status = 400, Message = "invalid item id" };

            var item = context.Items.Include(i => i.Bids).FirstOrDefault(i => i.Id == bidRequestDTO.BidDTO.Id);
            var user = context.Users.FirstOrDefault(u => u.Username.Equals(bidRequestDTO.Username));

            if (bidRequestDTO.BidDTO.Bid > user.Money)
                return new BidResponseDTO() { Status = 400, Message = "not enough money for this bid" };
            if (item.Sold)
                return new BidResponseDTO() { Status = 400, Message = "item cannot be bought" };
            if (bidRequestDTO.BidDTO.Bid > user.Money)
                return new BidResponseDTO() { Status = 400, Message = "not enough money for this bid" };
            if (item.Bids.Count == 0 && item.Price > bidRequestDTO.BidDTO.Bid)
                return new BidResponseDTO() { Status = 400, Message = "starting price is higher than your bid" };
            if (item.Bids.Count > 0)
                if (item.Bids.Last().Price >= bidRequestDTO.BidDTO.Bid)
                    return new BidResponseDTO() { Status = 400, Message = "higher bid already present" };
            if (item.PurchasePrice <= bidRequestDTO.BidDTO.Bid)
            {
                item.Buyer = user.Username;
                item.Sold = true;
                context.SaveChanges();
                return new BidResponseDTO() { Status = 200, Message = "item bought", Item = new BidViewDTO(item) };
            }

            else
            {
                item.Bids.Add(new Bid() { Bidder = user.Username, Price = bidRequestDTO.BidDTO.Bid, Item = item });
                context.SaveChanges();
                return new BidResponseDTO() { Status = 200, Message = "bid accepted", Item = new BidViewDTO(item) };
            }

        }

        private Item AddNewItem(SellRequestDTO sellRequestDTO)
        {
            var item = new Item()
            {
                Name = sellRequestDTO.SellDTO.Name,
                Description = sellRequestDTO.SellDTO.Description,
                Price = sellRequestDTO.SellDTO.StartingPrice,
                PurchasePrice = sellRequestDTO.SellDTO.PurchasePrice,
                Url = sellRequestDTO.SellDTO.PhotoURL,
                Sold = false,
                User = context.Users.FirstOrDefault(u => u.Username.Equals(sellRequestDTO.Username))
            };
            context.Items.Add(item);
            context.SaveChanges();
            return item;
        }
    }
}
