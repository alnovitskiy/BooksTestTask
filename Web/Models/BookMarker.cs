using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BookMarker
    {
        private DbBooksContext dbContext = null;

        public BookMarker()
        {
            dbContext = new DbBooksContext();
        }

        public void Mark(bool isMarked, long isbn)
        {
            var book = dbContext.Books.SingleOrDefault(b => b.Isbn == isbn);
            if (book == null) throw new Exception("Can't find a book with ISBN = "+isbn);
            book.IsMakred = isMarked;
            dbContext.Entry(book).State = EntityState.Modified;
            dbContext.SaveChanges();
        }


    }
}