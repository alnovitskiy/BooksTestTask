using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DbBooksContext : DbContext
    {
        public DbBooksContext(): base("DefaultConnection")
        {
            
        }

        public DbSet<Book> Books { get; set; }
    }
}