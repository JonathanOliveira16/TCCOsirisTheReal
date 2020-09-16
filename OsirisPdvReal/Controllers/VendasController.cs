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
    public class VendasController : Controller
    {
        private readonly Contexto _context;

        public VendasController(Contexto context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {

            ViewBag.produtos =  _context.Produto.AsNoTracking().OrderBy(c => c.NomeProduto);
            var contexto = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status);
            return View(await contexto.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Bancas)
                .Include(v => v.Clientes)
                .Include(v => v.Status)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create(int page = 1)
        {
            ViewBag.produtos = _context.Produto.AsNoTracking().OrderBy(c => c.NomeProduto);
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca");
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,DataVenda,ValorVenda,StatusId,BancaId,CPFcliente")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("VendaId,DataVenda,ValorVenda,StatusId,BancaId,CPFcliente")] Venda venda)
        {
            if (id != venda.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaId))
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
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Bancas)
                .Include(v => v.Clientes)
                .Include(v => v.Status)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int? id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
    }
}
