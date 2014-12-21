using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Xml;

namespace Web.Models
{
    public class BooksModel
    {
        private List<Book> books = new List<Book>();

        private DbBooksContext dbContext = null;

        public List<Book> Books { get { return books; } }
        public BooksModel(String isbns)
        {
            dbContext = new DbBooksContext();
            LoadBooks(isbns);
        }

        private void LoadBooks(string isbns)
        {
            var newRecords = new List<Book>();
            var isbnList = isbns.Split(' ');
            foreach (var isbn in isbnList)
            {
                LoadBook(isbn, newRecords);
            }
            if (newRecords.Count > 0)
            {
                dbContext.Books.AddRange(newRecords);
                dbContext.SaveChanges();
            }
        }

        private void LoadBook(string isbn, List<Book> newRecords)
        {
            if(String.IsNullOrEmpty(isbn)) return;
            var key = Convert.ToInt64(isbn);
            var book = dbContext.Books.SingleOrDefault(b => b.Isbn == key);
            if (book != null) { books.Add(book); return;}
            var uri = "http://api.saxo.com/v1/products/products.json?key=" + 
                WebConfigurationManager.AppSettings["ApiKey"] + "&isbn=" + isbn;
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var json = streamReader.ReadToEnd();
                AddBook(json, isbn, newRecords);
            }
            response.Close();
        }

        private void AddBook(String json, string isbn, List<Book> newRecords)
        {
            var jss = new JavaScriptSerializer();

            var data = jss.Deserialize<dynamic>(json);
            if (!data.ContainsKey("products")) return;

            var products = data["products"];
            if (products.Length==0) return;
            var product = products[0];
            var id = Convert.ToInt64(product["id"]);
            var book = new Book() { Isbn = Convert.ToInt64(isbn), Id = id };
            book.Title = product["title"];
            book.ImageUrl = product["imageurl"];
            if (product.ContainsKey("contributors"))
                book.Authors = GetAuthors(product["contributors"]);
            book.Rating = Convert.ToInt32(product["ratingavg"]);
            books.Add(book);
            newRecords.Add(book);
        }

        private string GetAuthors(dynamic contributors)
        {
            if (contributors.Length == 0) return String.Empty;
            var authors = String.Empty;
            for (int i = 0; i < contributors.Length; i++)
            {
                if (!String.IsNullOrEmpty(authors)) authors += " & ";
                authors += contributors[i]["name"];
            }
            return "af "+authors;
        }
    }
}