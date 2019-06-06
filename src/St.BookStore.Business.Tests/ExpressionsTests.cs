using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using St.BookStore.Core.Entities;
using St.BookStore.Data;
using Xunit;
using Xunit.Abstractions;

namespace St.BookStore.Business.Tests
{
    public class ExpressionsTests
    {
        private ITestOutputHelper _output;
        public ExpressionsTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task DeserializeJsonFromString()
        {
            // Arrange
            var jsonStr = await File.ReadAllTextAsync("bookstore.json");

            // Act 

            var model = JsonConvert.DeserializeObject<BookStoreAdapter>(jsonStr);

            // Assert
            Assert.NotNull(model);

        }

        [Fact]
        public void GroupBy()
        {
            // Arrange

            var catalog = new List<Book> {
                new Book { Name = "J.K Rowling - Goblet Of fire", Category = "cat1", Price= 8, Quantity= 2},
                new Book { Name = "Isaac Asimov - Foundation", Category = "cat2", Price= 16, Quantity= 1},
                new Book { Name = "Sample Book", Category = "cat1", Price= 10, Quantity= 15}
            };
            // Act 

            var groups = catalog.GroupBy(x => x.Category);

            foreach (var item in groups)
            {
                this._output.WriteLine($"{item.Key} - { string.Join(',', item.Select(x => x.Name))}");
            }

            // Assert
        }

        [Fact]
        public void Join()
        {
            // Arrange

            var catalog = new List<Book> {
                new Book { Name = "J.K Rowling - Goblet Of fire", Category = "cat1", Price= 8, Quantity= 2},
                new Book { Name = "Isaac Asimov - Foundation", Category = "cat2", Price= 16, Quantity= 1},
                new Book { Name = "Sample Book", Category = "cat3", Price= 10, Quantity= 15}
            };

            var categories = new List<CategoryWithDiscount> {
                new CategoryWithDiscount { Name = "cat1" , Discount = 10.0 },
                new CategoryWithDiscount { Name = "cat2" , Discount = 20.0 }
            };

            // Act 

            var join = catalog.Join(categories, x => x.Category, y => y.Name, (x, y) => new
            {
                Book = x,
                Category = y
            });

            foreach (var item in join)
            {
                this._output.WriteLine($"{item.Book.Name} - { item.Category.Name} ({item.Category.Discount} %)");
            }

            // Assert
        }
    }
}
