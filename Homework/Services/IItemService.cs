using Homework.Models.DTOs;

namespace Homework.Services
{
    public interface IItemService
    {
        BidResponseDTO Bid(BidRequestDTO bidRequestDTO);
        ListResponseDTO List(int n);
        SellResponseDTO Sell(SellRequestDTO sellRequestDTO);
        ViewResponseDTO ViewSpecific(int id);
    }
}