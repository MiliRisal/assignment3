using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;

namespace Assignment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comments>>> GetComments()
        {
            var comments = await _context.Comments
                                         .Include(c => c.User)
                                         .Include(c => c.Product)
                                         .ToListAsync();
            return Ok(comments);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetComment(int id)
        {
            var comment = await _context.Comments.Include(c => c.User)
                                                .Include(c => c.Product).FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comments>> CreateComment([FromBody] Comments comments)
        {
            // Check if the provided ProductId exists in the database
            var existingProduct = await _context.Products.FindAsync(comments?.Product?.Id);
            if (existingProduct == null)
            {
                ModelState.AddModelError("Product", "The specified product does not exist.");
                return BadRequest(ModelState);
            }

            // Check if the provided UserId exists in the database
            var existingUser = await _context.Users.FindAsync(comments?.UserId);
            if (existingUser == null)
            {
                ModelState.AddModelError("UserId", "The specified user does not exist.");
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                // Set the navigation properties
                comments.Product = existingProduct;
                comments.User = existingUser;

                _context.Comments.Add(comments);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetComment", new { id = comments.Id }, comments);
            }

            return BadRequest(ModelState);
        }


        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] Comments comments)
        {
            if (id != comments.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(comments).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new { message = "Comment updated successfully.", comments });
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comments>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment deleted successfully.", id });
        }

        private bool CommentsExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
