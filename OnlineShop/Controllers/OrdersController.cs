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
    public class OrdersController : Controller
    {
        private readonly ShopContext _context;

        public OrdersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderProducts) 
                .ThenInclude(op => op.Product); 
            return View(await shopContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.CourierId = new SelectList(_context.Couriers, "Id", "Name");
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name");
            ViewBag.PaymentId = new SelectList(_context.Payments, "Id", "Id");
            ViewBag.ShippingAddressId = new SelectList(_context.Addresses.Where(a => !a.IsDeleted), "Id", "Street");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");

            // Передаємо значення enum OrderStatus у View через ViewBag
            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus));

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,Price,OrderDate,ShippingDate,ShippingAddressId,Status,PaymentId,CourierId,DiscountCode,LastEditted")] Order order, string[] ProductIds, int[] Quantities)
        {
            if (ModelState.IsValid)
            {
                order.LastEditted = DateTime.UtcNow;

                _context.Add(order);
                await _context.SaveChangesAsync();

                for (int i = 0; i < ProductIds.Length; i++)
                {
                    var product = await _context.Products.FindAsync(ProductIds[i]);

                    if (product != null)
                    {
                        var orderProduct = new OrderProduct
                        {
                            OrderId = order.Id,     
                            ProductId = product.Id,
                            Quantity = Quantities[i],
                            Price = product.Price 
                        };
                        _context.OrderProducts.Add(orderProduct);
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Product with ID {ProductIds[i]} not found.");
                        return View(order); 
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourierId"] = new SelectList(_context.Couriers, "Id", "Name", order.CourierId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", order.PaymentId);
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.CourierId = new SelectList(_context.Couriers, "Id", "Name", order.CourierId);
            ViewBag.CustomerId = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewBag.PaymentId = new SelectList(_context.Payments, "Id", "Id", order.PaymentId);
            ViewBag.ShippingAddressId = new SelectList(_context.Addresses.Where(a => !a.IsDeleted), "Id", "Street");

            // Pass enum values for order statuses to the view
            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CustomerId,Price,OrderDate,ShippingDate,ShippingAddressId,Status,PaymentId,CourierId,DiscountCode,LastEditted,ProductId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.LastEditted = DateTime.UtcNow; // Автоматично оновлюємо LastEditted
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
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourierId"] = new SelectList(_context.Couriers, "Id", "Name", order.CourierId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", order.PaymentId);
            ViewData["ShippingAddressId"] = new SelectList(_context.Addresses, "Id", "Street", order.ShippingAddressId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
