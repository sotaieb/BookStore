using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using St.BookStore.Core.Entities;
using St.BookStore.Core.Exceptions;
using St.BookStore.Data;
using Xunit;

namespace St.BookStore.Business.Tests
{
    public class StoreServiceTests
    {
        private string _catalogAsString;
        private BookStoreContext _context;
        public StoreServiceTests()
        {
            this._context = new BookStoreContext();
            this._catalogAsString = File.ReadAllText("bookstore.json");
        }

        [Fact]
        public void Import_Loads_Catalog()
        {
            // Arrange
            var json = File.ReadAllText("bookstore.json");
            var context = new BookStoreContext();
            var service = new StoreService(context);

            // Act
            service.Import(json);

            // Assert
            Assert.NotNull(context.Catalog);
            Assert.True(context.Catalog.Count > 0);
        }

        [Fact]
        public void Import_Loads_Category()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act
            service.Import(this._catalogAsString);

            // Assert
            Assert.NotNull(this._context.Category);
            Assert.True(this._context.Category.Count > 0);
        }

        [Fact]
        public void Import_With_Null_Json_Throws_ArgumentNullException()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Import(null));
        }

        [Fact]
        public void Import_With_Empty_Json_Throws_ArgumentNullException()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act
            // Assert

            Assert.Throws<ArgumentNullException>(() => service.Import(""));
        }

        [Fact]
        public void Import_With_Invalid_Json_Throws_JsonReaderException()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<JsonReaderException>(() => service.Import("data"));
        }

        [Fact]
        public void Quantity_With_Null_Name_Throws_ArgumentNullException()
        {
            // Arrange
            this._context.Catalog = new List<Book> {
                new Book { Name = "book1", Quantity= 10}
            };
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Quantity(null));
        }

        [Fact]
        public void Quantity_With_Empty_Name_Throws_ArgumentNullException()
        {
            // Arrange
            this._context.Catalog = new List<Book> {
                new Book { Name = "book1", Quantity= 10}
            };
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Quantity(""));
        }

        [Fact]
        public void Quantity_With_Empty_Name_Throws_BookNotFoundException()
        {
            // Arrange
            this._context.Catalog = new List<Book> {
                new Book { Name = "book1", Quantity= 10}
            };
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<BookNotFoundException>(() => service.Quantity("moq"));
        }

        [Fact]
        public void Quantity_Returns_Quantity()
        {
            // Arrange
            this._context.Catalog = new List<Book> {
                new Book { Name = "book1", Quantity= 10}
            };
            var service = new StoreService(this._context);

            // Act
            var quantity = service.Quantity("book1");

            // Assert
            Assert.Equal(10, quantity);
        }

        [Fact]
        public void Import_Then_Quantity_Returns_Quantity()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act
            service.Import(this._catalogAsString);

            var quantity = service.Quantity("Ayn Rand - FountainHead");

            // Assert
            Assert.Equal(10, quantity);
        }

        [Fact]
        public void Buy_Returns_TotalAmount()
        {
            // Arrange
            
            this._context.Catalog = new List<Book> {
                new Book { Name = "J.K Rowling - Goblet Of fire", Category = "Fantastique", Price= 8, Quantity= 2},
                new Book { Name = "Isaac Asimov - Foundation", Category = "Science Fiction", Price= 16, Quantity= 1},
                
            };

            this._context.Category = new List<CategoryWithDiscount> { 
                new CategoryWithDiscount { Name = "Fantastique" , Discount = 0.1 },
                new CategoryWithDiscount { Name = "Science Fiction" , Discount = 0.05 }
            };

            var service = new StoreService(this._context);

            // Act
            var total = service.Buy("J.K Rowling - Goblet Of fire", "Isaac Asimov - Foundation");

            // Assert
            Assert.Equal(24.00, total);
        }

        [Fact]
        public void Buy_With_Discount_Returns_TotalAmount()
        {
            // Arrange
            
            this._context.Catalog = new List<Book> {
                new Book { Name = "J.K Rowling - Goblet Of fire", Category = "Fantastique", Price= 8, Quantity= 2},
                new Book { Name = "Isaac Asimov - Foundation", Category = "Science Fiction", Price= 16, Quantity= 1},
                
            };

            this._context.Category = new List<CategoryWithDiscount> { 
                new CategoryWithDiscount { Name = "Fantastique" , Discount = 0.1 },
                new CategoryWithDiscount { Name = "Science Fiction" , Discount = 0.05 }
            };

            var service = new StoreService(this._context);

            // Act
            var total = service.Buy("J.K Rowling - Goblet Of fire","J.K Rowling - Goblet Of fire", "Isaac Asimov - Foundation");

            // Assert

            // 8*0.9 + 8 + 16
            Assert.Equal(31.20, total);
        }

        [Fact]
        public void Import_Then_Buy_Returns_TotalAmount_Ex1()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act
            service.Import(this._catalogAsString);
            
            var total = service.Buy("J.K Rowling - Goblet Of fire",
            "Robin Hobb - Assassin Apprentice",
            "Robin Hobb - Assassin Apprentice");

            // Assert

            Assert.Equal(30.0, total);
        }

        [Fact]
        public void Import_Then_Buy_Returns_TotalAmount_Ex2()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act
            service.Import(this._catalogAsString);

            var total = service.Buy("Ayn Rand - FountainHead",
            "Isaac Asimov - Foundation",
            "Isaac Asimov - Robot series",
            "J.K Rowling - Goblet Of fire",
            "J.K Rowling - Goblet Of fire",
            "Robin Hobb - Assassin Apprentice",
            "Robin Hobb - Assassin Apprentice");

            // Assert

            Assert.Equal(69.95, total);
        }

        [Fact]
        public void Buy_Filters_BasketNames()
        {
            this._context.Catalog = new List<Book> {
                new Book { Name = "J.K Rowling - Goblet Of fire", Category = "Fantastique", Price= 8, Quantity= 2},
                new Book { Name = "Isaac Asimov - Foundation", Category = "Science Fiction", Price= 16, Quantity= 1},
                
            };

            this._context.Category = new List<CategoryWithDiscount> { 
                new CategoryWithDiscount { Name = "Fantastique" , Discount = 0.1 },
                new CategoryWithDiscount { Name = "Science Fiction" , Discount = 0.05 }
            };
            
            var service = new StoreService(this._context);

            // Act
            var total = service.Buy(null, "", "J.K Rowling - Goblet Of fire");

            // Assert
            Assert.Equal(8.00, total);
        }

    

         [Fact]
        public void Buy_Invalid_Product_Throws_BookNotFoundException()
        {
            // Arrange
            var service = new StoreService(this._context);

            // Act

            // Assert
            Assert.Throws<NotEnoughInventoryException>(() => service.Buy("csharp"));
        }
    }
}
