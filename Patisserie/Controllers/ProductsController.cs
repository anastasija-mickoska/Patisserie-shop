using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patisserie.Data;
using Patisserie.Models;
using Patisserie.ViewModels;
using Patisserie.Areas.Identity.Data;

namespace Patisserie.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PatisserieContext _context;
        private readonly UserManager<PatisserieUser> _userManager;

        public ProductsController(PatisserieContext context, UserManager<PatisserieUser> userManager)
        {
            _context = context;
            _userManager= userManager;
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BuyProduct(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var userProduct = new UserProduct
            {
                UserId = user.Id,
                ProductId = product.ProductId,
            };

            _context.UserProducts.Add(userProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyOrders));
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var userProducts = await _context.UserProducts
                .Include(up => up.Product)
                .Where(up => up.UserId == user.Id)
                .ToListAsync();

            return View(userProducts);
        }
        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            IQueryable<Product> products = _context.Product.Include(m => m.Category).Include(m => m.ProductFlavours).ThenInclude(m => m.Flavour);
            IQueryable<string> name = _context.Product.OrderBy(b => b.Name).Select(b => b.Name).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(b =>
                    b.Name.Contains(searchString) ||
                    b.Category.Name.Contains(searchString) ||
                    b.ProductFlavours.Any(pf => pf.Flavour.Name.Contains(searchString)));
            }

            var product = new ProductFilterViewModel
            {
                Products = await products.ToListAsync(),
                Filters = new SelectList(await name.ToListAsync())
            };

            return View(product);

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product
                          .Include(p => p.Category)
                          .Include(p => p.ProductFlavours)
                          .ThenInclude(p => p.Flavour)
                          .Include(p => p.Reviews)
                          .ThenInclude(p => p.User)
                          .FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var averageRating = product.AverageRating();
            ViewData["AverageRating"] = averageRating;

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var flavours = _context.Flavour.OrderBy(s => s.Name).ToList();
            var viewmodel = new ProductsFlavoursEditViewModel
            {
                Product = new Product(),
                FlavourList = flavours.Select(f => new SelectListItem { Value = f.FlavourId.ToString(), Text = f.Name })
            };

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");

            return View(viewmodel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsFlavoursEditViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    // Add the product to the database
                    _context.Add(viewmodel.Product);
                    await _context.SaveChangesAsync();

                    // Assign the selected flavours
                    if (viewmodel.SelectedFlavours != null && viewmodel.SelectedFlavours.Any())
                    {
                        foreach (int flavourId in viewmodel.SelectedFlavours)
                        {
                            _context.ProductFlavours.Add(new ProductFlavour { FlavourId = flavourId, ProductId = viewmodel.Product.ProductId });
                        }
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                }
            }

            // Repopulate ViewData and FlavourList in case of validation failure
            var flavours = _context.Flavour.OrderBy(s => s.Name).ToList();
            viewmodel.FlavourList = flavours.Select(f => new SelectListItem { Value = f.FlavourId.ToString(), Text = f.Name });

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", viewmodel.Product.CategoryId);

            return View(viewmodel);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                                        .Where(m => m.ProductId == id)
                                        .Include(m => m.ProductFlavours)
                                        .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }

            var flavours = _context.Flavour.OrderBy(s => s.Name).ToList();

            var viewmodel = new ProductsFlavoursEditViewModel
            {
                Product = product,
                FlavourList = flavours.Select(f => new SelectListItem { Value = f.FlavourId.ToString(), Text = f.Name }),
                SelectedFlavours = product.ProductFlavours.Select(pf => pf.FlavourId).ToList()
            };

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", product.CategoryId);
            return View(viewmodel);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsFlavoursEditViewModel viewmodel)
        {
            if (id != viewmodel.Product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Product);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newFlavourList = viewmodel.SelectedFlavours ?? Enumerable.Empty<int>();
                    IEnumerable<int> prevFlavourList = _context.ProductFlavours.Where(s => s.ProductId == id).Select(s => s.FlavourId);

                    var toBeRemoved = _context.ProductFlavours.Where(s => s.ProductId == id && !newFlavourList.Contains(s.FlavourId));
                    _context.ProductFlavours.RemoveRange(toBeRemoved);
                    foreach (int flavourId in newFlavourList)
                    {
                        if (!prevFlavourList.Contains(flavourId))
                        {
                            _context.ProductFlavours.Add(new ProductFlavour { FlavourId = flavourId, ProductId = id });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewmodel.Product.ProductId))
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

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", viewmodel.Product.CategoryId);
            var flavours = _context.Flavour.OrderBy(s => s.Name).ToList();
            viewmodel.FlavourList = flavours.Select(f => new SelectListItem { Value = f.FlavourId.ToString(), Text = f.Name });
            return View(viewmodel);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductFlavours)
                .ThenInclude(p => p.Flavour)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
