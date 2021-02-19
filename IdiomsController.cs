using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using English_Idioms.Data;
using English_Idioms.Models;
using Microsoft.AspNetCore.Authorization;

namespace English_Idioms.Controllers
{
    public class IdiomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IdiomsController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        // GET: Idioms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Idioms.ToListAsync());
        }

        // GET: Idioms/show_search_form
        public async Task<IActionResult> show_search_form()
        {
            return View();
        }

        // Post: Idioms/show_search_results
        public async Task<IActionResult> show_search_results(String SearchPhrase)
        {
            return View("index", await _context.Idioms.Where( j => j.Idiom.Contains(SearchPhrase) ).ToListAsync());
        }

        // GET: Idioms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idioms = await _context.Idioms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idioms == null)
            {
                return NotFound();
            }

            return View(idioms);
        }

        // GET: Idioms/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Idioms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idiom,definition,Example")] Idioms idioms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idioms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idioms);
        }

        // GET: Idioms/Edit/5

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idioms = await _context.Idioms.FindAsync(id);
            if (idioms == null)
            {
                return NotFound();
            }
            return View(idioms);
        }

        // POST: Idioms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idiom,definition,Example")] Idioms idioms)
        {
            if (id != idioms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idioms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdiomsExists(idioms.Id))
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
            return View(idioms);
        }

        // GET: Idioms/Delete/5

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idioms = await _context.Idioms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idioms == null)
            {
                return NotFound();
            }

            return View(idioms);
        }

        // POST: Idioms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idioms = await _context.Idioms.FindAsync(id);
            _context.Idioms.Remove(idioms);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdiomsExists(int id)
        {
            return _context.Idioms.Any(e => e.Id == id);
        }
    }
}
