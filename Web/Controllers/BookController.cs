using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private BookMarker bookMarker = new BookMarker();

        public ActionResult GetList(String isbns)
        {
            return PartialView("_List", new BooksModel(isbns));
        }

        [HttpPost]
        public ActionResult Mark(bool isMarked, long isbn)
        {
            var error = string.Empty;
            try
            {
                bookMarker.Mark(isMarked,isbn);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return Json(new { error = error });
        }

	}
}