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
    public class FornecedoresController : Controller
    {
        private readonly Contexto _context;
        private static int CpnjFornece;

        public FornecedoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Fornecedores.Include(j => j.Status).AsNoTracking().OrderBy(j => j.NomeFornecedor);
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
                    var query = _context.Fornecedores.Include(j => j.Status).AsNoTracking().OrderBy(j => j.NomeFornecedor);
                    var model = await PagingList.CreateAsync(query, 5, page);
                    return View(model);
                }
                else
                {
                    List<Fornecedor> listaDeFornecedor = new List<Fornecedor>();
                    var fornecedor = _context.Fornecedores.Include(j => j.Status).Where(b => b.NomeFornecedor.Contains(busca)).OrderBy(b => b.NomeFornecedor);
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
        public string ValidateFornecedor(int id)
        {
            var CNPJExist = _context.Fornecedores.Where(j => j.CNPJ == id).Select(j => j.NomeFornecedor).FirstOrDefault();
            if (CNPJExist == null)
            {
                CpnjFornece = id;
                return "ok";
            }
            else
            {
                return "editar";
            }
        }

        // GET: Fornecedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .Include(f => f.Status)
                .FirstOrDefaultAsync(m => m.CNPJ == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            return View();
        }

        // POST: Fornecedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CNPJ,NomeFornecedor,EmailFornecedor,TelefoneFornecedor,PontoFocalFornecedor,LogradouroFornecedor,CEPFornecedor,StatusId")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                var existeForne = _context.Fornecedores.Where(f=>f.NomeFornecedor == fornecedor.NomeFornecedor).Select(b => b.NomeFornecedor).FirstOrDefault();
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

                    TempData["msgSucesso"] = "Nome de fornecedor já existente em nosso banco de dados!";
                    return View();
                }
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);
            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CNPJ,NomeFornecedor,EmailFornecedor,TelefoneFornecedor,PontoFocalFornecedor,LogradouroFornecedor,CEPFornecedor,StatusId")] Fornecedor fornecedor)
        {
            if (id != fornecedor.CNPJ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existeFornce = _context.Fornecedores.Where(f => f.NomeFornecedor == fornecedor.NomeFornecedor && f.CNPJ != fornecedor.CNPJ).Select(f => f.NomeFornecedor).FirstOrDefault();
                    if (existeFornce == null)
                    {
                        _context.Update(fornecedor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", fornecedor.StatusId);

                        TempData["msgSucesso"] = "Nome de fornecedor já existente em nosso banco de dados!";
                        return View();
                    }
                   
                }
                catch (Exception)
                {

                    ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");

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
        public async Task<IActionResult> Delete(int? id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int? id)
        {
            return _context.Fornecedores.Any(e => e.CNPJ == id);
        }
    }
}
