using System;
using System.Collections.Generic;
using St.BookStore.Core.Entities;

namespace St.BookStore.Data
{
    /// <summary>
    /// The book store context
    /// </summary>
    public class BookStoreContext
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public BookStoreContext()
        {
            this.Catalog = new List<Book>();
            this.Category = new List<CategoryWithDiscount>();
        }

        /// <summary>
        /// List of existing category with associated discount
        /// </summary>

        public List<CategoryWithDiscount> Category { get; set; }

        /// <summary>
        /// The Catalog of the store
        /// </summary>
        public List<Book> Catalog { get; set; }
    }
}
