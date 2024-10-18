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
    public class RepresentativesController : Controller
    {
        private readonly ShopContext _context;

        public RepresentativesController(ShopContext context)
        {
            _context = context;
        }

        // GET: Representatives
        public async Task<IActionResult> Index()
        {
            return View(await _context.Representatives.ToListAsync());
        }

        // GET: Representatives/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (representative == null)
            {
                return NotFound();
            }

            return View(representative);
        }

        // GET: Representatives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Representatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,IsDeleted")] Representative representative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(representative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(representative);
        }

        // GET: Representatives/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives.FindAsync(id);
            if (representative == null)
            {
                return NotFound();
            }
            return View(representative);
        }

        // POST: Representatives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,Password,IsDeleted")] Representative representative)
        {
            if (id != representative.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(representative);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepresentativeExists(representative.Id))
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
            return View(representative);
        }

        // GET: Representatives/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (representative == null)
            {
                return NotFound();
            }

            return View(representative);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var repres = await _context.Representatives.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);
            if (repres != null)
            {
                repres.IsDeleted = true;
                _context.Update(repres);

                // Встановлюємо IsDeleted для всіх продуктів цього бренду
                foreach (var product in repres.Products)
                {
                    product.IsDeleted = true;
                    _context.Update(product);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Brands/Undo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Undo(string id)
        {
            var repres = await _context.Representatives.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);
            if (repres != null)
            {
                repres.IsDeleted = false;
                _context.Update(repres);

                foreach (var product in repres.Products)
                {
                    product.IsDeleted = false;
                    _context.Update(product);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RepresentativeExists(string id)
        {
            return _context.Representatives.Any(e => e.Id == id);
        }
    }
}
