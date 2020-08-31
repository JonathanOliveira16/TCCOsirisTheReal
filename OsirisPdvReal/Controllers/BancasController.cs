using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class BancasController : Controller
    {
        private readonly Contexto _context;

        public BancasController(Contexto context)
        {
            _context = context;
        }

        // GET: Bancas
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Bancas.Include(j => j.Jornaleiro).AsNoTracking().OrderBy(j => j.NomeBanca);
            //var contexto = _context.Bancas.Include(b => b.Jornaleiro);
            var model = await PagingList.CreateAsync(query, 5, page);

            return View(model);
        }

        // GET: Bancas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas
                .Include(b => b.Jornaleiro)
                .FirstOrDefaultAsync(m => m.BancaId == id);
            if (banca == null)
            {
                return NotFound();
            }

            return View(banca);
        }

        // GET: Bancas/Create
        public IActionResult Create()
        {
            ViewData["JornaleiroId"] = new SelectList(_context.Jornaleiros, "JornaleiroId", "EmailJornaleiro");
            return View();
        }

        // POST: Bancas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BancaId,NomeBanca,JornaleiroId")] Banca banca)
        {
            if (ModelState.IsValid)
            {

                var existe = _context.Bancas.Where(b => b.NomeBanca == banca.NomeBanca).Select(b => b.NomeBanca).FirstOrDefault();

                if (existe != null)
                {
                    _context.Add(banca);
                    _context.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["Mensagem"] = "Essa Banca já existe, Por favor escolha outro Nome";
                    return View();
                }

            }
            return View();
        }

        // GET: Bancas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas.FindAsync(id);
            if (banca == null)
            {
                return NotFound();
            }
            ViewData["JornaleiroId"] = new SelectList(_context.Jornaleiros, "JornaleiroId", "EmailJornaleiro", banca.JornaleiroId);
            return View(banca);
        }

        // POST: Bancas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BancaId,NomeBanca,JornaleiroId")] Banca banca)
        {
            if (id != banca.BancaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BancaExists(banca.BancaId))
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
            ViewData["JornaleiroId"] = new SelectList(_context.Jornaleiros, "JornaleiroId", "EmailJornaleiro", banca.JornaleiroId);
            return View(banca);
        }

        // GET: Bancas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banca = await _context.Bancas
                .Include(b => b.Jornaleiro)
                .FirstOrDefaultAsync(m => m.BancaId == id);
            if (banca == null)
            {
                return NotFound();
            }

            return View(banca);
        }

        // POST: Bancas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banca = await _context.Bancas.FindAsync(id);
            _context.Bancas.Remove(banca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BancaExists(int id)
        {
            return _context.Bancas.Any(e => e.BancaId == id);
        }
    }
}
