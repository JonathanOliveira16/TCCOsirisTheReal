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
    public class ClientesController : Controller
    {
        private readonly Contexto _context;

        public ClientesController(Contexto context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Clientes.Include(j => j.Status).AsNoTracking().OrderBy(j => j.NomeCliente);
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
                    var query = _context.Clientes.Include(j => j.Status).AsNoTracking().OrderBy(j => j.NomeCliente);
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View(model);
                }
                else
                {
                    List<Cliente> listadeClientes = new List<Cliente>();
                    var clientes = _context.Clientes.Include(j => j.Status).Where(b => b.NomeCliente.Contains(busca)).OrderBy(b => b.NomeCliente);
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

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.CPFcliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPFcliente,NomeCliente,EmailCliente,TelefoneCliente,StatusId")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existeCliente = _context.Clientes.Where(c => c.NomeCliente == cliente.NomeCliente).Select(c => c.NomeCliente).FirstOrDefault();
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
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("CPFcliente,NomeCliente,EmailCliente,TelefoneCliente,StatusId")] Cliente cliente)
        {
            if (id != cliente.CPFcliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existeCliente = _context.Clientes.Where(c => c.NomeCliente == cliente.NomeCliente).Select(c => c.NomeCliente).FirstOrDefault();

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
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.CPFcliente == id);
        }
    }
}
