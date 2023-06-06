using BezeqFinalProject.Common.Data.Contexts;
using BezeqFinalProject.Common.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BezeqFinalProject.WebApp.Controllers {
    public class ProductsController : Controller {
        private readonly MainContext _context;

        public ProductsController(MainContext context) {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index() => View(await _context.Customers.ToListAsync());

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id) {
            if(id == null || _context.Customers == null) return NotFound();

            var product = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if(product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create() => View();

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Product product) {
            if(ModelState.IsValid) {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if(id == null || _context.Customers == null) return NotFound();

            var product = await _context.Customers.FindAsync(id);
            if(product == null) return NotFound();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product) {
            if(id != product.Id) return NotFound();

            if(ModelState.IsValid) {
                try {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                } catch(DbUpdateConcurrencyException) {
                    if(!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if(id == null || _context.Customers == null) return NotFound();

            var product = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if(product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if(_context.Customers == null) return Problem("Entity set 'MainContext.Customers'  is null.");
            var product = await _context.Customers.FindAsync(id);
            if(product != null) _context.Customers.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id) => _context.Customers.Any(e => e.Id == id);
    }
}
