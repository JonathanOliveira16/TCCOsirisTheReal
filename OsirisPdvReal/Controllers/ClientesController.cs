using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caelum.Stella.CSharp.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using OsirisPdvReal.Utils;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class ClientesController : Controller
    {
        private readonly Contexto _context;
        private static long cpfUser;
        private static List<Cliente> ListaParaCsv = new List<Cliente>();

        public ClientesController(Contexto context)
        {
            _context = context;
        }


        [HttpPost]
        public string ValidateCpf(string id)
        {
            if (id.Length < 11)
            {
                TempData["msgSucesso"] = "Tamanho de CPF inválido!";
                return "nada";
            }
            try
            {
                new CPFValidator().AssertValid(id);
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "CPF inválido!";
                return "nada";
            }
            var cpfExist = _context.Clientes.Where(j => j.CPFcliente == Convert.ToInt64(id)).Select(j => j.NomeCliente).FirstOrDefault();
            if (cpfExist == null)
            {
                cpfUser = Convert.ToInt64(id);
                return "ok";
            }
            else
            {
                return "editar";
            }
        }

        // GET: Clientes
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            var query = _context.Clientes.Include(j => j.Status).AsNoTracking().Where(c=>c.StatusId ==1).OrderBy(j => j.NomeCliente);
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
                    var query = _context.Clientes.Include(j => j.Status).AsNoTracking().Where(c => c.StatusId == 1).OrderBy(j => j.NomeCliente);
                    ListaParaCsv.Clear();
                    ListaParaCsv = query.ToList();
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View(model);
                }
                else
                {
                    List<Cliente> listadeClientes = new List<Cliente>();
                    var clientes = _context.Clientes.Include(j => j.Status).Where(b => b.NomeCliente.Contains(busca) && b.StatusId==1).OrderBy(b => b.NomeCliente);
                    ListaParaCsv.Clear();
                    ListaParaCsv = clientes.ToList();
                    var model = await PagingList.CreateAsync(clientes, 5, page);
                    return View(model);
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }


        }


        // GET: Clientes/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPFcliente,NomeCliente,EmailCliente,TelefoneCliente,CEPcliente,StatusId")] Cliente cliente)
        {
            cliente.CPFcliente = cpfUser;
            try
            {
                if (ModelState.IsValid)
                {
                    var existeCliente = _context.Clientes.Where(c => c.NomeCliente == cliente.NomeCliente && c.StatusId == 1).Select(c => c.NomeCliente).FirstOrDefault();
                    if (existeCliente == null)
                    {
                        _context.Add(cliente);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }

                }
            }
            catch (Exception ex)
            {

                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }
           
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", cliente.StatusId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
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

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", cliente.StatusId);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CPFcliente,NomeCliente,EmailCliente,TelefoneCliente,CEPcliente,StatusId")] Cliente cliente)
        {
            if (id != cliente.CPFcliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existeCliente = _context.Clientes.Where(c => c.NomeCliente == cliente.NomeCliente && c.StatusId == 1 && c.CPFcliente != cliente.CPFcliente).Select(c => c.NomeCliente).FirstOrDefault();

                try
                {
                    if (existeCliente == null)
                    { 
                        _context.Update(cliente);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

                        TempData["msgSucesso"] = "Nome já existente em nosso banco de dados!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

                    TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", cliente.StatusId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cliente = await _context.Clientes
        //        .Include(c => c.Status)
        //        .FirstOrDefaultAsync(m => m.CPFcliente == id);
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cliente);
        //}

        // POST: Clientes/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            cliente.StatusId = 2;
            _context.Update(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("CPF;Nome;Email;Telefone");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.NomeCliente);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
                arquivo.AppendLine(item.CPFcliente + ";" + asciiStr + ";" + item.EmailCliente + ";" + item.TelefoneCliente);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "clientes.csv");
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.CPFcliente == id);
        }
    }
}
