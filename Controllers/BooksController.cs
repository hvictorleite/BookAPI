using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Controllers {

    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var bookToGet = await _bookRepository.Get(id);

            if (bookToGet == null){
                return NotFound();
            }
            return bookToGet;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromBody] Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            var bookToDelete = await _bookRepository.Get(id);

            if (bookToDelete == null)
                return NotFound();
            
            await _bookRepository.Delete(bookToDelete.Id);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Book book)
        {
            if (id != book.Id)
                return BadRequest();    
            
            await _bookRepository.Update(book);
            return NoContent();

        }
    }
}
