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
    public class ComprasController : Controller
    {
        private readonly Contexto _context;

        public ComprasController(Contexto context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            var query = _context.Compras.Include(c => c.Status).Include(c => c.fornecedor).OrderBy(c=>c.DataCompra);
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DateTime busca, int page = 1)
        {
            if (busca == DateTime.MinValue)
            {
                var query2 = _context.Compras.Include(c => c.Status).Include(c => c.fornecedor).OrderBy(c => c.DataCompra);
                var model2 = await PagingList.CreateAsync(query2, 5, page);
                return View(model2);
            }
            var query = _context.Compras.Include(c => c.Status).Include(c => c.fornecedor).Where(c => c.DataCompra == busca).OrderBy(c=>c.DataCompra);
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }


        // GET: Compras/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "NomeProduto", "NomeProduto").OrderBy(c=>c.Text);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            ViewData["CNPJ"] = new SelectList(_context.Fornecedores, "CNPJ", "NomeFornecedor");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComprasId,NomeItemCompra,QuantidadeCompra,DataCompra,ValorCompra,StatusId,CNPJ")] Compras compras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compras);
                await _context.SaveChangesAsync();
                var prod = _context.Produto.Where(p => p.NomeProduto == compras.NomeItemCompra).FirstOrDefault();
                prod.QuantideProduto = prod.QuantideProduto + compras.QuantidadeCompra;
                _context.Update(prod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "NomeProduto", "NomeProduto", compras.NomeItemCompra) ;
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", compras.StatusId);
            ViewData["CNPJ"] = new SelectList(_context.Fornecedores, "CNPJ", "NomeFornecedor", compras.CNPJ);
            return View(compras);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            if (id == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras.FindAsync(id);
            if (compras == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "NomeProduto", "NomeProduto", compras.NomeItemCompra);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", compras.StatusId);
            ViewData["CNPJ"] = new SelectList(_context.Fornecedores, "CNPJ", "NomeFornecedor", compras.CNPJ);
            return View(compras);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ComprasId,NomeItemCompra,QuantidadeCompra,DataCompra,ValorCompra,StatusId,CNPJ")] Compras compras)
        {
            if (id != compras.ComprasId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var compraQuantidade = _context.Compras.Where(c => c.ComprasId == compras.ComprasId).Select(c => c.QuantidadeCompra).FirstOrDefault();
                    if (compraQuantidade != compras.QuantidadeCompra)
                    {
                        var produtoComprado = _context.Produto.Where(p => p.NomeProduto == compras.NomeItemCompra).FirstOrDefault();
                        produtoComprado.QuantideProduto = produtoComprado.QuantideProduto - compraQuantidade;
                        _context.Update(produtoComprado);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(compras);
                    await _context.SaveChangesAsync();
                    var prod = _context.Produto.Where(p => p.NomeProduto == compras.NomeItemCompra).FirstOrDefault();
                    prod.QuantideProduto = prod.QuantideProduto + compras.QuantidadeCompra;
                    _context.Update(prod);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprasExists(compras.ComprasId))
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
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "NomeProduto", "NomeProduto", compras.NomeItemCompra);

            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", compras.StatusId);
            ViewData["CNPJ"] = new SelectList(_context.Fornecedores, "CNPJ", "NomeFornecedor", compras.CNPJ);
            return View(compras);
        }

        // GET: Compras/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var compras = await _context.Compras
        //        .Include(c => c.Status)
        //        .Include(c => c.fornecedor)
        //        .FirstOrDefaultAsync(m => m.ComprasId == id);
        //    if (compras == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(compras);
        //}

        // POST: Compras/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var compras = await _context.Compras.FindAsync(id);
            var prod = _context.Produto.Where(p => p.NomeProduto == compras.NomeItemCompra).FirstOrDefault();
            prod.QuantideProduto = prod.QuantideProduto - compras.QuantidadeCompra;
            _context.Compras.Remove(compras);
            await _context.SaveChangesAsync();        
            _context.Update(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprasExists(int? id)
        {
            return _context.Compras.Any(e => e.ComprasId == id);
        }
    }
}
