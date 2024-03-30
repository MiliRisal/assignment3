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
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var order = _context.Orders.Include(o => o.User);
            return Json(order);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Json(order);
        }

        // GET: Order/Create
        // public IActionResult Create()
        // {
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        //     return View();
        // }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return Ok(new {message = "Order made successfully.", order});
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Order/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var order = await _context.Orders.FindAsync(id);
        //     if (order == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
        //     return View(order);
        // }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new {message = "Order edited successfully.", order});
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Order/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var order = await _context.Orders
        //         .Include(o => o.User)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (order == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(order);
        // }

        // POST: Order/Delete/5
        [HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return Ok(new {message = "Order deleted successfully.",id});
        }

        private bool OrderExists(int? id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
