using System;

namespace St.BookStore.Core.Entities
{
    /// <summary>
    /// A book in the catalog
    /// </summary>
    public class Book : INameQuantity
    {
        /// <summary>
        /// The unique Name of the book, it is a functionnal key
        /// Required
        /// </summary>
        
        public string Name { get; set; }

        /// <summary>
        /// The name of one the <see cref="Category" /> existing in the Category root properties.
        /// <remarks>Required</remarks>
        /// <example>J.K Rowling - Goblet Of fire</example> 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The price of copy of the book
        /// Required
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The Quantity of copy of the book in the catalog
        /// Required
        /// </summary>
        public int Quantity { get; set; }
    }

}