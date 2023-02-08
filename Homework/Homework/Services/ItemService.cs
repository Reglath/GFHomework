using Homework.Contexts;
using Homework.Models.DTOs;
using Homework.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework.Services
{
    public class ItemService
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
            if (sellRequestDTO.SellDTO.Name == null)
                return new SellResponseDTO() { Status = 400, Message = "item name required" };
            if (sellRequestDTO.SellDTO.Description == null)
                return new SellResponseDTO() { Status = 400, Message = "description required" };
            if (sellRequestDTO.SellDTO.StartingPrice == null || sellRequestDTO.SellDTO.StartingPrice <=0)
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

            var all = context.Items.Where(i => i.Sold.Equals(false)).ToList();
            var items = new List<Item>();
            for(int i = n*20; i < 20+(n*20); i++)
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
            if(!context.Items.Any(i => i.Id == id))
                return new ViewResponseDTO() { Status = 400, Message = "invalid item id" };

            var result = new ViewViewDTO(context.Items.Include(i => i.User).FirstOrDefault(i => i.Id == id));

            return new ViewResponseDTO() { Status = 200, Message = "item found", View = result };
        }

        private Item AddNewItem(SellRequestDTO sellRequestDTO)
        {
            var item = new Item()
            {
                Name = sellRequestDTO.SellDTO.Name,
                Description = sellRequestDTO.SellDTO.Description,
                Price = sellRequestDTO.SellDTO.StartingPrice,
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
