using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Isbn { get; set; }
        [Index(IsUnique = true)]  
        public long Id { get; set; }
        public bool IsMakred { get; set; }
        public String Title { get; set; }
        public String ImageUrl { get; set; }
        public String Authors { get; set; }
        public int Rating { get; set; }
    }
}