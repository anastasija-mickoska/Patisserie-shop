using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patisserie.Areas.Identity.Data;
using Patisserie.Data;
using Patisserie.Models;
using Patisserie.ViewModels;

namespace Patisserie.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly PatisserieContext _context;
        private readonly UserManager<PatisserieUser> _userManager;

        public ReviewsController(PatisserieContext context, UserManager<PatisserieUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var patisserieContext = _context.Review.Include(r => r.Product).Include(r => r.User);
            return View(await patisserieContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var userEmail = user?.Email;
            var userId = user?.Id; // Retrieve the UserId

            ViewData["UserEmail"] = userEmail;
            ViewData["UserId"] = userId; // Add UserId to ViewData
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,ProductId, UserId,Comment,Rating")] Review review)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user!=null)
            {
                review.UserId = user.Id; 
            }
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", review.ProductId);
            return View(review);
        }
        [HttpGet]
        public async Task<IActionResult> CreateReview(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            var viewModel = new CreateReviewViewModel
            {
                Review = new Review { ProductId = product.ProductId }, 
                User = currentUser,
                ProductName = product.Name 
            };

            ViewData["ProductName"] = product.Name;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Review.UserId = viewModel.User.Id; 
                _context.Review.Add(viewModel.Review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            var user = _context.Users.SingleOrDefault(u => u.Id == review.UserId);
            ViewData["UserName"] = user.UserName;
            ViewData["ProductName"] = new SelectList(_context.Product, "ProductId", "Name", review.ProductId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,ProductId,UserId,Comment,Rating")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(review.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserName"] = user.UserName;
            ViewData["ProductName"] = new SelectList(_context.Product, "ProductId", "Name", review.ProductId);
            return View(review);
        }


        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ReviewId == id);
        }
    }
}
