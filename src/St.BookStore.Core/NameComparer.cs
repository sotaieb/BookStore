
using System;
using System.Collections.Generic;
using St.BookStore.Core.Entities;

namespace St.BookStore.Core
{
    public class NameComparer : IEqualityComparer<Book>
    {
        public bool Equals(Book x, Book y) => string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(Book obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}