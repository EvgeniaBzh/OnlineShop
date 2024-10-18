using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Products
                .Include(p => p.Brand)     
                .Include(p => p.Category)     
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Weight = p.Weight,
                    StockQuantity = p.StockQuantity,
                    Brand = p.Brand,      
                    Category = p.Category,     
                    LastEdited = p.LastEdited
                });

            return View(await shopContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Representative)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.BrandId = new SelectList(_context.Brands.Where(a => !a.IsDeleted), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_context.Categories.Where(a => !a.IsDeleted), "Id", "Name");
            ViewData["RepresentativeId"] = new SelectList(_context.Representatives, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Weight,StockQuantity,CategoryId,BrandId,RepresentativeId,Image,LastEdited,IsDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["RepresentativeId"] = new SelectList(_context.Representatives, "Id", "Name", product.RepresentativeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.BrandId = new SelectList(_context.Brands.Where(a => !a.IsDeleted), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_context.Categories.Where(a => !a.IsDeleted), "Id", "Name");
            ViewData["RepresentativeId"] = new SelectList(_context.Representatives, "Id", "Name", product.RepresentativeId); // "Id", "Name"
            return View(product);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,Price,Weight,StockQuantity,CategoryId,BrandId,RepresentativeId,Image,LastEdited,IsDeleted")] Product product)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["RepresentativeId"] = new SelectList(_context.Representatives, "Id", "Name", product.RepresentativeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Representative)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod != null)
            {
                prod.IsDeleted = true;
                _context.Update(prod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Addresses/Undo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Undo(string id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod != null)
            {
                prod.IsDeleted = false;
                _context.Update(prod);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
