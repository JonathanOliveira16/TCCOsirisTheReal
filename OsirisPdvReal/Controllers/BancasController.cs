using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using OsirisPdvReal.Utils;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class BancasController : Controller
    {
        private readonly Contexto _context;
        private static List<Banca> ListaParaCsv = new List<Banca>();

        public BancasController(Contexto context)
        {
            _context = context;
        }

        // GET: Bancas
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login","Jornaleiros");
            }
            List<String> Bairros = BairroUtil.GetBairros();
            ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
            var query = _context.Bancas.Include(j => j.Jornaleiro).AsNoTracking().OrderBy(j => j.NomeBanca);
            ListaParaCsv.Clear();
            ListaParaCsv = query.ToList();
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
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                    return View(model);
                }
                else
                {
                    List<Banca> listaDasBancas = new List<Banca>();
                    var bancas = _context.Bancas.Include(j => j.Jornaleiro).Where(b => b.NomeBanca.Contains(busca)).OrderBy(b => b.NomeBanca);
                    ListaParaCsv.Clear();
                    ListaParaCsv = bancas.ToList();
                    var model = await PagingList.CreateAsync(bancas, 5, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
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
        public async Task<IActionResult> SearchJornaleiro(string buscaJornaleiro,int page = 1)
        {
            try
            {
                if (buscaJornaleiro == null)
                {
                    var query = _context.Bancas.Include(j => j.Jornaleiro).AsNoTracking().OrderBy(j => j.NomeBanca);
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                    return View("Index", model);
                }
                else
                {
                    List<Banca> listaDasBancas = new List<Banca>();
                    var bancas = _context.Bancas.Include(j => j.Jornaleiro).Where(j => j.Jornaleiro.NomeJornaleiro.Contains(buscaJornaleiro)).OrderBy(b => b.NomeBanca);
                    ListaParaCsv.Clear();
                    ListaParaCsv = bancas.ToList();
                    var model = await PagingList.CreateAsync(bancas, 100, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                    return View("Index", model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> BuscarBairro(string bairroBusca, int page = 1)
        {
            try
            {
                if (bairroBusca == null)
                {
                    var query = _context.Bancas.Include(j => j.Jornaleiro).AsNoTracking().OrderBy(j => j.NomeBanca);
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                    return View("Index",model);
                }
                else
                {
                    List<Banca> listaDasBancas = new List<Banca>();
                    var bancas = _context.Bancas.Include(j => j.Jornaleiro).Where(b => b.Bairro.Contains(bairroBusca)).OrderBy(b => b.NomeBanca);
                    ListaParaCsv.Clear();
                    ListaParaCsv = bancas.ToList();
                    var model = await PagingList.CreateAsync(bancas, 100, page);
                    List<String> Bairros = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                    return View("Index",model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View("Index");
            }


        }

        // GET: Bancas/Details/5


        // GET: Bancas/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            List<String> Bairros = BairroUtil.GetBairros();
            ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
            ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");
            return View();
        }

        // POST: Bancas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BancaId,NomeBanca,CPF,CEPbanca, Bairro")] Banca banca)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var existe = _context.Bancas.Where(b => b.NomeBanca == banca.NomeBanca).Select(b => b.NomeBanca).FirstOrDefault();

                    if (existe == null)
                    {
                        var jornaleiro = _context.Jornaleiros.Where(j => j.CPF == banca.CPF).Select(j => j).FirstOrDefault();
                        banca.Jornaleiro = jornaleiro;
                      
                        _context.Add(banca);
                        _context.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");
                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }

                }
                else
                {
                    ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");

                }
            }
            catch (Exception ex)
            {
                ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");
                List<String> Bairros = BairroUtil.GetBairros();
                ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }
            List<String> Bairros2 = BairroUtil.GetBairros();
            ViewBag.bairros = Bairros2.OrderBy(b => b).ToList();
            return View();
        }

        // GET: Bancas/Edit/5
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

            var banca = await _context.Bancas.FindAsync(id);
            if (banca == null)
            {
                return NotFound();
            }
            List<String> Bairros = BairroUtil.GetBairros();
            ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
            ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro", banca.CPF);
            return View(banca);
        }

        // POST: Bancas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BancaId,NomeBanca,CPF,CEPbanca,Bairro")] Banca banca)
        {
            if (id != banca.BancaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existe = _context.Bancas.Where(b => b.NomeBanca == banca.NomeBanca && b.BancaId != banca.BancaId).Select(b => b.NomeBanca).FirstOrDefault();
                    if (existe == null)
                    {
                        var jornaleiro = _context.Jornaleiros.Where(j => j.CPF == banca.CPF).Select(j => j).FirstOrDefault();
                        banca.Jornaleiro = jornaleiro;
                        _context.Update(banca);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");
                        List<String> Bairros3 = BairroUtil.GetBairros();
                        ViewBag.bairros = Bairros3.OrderBy(b => b).ToList();
                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }
                    
                }
                catch (Exception ex)
                {
                    ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");
                    List<String> Bairros2 = BairroUtil.GetBairros();
                    ViewBag.bairros = Bairros2.OrderBy(b => b).ToList();
                    TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro");

            }
            List<String> Bairros = BairroUtil.GetBairros();
            ViewBag.bairros = Bairros.OrderBy(b => b).ToList();
            ViewData["CPF"] = new SelectList(_context.Jornaleiros, "CPF", "NomeJornaleiro", banca.CPF);
            return View(banca);
        }

      
        // POST: Bancas/Delete/5
        [HttpPost]
        public async Task<String> Delete(int id)
        {
            var temCompra = _context.Vendas.Where(v => v.BancaId == id).ToList();

            if (temCompra.Count() > 0)
            {
               
                return "erro";

            }
            else
            {
                var banca = await _context.Bancas.FindAsync(id);
                _context.Bancas.Remove(banca);
                await _context.SaveChangesAsync();
                return "ok";
            }
           
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("BancaId;Banca;Bairro;Jornaleiro");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.Jornaleiro.NomeJornaleiro);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

                byte[] tempBytes2;
                tempBytes2 = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.Bairro);
                string asciiStr2 = System.Text.Encoding.UTF8.GetString(tempBytes2);
                arquivo.AppendLine(item.BancaId + ";" + item.NomeBanca + ";" + asciiStr2 + ";" + asciiStr);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "bancas.csv");
        }

        private bool BancaExists(int id)
        {
            return _context.Bancas.Any(e => e.BancaId == id);
        }
    }
}
