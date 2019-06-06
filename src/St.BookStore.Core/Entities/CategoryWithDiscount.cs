using System;

namespace St.BookStore.Core.Entities
{
    /// <summary>
    /// A category with its discount
    /// </summary>
    public class CategoryWithDiscount
    {
        /// <summary>
        /// The unique name of the category, it is a functionnal key
        /// <remarks>Required, pattern": "^(.+)$</remarks>
        /// <example>Fantastique</example>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The discount applies when buying multiple book of this category
        /// Required
        /// </summary>
        public double Discount { get; set; }
    }

}