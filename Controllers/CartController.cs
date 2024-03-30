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
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var cart = _context.Carts.Include(c => c.User);
            return Json(cart);
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Cart/Create
        // public IActionResult Create()
        // {
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        //     return View();
        // }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return Ok(new {message = "Cart added successfully.", cart});
           }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var cart = await _context.Carts.FindAsync(id);
        //     if (cart == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cart.UserId);
        //     return View(cart);
        // }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromBody] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new {message = "Cart edited successfully.", cart});
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cart.UserId);
            return View(cart);
        }

        // GET: Cart/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var cart = await _context.Carts
        //         .Include(c => c.User)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (cart == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(cart);
        // }

        // POST: Cart/Delete/5
        [HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return Ok(new {message = "Cart deleted successfully.",id});
        }

        private bool CartExists(int? id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
