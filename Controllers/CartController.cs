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
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var carts = await _context.Carts
                                    .Include(c => c.User)
                                    .Include(c => c.Products)
                                    .ToListAsync();
            return Ok(carts);
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var cart = await _context.Carts
                                    .Include(c => c.User)
                                    .Include(c => c.Products)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        // POST: api/Cart
[HttpPost]
public async Task<ActionResult<Cart>> CreateCart([FromBody] Cart cart)
{
    if (cart.Products == null || cart.Products.Count == 0)
    {
        ModelState.AddModelError("Products", "Cart must contain at least one product.");
        return BadRequest(ModelState);
    }

    if (cart.UserId == null)
    {
        ModelState.AddModelError("UserId", "UserId is required.");
        return BadRequest(ModelState);
    }

    if (cart.Quantities == null || cart.Quantities.Count != cart.Products.Count)
    {
        ModelState.AddModelError("Quantities", "Quantities should be provided for each product.");
        return BadRequest(ModelState);
    }

    // Check if the user exists
    var existingUser = await _context.Users.FindAsync(cart.UserId);
    if (existingUser == null)
    {
        ModelState.AddModelError("UserId", "User with this ID does not exist.");
        return BadRequest(ModelState);
    }

    // Create a new list to store valid products
    List<Product> validProducts = new List<Product>();

    // Now we can create the cart
    var newCart = new Cart
    {
        Products = new List<Product>(),
        UserId = cart.UserId,
        User = existingUser,
        Quantities = new List<int>()
    };

    // Iterate through each product in the cart
    for (int i = 0; i < cart.Products.Count; i++)
    {
        var product = cart.Products[i];
        var quantity = cart.Quantities[i];

        // Check if the product ID is null
        if (product.Id == null)
        {
            ModelState.AddModelError("Products", "Product ID cannot be null.");
            return BadRequest(ModelState);
        }

        // Check if the product exists
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null)
        {
            ModelState.AddModelError("Products", $"Product with ID {product.Id} does not exist.");
            return BadRequest(ModelState);
        }

        // If the product exists, add it to the list of valid products
        validProducts.Add(existingProduct);
        newCart.Quantities.Add(quantity); // Add the quantity for this product
    }

    // Set the products for the new cart
    newCart.Products = validProducts;

    // Add the new cart to the context and save changes
    _context.Carts.Add(newCart);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetCart", new { id = newCart.Id }, newCart);
}



   // PUT: api/Cart/{id}
[HttpPut("{id}")]
public async Task<IActionResult> EditCart(int id, [FromBody] Cart updatedCart)
{
    if (id != updatedCart.Id)
    {
        return BadRequest("Cart ID mismatch.");
    }

    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        var existingCart = await _context.Carts
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (existingCart == null)
        {
            return NotFound("Cart not found.");
        }

        // Update Products and Quantities if provided
        if (updatedCart.Products != null && updatedCart.Quantities != null)
        {
            if (updatedCart.Products.Count != updatedCart.Quantities.Count)
            {
                ModelState.AddModelError("Quantities", "Quantities should be provided for each product.");
                return BadRequest(ModelState);
            }

            existingCart.Products.Clear(); // Clear existing products
            existingCart.Quantities.Clear(); // Clear existing quantities

            for (int i = 0; i < updatedCart.Products.Count; i++)
            {
                var product = updatedCart.Products[i];
                var quantity = updatedCart.Quantities[i];

                // Check if the product ID is null
                if (product.Id == null)
                {
                    ModelState.AddModelError("Products", "Product ID cannot be null.");
                    return BadRequest(ModelState);
                }

                // Check if the product exists
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct == null)
                {
                    ModelState.AddModelError("Products", $"Product with ID {product.Id} does not exist.");
                    return BadRequest(ModelState);
                }

                existingCart.Products.Add(existingProduct);
                existingCart.Quantities.Add(quantity);
            }
        }

        _context.Entry(existingCart).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Cart updated successfully.", cart = existingCart });
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!CartExists(id))
        {
            return NotFound("Cart not found.");
        }
        else
        {
            throw;
        }
    }
}


// DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int? id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cart deleted successfully.", cart });
        }


        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
