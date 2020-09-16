using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;

namespace OsirisPdvReal.Controllers
{
    public class StatusController : Controller
    {
        private readonly Contexto _context;

        public StatusController(Contexto context)
        {
            _context = context;
        }

        // GET: Status
        public async Task<IActionResult> Index()
        {
            return View(await _context.Status.ToListAsync());
        }

        public async Task<IActionResult> InativosJornaleiros()
        {

            return View(await _context.Jornaleiros.Include(j=>j.Status).Where(j=>j.StatusId == 2).ToListAsync());
        }

        public async Task<IActionResult> AtivarJornaleiro(long id)
        {
            try
            {
                var jornaleiroStatus = _context.Jornaleiros.Include(s => s.Status).Where(j => j.CPF == id).FirstOrDefault();
                jornaleiroStatus.StatusId = 1;
                _context.Update(jornaleiroStatus);
                await _context.SaveChangesAsync();
                TempData["msgSucesso"] = "Jornaleiro ativo novamente!";
                TempData["color"] = "alert-info";
                return RedirectToAction("InativosJornaleiros");
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Ocorreu um erro na sua solicitação!";
                TempData["color"] = "alert-danger";
                return RedirectToAction("InativosJornaleiros");
            }
           
        }

        public async Task<IActionResult> InativosFornecedores()
        {

            return View(await _context.Fornecedores.Include(j => j.Status).Where(j => j.StatusId == 2).ToListAsync());
        }

        public async Task<IActionResult> AtivarFornecedores(long id)
        {
            try
            {
                var fornecedorStatus = _context.Fornecedores.Include(s => s.Status).Where(j => j.CNPJ == id).FirstOrDefault();
                fornecedorStatus.StatusId = 1;
                _context.Update(fornecedorStatus);
                await _context.SaveChangesAsync();
                TempData["msgSucesso"] = "Fornecedor ativo novamente!";
                TempData["color"] = "alert-info";
                return RedirectToAction("InativosFornecedores");
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Ocorreu um erro na sua solicitação!";
                TempData["color"] = "alert-danger";
                return RedirectToAction("InativosFornecedores");
            }
        
        }

        public async Task<IActionResult> InativosClientes()
        {
            return View(await _context.Clientes.Include(j => j.Status).Where(j => j.StatusId == 2).ToListAsync());
        }

        public async Task<IActionResult> AtivarClientes(long id)
        {
            try
            {
                var clienteStatus = _context.Clientes.Include(s => s.Status).Where(j => j.CPFcliente == id).FirstOrDefault();
                clienteStatus.StatusId = 1;
                _context.Update(clienteStatus);
                await _context.SaveChangesAsync();
                TempData["msgSucesso"] = "Cliente ativo novamente!";
                TempData["color"] = "alert-info";
                return RedirectToAction("InativosClientes");
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Ocorreu um erro na sua solicitação!";
                TempData["color"] = "alert-danger";
                return RedirectToAction("InativosClientes");
            }
           
        }


        // GET: Status/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Status
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // GET: Status/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,NomeStatus")] Status status)
        {
            if (ModelState.IsValid)
            {
                var existStatus = _context.Status.Where(s => s.NomeStatus == status.NomeStatus).Select(s => s.NomeStatus).FirstOrDefault();
                if (existStatus == null)
                {
                    _context.Add(status);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msgSucesso"] = "Nome de status já existente em nosso banco de dados!";
                    return View(status);
                }
              
            }
            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Status.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("StatusId,NomeStatus")] Status status)
        {
            if (id != status.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existStatus = _context.Status.Where(s => s.NomeStatus == status.NomeStatus && s.StatusId != status.StatusId).Select(s => s.NomeStatus).FirstOrDefault();
                    if (existStatus == null)
                    {
                        _context.Update(status);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["msgSucesso"] = "Nome de status já existente em nosso banco de dados!";
                        return View(status);
                    }
                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.StatusId))
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
            return View(status);
        }

        // GET: Status/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var status = await _context.Status
        //        .FirstOrDefaultAsync(m => m.StatusId == id);
        //    if (status == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(status);
        //}

        // POST: Status/Delete/5
        [HttpPost]
        public async Task<String> Delete(int? id)
        {
            try
            {
                var status = await _context.Status.FindAsync(id);
                _context.Status.Remove(status);
                await _context.SaveChangesAsync();
                return "ok";
            }
            catch (Exception ex)
            {

                return "bad";
            }
          
        }

        private bool StatusExists(int? id)
        {
            return _context.Status.Any(e => e.StatusId == id);
        }
    }
}
