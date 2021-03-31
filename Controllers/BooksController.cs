using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() =>
            _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut]
        [Route("updateNew")]
        public IActionResult UpdateData(Dictionary<string, string> body)
        {
            string updateKey = body["key"];
            string updateValue = body["value"];
            string id = body["id"];

            _bookService.UpdateNew(id, updateKey, updateValue);

           return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }


        // Dummy function to insert array based data.
        [HttpPost]
        [Route("insertArray")]
        public IActionResult InsertArray()
        {
            _bookService.InsertArray();

            return Ok();
        }

        // Example of Updating the array using POSITIONAL operator
        [HttpPut]
        [Route("updateArray")]
        public IActionResult UpdateArray()
        {

            _bookService.UpdateArray();
            return Ok();
        }

        // Example of updating the array elements with array filters
        [HttpPut]
        [Route("updateArrayInsideElement")]
        public IActionResult UpdateArrayInsideElement()
        {

            _bookService.UpdateElementInsideArray();
            return Ok();
        }
    }
}
