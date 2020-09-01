﻿using System;
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

        [HttpPost]
        public async Task<IActionResult> Index(string busca, int page=1)
        {
            try
            {
                if (busca == null)
                {
                    var query = _context.Bancas.Include(j => j.Jornaleiro).AsNoTracking().OrderBy(j => j.NomeBanca);
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View(model);
                }
                else
                {
                    List<Banca> listaDasBancas = new List<Banca>();
                    var bancas = _context.Bancas.Include(j => j.Jornaleiro).Where(b => b.NomeBanca.Contains(busca)).OrderBy(b => b.NomeBanca);
                    var model = await PagingList.CreateAsync(bancas, 5, page);
                    return View(model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }
        
            
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
            try
            {
                if (ModelState.IsValid)
                {

                    var existe = _context.Bancas.Where(b => b.NomeBanca == banca.NomeBanca).Select(b => b.NomeBanca).FirstOrDefault();

                    if (existe == null)
                    {
                        _context.Add(banca);
                        _context.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewData["JornaleiroId"] = new SelectList(_context.Jornaleiros, "JornaleiroId", "EmailJornaleiro");
          
                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }

                }
            }
            catch (Exception)
            {
                ViewData["JornaleiroId"] = new SelectList(_context.Jornaleiros, "JornaleiroId", "EmailJornaleiro");
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
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
                    var existe = _context.Bancas.Where(b => b.NomeBanca == banca.NomeBanca).Select(b => b.NomeBanca).FirstOrDefault();
                    if (existe == null)
                    {
                        _context.Update(banca);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }
                    
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

      
        // POST: Bancas/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var temCompra = _context.Bancas.Select(b => b.Vendas).ToList();
            if (temCompra.Count() > 0)
            {
                TempData["msgSucesso"] = "Banca vinculada à uma venda, logo não é possivel realizar sua exclusão";
                return RedirectToAction(nameof(Index));

            }
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
