using System;
using St.BookStore.Core.Entities;
using Newtonsoft.Json;
using System.Linq;
using St.BookStore.Core.Exceptions;
using St.BookStore.Data;
using System.Collections.Generic;
using St.BookStore.Core;

namespace St.BookStore.Business
{
    /// <summary>
    /// The store service
    /// </summary>
    public class StoreService : IStore
    {
        /// <summary>
        /// The data context
        /// </summary>
        private BookStoreContext _context;

        /// <summary>
        /// The constructor
        /// </summary>
        public StoreService(BookStoreContext context)
        {
            this._context = context ?? throw new ArgumentNullException("context");
        }

        public void Import(string catalogAsJson)
        {
            if (string.IsNullOrEmpty(catalogAsJson))
            {
                throw new ArgumentNullException("catalogAsJson");
            }

            var adapter = JsonConvert.DeserializeObject<BookStoreAdapter>(catalogAsJson);

            this._context.Catalog = adapter.Catalog;
            this._context.Category = adapter.Category;
        }

        public int Quantity(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var book = this._context.Catalog.FirstOrDefault(x => x.Name == name);

            if (book == null)
            {
                throw new BookNotFoundException();
            }

            return book.Quantity;
        }

        public double Buy(params string[] basketByNames)
        {
            // filter basket array
            var filteredNames = basketByNames.Where(x => !string.IsNullOrEmpty(x));

            if (filteredNames.Count() == 0)
            {
                throw new ArgumentException("Basket is empty.");
            }

            // Oragnize basket by product
            var basket = filteredNames.GroupBy(x => x)
                                       .Select(x => new Book
                                       {
                                           Name = x.Key,
                                           Quantity = x.Count()
                                       });

            // Check stock
            var existingItems = basket.Join(this._context.Catalog, x =>
                                             x.Name, y => y.Name, (x, y) => new
                                             {
                                                 AskedQuantity = x.Quantity,
                                                 Book = y
                                             })
                                        .Where(x => x.Book.Quantity >= x.AskedQuantity);

            // validate basket
            var notEnoughBooks = basket.Except(existingItems.Select(x => x.Book),
                                            new NameComparer());

            if (notEnoughBooks.Count() > 0)
            {
                throw new NotEnoughInventoryException(notEnoughBooks);
            }

            // Group items by category
            var categories = existingItems.Join(this._context.Category, x => x.Book.Category,
                                            y => y.Name, (x, y) => new
                                            {
                                                Book = x.Book,
                                                AskedQuantity = x.AskedQuantity,
                                                Category = y
                                            })
                                            .GroupBy(x => x.Book.Category)
                                            .ToList();

            var total = 0.0;
            foreach (var category in categories)
            {
                // Get items count by category
                var size = category.Sum(x => x.AskedQuantity);

                foreach (var item in category)
                {
                    // Compute items price without discount
                    total += (item.Book.Price * item.AskedQuantity);

                    // Apply the discount to first element
                    if (size > 1)
                    {
                        total -= (item.Book.Price * item.Category.Discount);
                    }

                    // Update stock
                    item.Book.Quantity -= item.AskedQuantity;
                }
            }

            return total;
        }
    }
}
