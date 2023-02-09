using Homework.Contexts;
using Homework.Models.DTOs;
using Homework.Services;
using HomeworkTest.Helpers;
using HomeworkTest.Seeding;
using Moq;
using System.Numerics;

namespace HomeworkTest.UnitTests
{
    public class ItemServiceUnitTests
    {
        private ItemService service;
        private ApplicationDbContext context;

        public ItemServiceUnitTests()
        {
            context = ContextProvider.CreateContextFromScratch();
            service = new ItemService(context);
            DBSeed.Seed(context);
        }

        [Theory]
        [InlineData(201, "username1", "name", "description", "https://www.google.cz/", 30, 10)]
        [InlineData(400, "username1", "", "description", "https://www.google.cz/", 30, 10)]
        [InlineData(400, "username1", "name", "", "https://www.google.cz/", 30, 10)]
        [InlineData(400, "username1", "name", "description", "link", 30, 10)]
        [InlineData(400, "username1", "name", "description", "https://www.google.cz/", 0, 10)]
        [InlineData(400, "username1", "name", "description", "https://www.google.cz/", 30, 0)]
        public async Task SellTest(int expected, string username, string name, string description, string photourl, double purchase, double starting)
        {
            //Arrange
            var DTO = new SellRequestDTO() { Username = username, SellDTO = new SellDTO() { Name = name, Description = description, PhotoURL = photourl, PurchasePrice = purchase, StartingPrice = starting } };

            //Act
            var result = service.Sell(DTO);

            //Assert
            Assert.Equal(expected, result.Status);
        }

        [Theory]
        [InlineData(200, 0)]
        [InlineData(400, -1)]

        public async Task ListTest(int expected, int n)
        {
            //Arrange

            //Act
            var result = service.List(n);

            //Assert
            Assert.Equal(expected, result.Status);
        }

        [Theory]
        [InlineData(200, 1)]
        [InlineData(400, -1)]

        public async Task ViewSpecificTest(int expected, int n)
        {
            //Arrange

            //Act
            var result = service.ViewSpecific(n);

            //Assert
            Assert.Equal(expected, result.Status);
        }

        [Theory]
        [InlineData(200, 1, 10)]
        [InlineData(400, -1, 10)]
        [InlineData(400, 1, 1)]
        [InlineData(400, 1, 200)]
        [InlineData(400, 2, 10)]
        [InlineData(400, 1, 3)]

        public async Task BidTest(int expected, int id, int bid)
        {
            //Arrange
            var dto = new BidRequestDTO() { Username = "username1", BidDTO = new BidDTO() { Id = id, Bid = bid } };

            //Act
            var result = service.Bid(dto);

            //Assert
            Assert.Equal(expected, result.Status);
        }
    }
}
