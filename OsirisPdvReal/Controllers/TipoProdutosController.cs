using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class TipoProdutosController : Controller
    {
        private readonly Contexto _context;
        private static List<TipoProduto> ListaParaCsv = new List<TipoProduto>();
        public TipoProdutosController(Contexto context)
        {
            _context = context;
        }

        // GET: TipoProdutos
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            var query = _context.TipoProdutos.OrderBy(v=>v.NomeTipoProduto);
            ListaParaCsv = query.ToList();
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        // GET: TipoProdutos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoProduto = await _context.TipoProdutos
                .FirstOrDefaultAsync(m => m.TipoProdId == id);
            if (tipoProduto == null)
            {
                return NotFound();
            }

            return View(tipoProduto);
        }

        // GET: TipoProdutos/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            return View();
        }

        // POST: TipoProdutos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoProdId,NomeTipoProduto")] TipoProduto tipoProduto)
        {
            if (ModelState.IsValid)
            {
                var temT = _context.TipoProdutos.Where(t => t.NomeTipoProduto == tipoProduto.NomeTipoProduto && t.TipoProdId != tipoProduto.TipoProdId).Select(t => t.NomeTipoProduto).FirstOrDefault();
                if (temT == null)
                {
                    _context.Add(tipoProduto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                    return View();
                }

            }
            return View(tipoProduto);
        }

        // GET: TipoProdutos/Edit/5
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

            var tipoProduto = await _context.TipoProdutos.FindAsync(id);
            if (tipoProduto == null)
            {
                return NotFound();
            }
            return View(tipoProduto);
        }

        // POST: TipoProdutos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoProdId,NomeTipoProduto")] TipoProduto tipoProduto)
        {
            if (id != tipoProduto.TipoProdId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var temT = _context.TipoProdutos.Where(t => t.NomeTipoProduto == tipoProduto.NomeTipoProduto && t.TipoProdId != tipoProduto.TipoProdId).Select(t => t.NomeTipoProduto).FirstOrDefault();
                    if (temT == null)
                    {
                        _context.Update(tipoProduto);
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
                    if (!TipoProdutoExists(tipoProduto.TipoProdId))
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
            return View(tipoProduto);
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("Nome Tipo");

            foreach (var item in registros)
            {


                arquivo.AppendLine(item.NomeTipoProduto);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "TipoProduto.csv");
        }


        // GET: TipoProdutos/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tipoProduto = await _context.TipoProdutos
        //        .FirstOrDefaultAsync(m => m.TipoProdId == id);
        //    if (tipoProduto == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tipoProduto);
        //}

        // POST: TipoProdutos/Delete/5
        [HttpPost]
        public async Task<string> Delete(int id)
        {
            var prodsComTipo = _context.Produto.Where(p => p.TipoProdId == id).Select(p=>p.NomeProduto).FirstOrDefault();
            if (prodsComTipo == null)
            {
                var tipoProduto = await _context.TipoProdutos.FindAsync(id);
                _context.TipoProdutos.Remove(tipoProduto);
                await _context.SaveChangesAsync();
                return "ok";
            }

            return "bad";
        }

        private bool TipoProdutoExists(int id)
        {
            return _context.TipoProdutos.Any(e => e.TipoProdId == id);
        }
    }
}
