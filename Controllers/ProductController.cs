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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        //get all products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return Json(products);
        }

        // GET: Product/Details/5
        //get single product
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Json(product);
        }

        // GET: Product/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //Create new product
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Ok(new {message = "Product added successfully.", product});
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //edit product
        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new {message = "Product edited successfully.", product});
            }
            return View(product);
        }

        // GET: Product/Delete/5
        // public async Task<IActionResult> Delete(string id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var product = await _context.Products
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (product == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(product);
        // }

        // POST: Product/Delete/5
        [HttpDelete, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return Ok(new {message = "Product deleted successfully.",id});
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
