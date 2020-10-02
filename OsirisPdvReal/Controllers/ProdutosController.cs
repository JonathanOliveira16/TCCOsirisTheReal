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
    public class ProdutosController : Controller
    {
        private readonly Contexto _context;

        public ProdutosController(Contexto context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewBag.categorias = _context.TipoProdutos.Select(t => t.NomeTipoProduto).ToList();
            var query = _context.Produto.Include(p => p.tipoProduto).AsNoTracking().OrderBy(j => j.NomeProduto);
            //var contexto = _context.Bancas.Include(b => b.Jornaleiro);
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string busca, int page = 1)
        {
            try
            {
                if (busca == null)
                {
                    var query = _context.Produto.Include(p=>p.tipoProduto).AsNoTracking().OrderBy(j => j.NomeProduto);
                    var model = await PagingList.CreateAsync(query, 5, page);
                    ViewBag.categorias = _context.TipoProdutos.Select(t => t.NomeTipoProduto).ToList();

                    return View(model);
                }
                else
                {
                    List<Produto> listaDeProdutos = new List<Produto>();
                    var produtos = _context.Produto.Include(p => p.tipoProduto).Where(b => b.NomeProduto.Contains(busca)).OrderBy(b => b.NomeProduto);
                    var model = await PagingList.CreateAsync(produtos, 5, page);
                    ViewBag.categorias = _context.TipoProdutos.Select(t => t.NomeTipoProduto).ToList();

                    return View(model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }


        }


        [HttpPost]
        public async Task<IActionResult> BuscarTipo(string tipoBusca, int page = 1)
        {
            try
            {
                if (tipoBusca == null)
                {
                    var query = _context.Produto.Include(p => p.tipoProduto).AsNoTracking().OrderBy(j => j.NomeProduto);
                    var model = await PagingList.CreateAsync(query, 5, page);
                    ViewBag.categorias = _context.TipoProdutos.Select(t => t.NomeTipoProduto).ToList();

                    return View("Index", model);
                }
                else
                {
                    List<Produto> listaDeProdutos = new List<Produto>();
                    var produtos = _context.Produto.Include(p => p.tipoProduto).Where(b => b.tipoProduto.NomeTipoProduto.Contains(tipoBusca)).OrderBy(b => b.NomeProduto);
                    var model = await PagingList.CreateAsync(produtos, 5, page);
                    ViewBag.categorias = _context.TipoProdutos.Select(t => t.NomeTipoProduto).ToList();

                    return View("Index", model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }


        }




        // GET: Produtos/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");

            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdutoId,NomeProduto,ValorProduto,QuantideProduto,TipoProdId")] Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var produtoExist = _context.Produto.Where(p => p.NomeProduto == produto.NomeProduto).Select(p => p.NomeProduto).FirstOrDefault();
                    if (produtoExist == null)
                    {
                        
                        _context.Add(produto);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");

                        TempData["msgSucesso"] = "Nome de produto já existente!";
                    }

                }
            }
            catch (Exception)
            {
                ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");

                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";

            }

            return View(produto);
        }

        // GET: Produtos/Edit/5
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

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");

            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,NomeProduto,ValorProduto,QuantideProduto,TipoProdId")] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var produtoExist = _context.Produto.Where(p => p.NomeProduto == produto.NomeProduto && p.ProdutoId != produto.ProdutoId).Select(p => p.NomeProduto).FirstOrDefault();
                    if (produtoExist == null)
                    {
                        _context.Update(produto);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");
                        TempData["msgSucesso"] = "Nome de produto já existente!";

                    }

                }
                catch (Exception ex)
                {
                    ViewData["TipoProdId"] = new SelectList(_context.TipoProdutos, "TipoProdId", "NomeTipoProduto");
                    TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                    return View(produto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var produto = await _context.Produto
        //        .FirstOrDefaultAsync(m => m.ProdutoId == id);
        //    if (produto == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(produto);
        //}

        // POST: Produtos/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            
            var produto = await _context.Produto.FindAsync(id);
            var vendaComProd = _context.VendaProduto.Where(p => p.ProdutoId == id).ToList();
            if (vendaComProd.Count == 0)
            {
                _context.Produto.Remove(produto);
            }
            else
            {
                TempData["msgSucesso"] = "Produto atrelado em uma venda, favor excluir a venda antes do produto!";

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.ProdutoId == id);
        }
    }
}
