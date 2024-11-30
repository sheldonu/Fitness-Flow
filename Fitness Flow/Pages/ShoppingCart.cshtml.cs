using Fitness_Flow.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitness_Flow.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ShoppingCart> CartItems { get; set; } = new List<ShoppingCart>();
        public decimal TotalPrice { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            CartItems = await _context.ShoppingCart
                .Where(item => item.UserId == userId)
                .Include(item => item.Product)
                .ToListAsync();

            TotalPrice = CartItems.Sum(item => item.Quantity * item.Product.Price);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var userId = _userManager.GetUserId(User);

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(item => item.ProductId == productId && item.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1
                };
                _context.ShoppingCart.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.ShoppingCart.FindAsync(cartItemId);

            if (cartItem != null)
            {
                _context.ShoppingCart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int cartItemId, int quantity)
        {
            if (quantity < 1)
            {
                ModelState.AddModelError(string.Empty, "Quantity must be at least 1.");
                return RedirectToPage();
            }

            var cartItem = await _context.ShoppingCart.FindAsync(cartItemId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSubmitOrderAsync()
        {
            var userId = _userManager.GetUserId(User);

            // Get all cart items for the user
            var cartItems = await _context.ShoppingCart
                .Where(item => item.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                // Clear the cart
                _context.ShoppingCart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                // Set success message
                TempData["OrderMessage"] = "Your order has been submitted successfully!";
            }
            else
            {
                TempData["OrderMessage"] = "Your cart is empty. Please add items before submitting an order.";
            }

            return RedirectToPage();
        }

    }
}
