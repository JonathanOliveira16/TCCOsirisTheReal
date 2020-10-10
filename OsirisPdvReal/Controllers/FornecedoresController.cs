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
    public class FornecedoresController : Controller
    {
        private readonly Contexto _context;
        private static long CpnjFornece;
        private static List<Fornecedor> ListaParaCsv = new List<Fornecedor>();

        public FornecedoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            List<String> Estados = BairroUtil.GetEstados();
            ViewBag.estados = Estados.OrderBy(b => b).ToList();
            var query = _context.Fornecedores.Include(j => j.Status).AsNoTracking().Where(f=>f.StatusId == 1).OrderBy(j => j.NomeFornecedor);
            ListaParaCsv.Clear();
            ListaParaCsv = query.ToList();
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
                    List<String> Estados = BairroUtil.GetEstados();
                    ViewBag.estados = Estados.OrderBy(b => b).ToList();
                    var query = _context.Fornecedores.Include(j => j.Status).AsNoTracking().Where(f=>f.StatusId == 1).OrderBy(j => j.NomeFornecedor);
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View(model);
                }
                else
                {
                    List<String> Estados = BairroUtil.GetEstados();
                    ViewBag.estados = Estados.OrderBy(b => b).ToList();
                    List<Fornecedor> listaDeFornecedor = new List<Fornecedor>();
                    var fornecedor = _context.Fornecedores.Include(j => j.Status).Where(b => b.NomeFornecedor.Contains(busca) && b.StatusId == 1).OrderBy(b => b.NomeFornecedor);
                    ListaParaCsv.Clear();
                    ListaParaCsv = fornecedor.ToList();
                    var model = await PagingList.CreateAsync(fornecedor, 5, page);
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
        public async Task<IActionResult> SearchEstado(string buscaEstado, int page = 1)
        {
            try
            {
                if (buscaEstado == null)
                {
                    List<String> Estados = BairroUtil.GetEstados();
                    ViewBag.estados = Estados.OrderBy(b => b).ToList();
                    var query = _context.Fornecedores.Include(j => j.Status).AsNoTracking().Where(f => f.StatusId == 1).OrderBy(j => j.NomeFornecedor);
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View("Index",model);
                }
                else
                {
                    List<String> Estados = BairroUtil.GetEstados();
                    ViewBag.estados = Estados.OrderBy(b => b).ToList();
                    List<Fornecedor> listaDeFornecedor = new List<Fornecedor>();
                    var fornecedor = _context.Fornecedores.Include(j => j.Status).Where(b => b.EstadoFornecedor.Contains(buscaEstado) && b.StatusId == 1).OrderBy(b => b.NomeFornecedor);
                    ListaParaCsv.Clear();
                    ListaParaCsv = fornecedor.ToList();
                    var model = await PagingList.CreateAsync(fornecedor, 5, page);
                    return View("Index", model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }
        }

        [HttpPost]
        public string ValidateFornecedor(string id)
        {
          
            if (id.ToString().Length < 14)
            {
                TempData["msgSucesso"] = "Tamanho de CNPJ inválido!";
                return "nada";
            }
            var isValid = Validadores.IsCnpj(id.ToString());
            if (isValid == false)
            {
                TempData["msgSucesso"] = "CNPJ inválido!";
                return "nada";
            }
            var CNPJExist = _context.Fornecedores.Where(j => j.CNPJ == Convert.ToInt64(id)).Select(j => j.NomeFornecedor).FirstOrDefault();
            if (CNPJExist == null)
            {
                CpnjFornece = Convert.ToInt64(id);
                return "ok";
            }
            else
            {
                return "editar";
            }
        }



        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            List<String> Estados = BairroUtil.GetEstados();
            ViewBag.estados = Estados.OrderBy(b => b).ToList();
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

            return View();
        }

        // POST: Fornecedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CNPJ,NomeFornecedor,EmailFornecedor,TelefoneFornecedor,PontoFocalFornecedor,LogradouroFornecedor,EstadoFornecedor,CEPFornecedor,StatusId")] Fornecedor fornecedor)
        {
            try
            {
                fornecedor.CNPJ = CpnjFornece;
                if (ModelState.IsValid)
                {
                    var existeForne = _context.Fornecedores.Where(f => f.NomeFornecedor == fornecedor.NomeFornecedor && f.StatusId == 1).Select(b => b.NomeFornecedor).FirstOrDefault();
                    if (existeForne == null)
                    {
                        //var forneceData = _context.Fornecedores.Where(j => j.CNPJ == fornecedor.CNPJ).Select(j => j).FirstOrDefault();
                        fornecedor.CNPJ = CpnjFornece;
                        _context.Add(fornecedor);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
                        List<String> Estados2 = BairroUtil.GetEstados();
                        ViewBag.estados = Estados2.OrderBy(b => b).ToList();
                        TempData["msgSucesso"] = "Nome de fornecedor já existente em nosso banco de dados!";
                        return View();
                    }
                }
                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
                List<String> Estados = BairroUtil.GetEstados();
                ViewBag.estados = Estados.OrderBy(b => b).ToList();
                return View(fornecedor);
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Ocorreu um erro interno na aplicação!";
                return View(fornecedor);
            }

        }

        // GET: Fornecedores/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
            List<String> Estados = BairroUtil.GetEstados();
            ViewBag.estados = Estados.OrderBy(b => b).ToList();
            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("CNPJ,NomeFornecedor,EmailFornecedor,TelefoneFornecedor,PontoFocalFornecedor,LogradouroFornecedor,EstadoFornecedor,CEPFornecedor,StatusId")] Fornecedor fornecedor)
        {
            if (id != fornecedor.CNPJ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existeFornce = _context.Fornecedores.Where(f => f.NomeFornecedor == fornecedor.NomeFornecedor && f.CNPJ != fornecedor.CNPJ && f.StatusId == 1).Select(f => f.NomeFornecedor).FirstOrDefault();
                    if (existeFornce == null)
                    {
                        _context.Update(fornecedor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
                        List<String> Estados = BairroUtil.GetEstados();
                        ViewBag.estados = Estados.OrderBy(b => b).ToList();
                        TempData["msgSucesso"] = "Nome de fornecedor já existente em nosso banco de dados!";
                        return View();
                    }
                   
                }
                catch (Exception)
                {

                    ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
                    List<String> Estados = BairroUtil.GetEstados();
                    ViewBag.estados = Estados.OrderBy(b => b).ToList();
                    TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
            return View(fornecedor);
        }

        // GET: Fornecedores/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var fornecedor = await _context.Fornecedores
        //        .Include(f => f.Status)
        //        .FirstOrDefaultAsync(m => m.CNPJ == id);
        //    if (fornecedor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(fornecedor);
        //}

        // POST: Fornecedores/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(long? id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            fornecedor.StatusId = 2;
            _context.Update(fornecedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("CNPJ;Nome;E-mail;Telefone;Ponto focal;Estado;Logradouro;CEP");
           
            foreach (var item in registros)
            {
                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.EstadoFornecedor);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
                arquivo.AppendLine(item.CNPJ + ";" + item.NomeFornecedor + ";" + item.EmailFornecedor + ";" + item.TelefoneFornecedor + ";" + item.PontoFocalFornecedor + ";" + asciiStr + ";" + item.LogradouroFornecedor + ";" + item.CEPFornecedor);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "fornecedores.csv");
        }

        private bool FornecedorExists(int? id)
        {
            return _context.Fornecedores.Any(e => e.CNPJ == id);
        }
    }
}
