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
    public class AddressesController : Controller
    {
        private readonly ShopContext _context;

        public AddressesController(ShopContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Addresses.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Settlement,Street,HouseNumber,ApartmentNumber,ZipCode, LastEdited, IsDeleted")] Address address)
        {
            if (ModelState.IsValid)
            {
                address.LastEdited = DateTime.UtcNow;
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Settlement,Street,HouseNumber,ApartmentNumber,ZipCode,LastEdited,IsDeleted")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    address.LastEdited = DateTime.UtcNow; 
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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

            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                address.IsDeleted = true; 
                _context.Update(address); 
            }

            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }

        // POST: Addresses/Undo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Undo(string id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                address.IsDeleted = false; 
                _context.Update(address);  
                await _context.SaveChangesAsync();  
            }

            return RedirectToAction(nameof(Index));  
        }

        private bool AddressExists(string id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
