﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caelum.Stella.CSharp.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using OsirisPdvReal.Utils;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class JornaleirosController : Controller
    {
        private readonly Contexto _context;
        private static String codeEmail;
        private static long CPFparaReset;
        private static long cpfUser;
        private static int deuErro;
        private static List<Jornaleiro> ListaParaCsv = new List<Jornaleiro>();

        public IActionResult Login()
        {
            Response.Cookies.Delete("admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("CPF,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro")] Jornaleiro jornaleiro)
        {
            var jornaleiroOk = _context.Jornaleiros.Include(j=>j.tipo).Where(j => j.EmailJornaleiro.ToLower() == jornaleiro.EmailJornaleiro.ToLower() && j.SenhaJornaleiro == jornaleiro.SenhaJornaleiro && j.StatusId == 1).Select(j => j).FirstOrDefault();
            String variavelDif = "";
            if (jornaleiroOk == null)
            {
                variavelDif = "a";
            }
            if (variavelDif != "a")
            {
                if (jornaleiroOk.tipo.NomeTipo.ToLower() == "admin")
                {
                    var option2 = new CookieOptions();
                    Response.Cookies.Append("admin", "admin", option2);

                }
                else
                {
                    var option2 = new CookieOptions();
                    Response.Cookies.Append("admin", "false", option2);
                }
                var option = new CookieOptions();
                Response.Cookies.Append("idDoUser", jornaleiroOk.CPF.ToString(), option);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["alertColor"] = "alert-danger";
                TempData["msgSucesso"] = "E-mail ou senha inválida / Usuário inativo";
                return RedirectToAction("Login");
            }
        }



        public IActionResult EnviarTokenSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarTokenSenha([Bind("CPF,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro,TelefoneJornaleiro")] Jornaleiro jornaleiro)
        {
            try
            {
                var jornaleiroExists = _context.Jornaleiros.Where(j => j.CPF == jornaleiro.CPF).Select(j => j).FirstOrDefault();
                if (jornaleiroExists != null)
                {
                    if (jornaleiroExists.TelefoneJornaleiro.Substring(4, 5) == jornaleiro.TelefoneJornaleiro && jornaleiroExists.CPF == jornaleiro.CPF)
                    {
                        CPFparaReset = Convert.ToInt64(jornaleiro.CPF);
                        return RedirectToAction("ResetSenha");
                    }
                    else {
                        TempData["msgSucesso"] = "Os dados enviados não coincidem com os dados cadastrados";
                        return RedirectToAction("EnviarTokenSenha");
                    }
                }
                else
                {
                    TempData["msgSucesso"] = "CPF não encontrado em nossa base de dados";
                    return RedirectToAction("EnviarTokenSenha");
                }
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!" + ex;
                return RedirectToAction("EnviarTokenSenha");
            }
        }


        public IActionResult ValidarCodigo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidarCodigo(string codeInput)
        {
            try
            {
                if (codeInput == codeEmail)
                {
                    return RedirectToAction("ResetSenha");
                }
                else
                {
                    TempData["msgSucesso"] = "Código inválido, favor tentar novamente!";
                    return RedirectToAction("EnviarTokenSenha");
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return RedirectToAction("EnviarTokenSenha");
            }

        }

        public IActionResult ResetSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetSenha(string senhaNova)
        {
            try
            {
                var jornaleiroUser = _context.Jornaleiros.Where(j => j.CPF == CPFparaReset).FirstOrDefault();
                jornaleiroUser.SenhaJornaleiro = senhaNova;
                _context.Update(jornaleiroUser);
                _context.SaveChanges();
                TempData["msgSucesso"] = "Senha alterada com sucesso!";
                TempData["alertColor"] = "alert-success";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return RedirectToAction("EnviarTokenSenha");
            }

        }

        public JornaleirosController(Contexto context)
        {
            _context = context;
        }

        // GET: Jornaleiros
        public async Task<IActionResult> Index(int page = 1)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            //essas linhas sao necessarias para a paginaca so trocar o tipo de context, jornaleiro, produto etc
            ViewBag.admin = Request.Cookies["admin"];

            var query = _context.Jornaleiros.Include(j => j.Status).Include(j=>j.tipo).AsNoTracking().Where(j=>j.StatusId == 1).OrderBy(j => j.NomeJornaleiro);
            ListaParaCsv.Clear();
            ListaParaCsv = query.ToList();
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string busca, int page = 1)
        {
            if(busca == null)
            {
                var query = _context.Jornaleiros.Include(j => j.Status).Include(j => j.tipo).AsNoTracking().Where(j => j.StatusId == 1).OrderBy(j => j.NomeJornaleiro);
                ListaParaCsv.Clear();
                ListaParaCsv = query.ToList();
                var model = await PagingList.CreateAsync(query, 5, page);
                return View(model);
            }
            else
            {
                var query = _context.Jornaleiros.Include(j => j.Status).Include(j => j.tipo).AsNoTracking().Where(j => j.StatusId == 1 && j.NomeJornaleiro.ToLower().Contains(busca.ToLower())).OrderBy(j => j.NomeJornaleiro);
                ListaParaCsv.Clear();
                ListaParaCsv = query.ToList();
                var model = await PagingList.CreateAsync(query, 5, page);
                return View(model);
            }
        }


        // GET: Jornaleiros/Create
        public IActionResult Create()
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            ViewData["disable"] = "readonly";
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");
            return View();
        }

        [HttpPost]
        public string ValidateCpf(string id)
        {
            if (id.ToString().Length < 11)
            {
                TempData["msgSucesso"] = "Tamanho de CPF inválido!";
                return "nada";
            }
            try
            {
                new CPFValidator().AssertValid(id.ToString());
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "CPF inválido!";
                return "nada";
            }
            var cpfExist = _context.Jornaleiros.Where(j => j.CPF == Convert.ToInt64(id)).Select(j => j.NomeJornaleiro).FirstOrDefault();
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
        // POST: Jornaleiros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPF,NomeJornaleiro,EmailJornaleiro,TelefoneJornaleiro,SenhaJornaleiro,StatusId, TipoId")] Jornaleiro jornaleiro)
        {
            ModelState.Remove("CPF");
            try
            {
                if (ModelState.IsValid)
                {
                    var emailOrName = _context.Jornaleiros.Where(j => j.EmailJornaleiro == jornaleiro.EmailJornaleiro || j.NomeJornaleiro == jornaleiro.NomeJornaleiro && j.StatusId == 1).Select(j => j.EmailJornaleiro).FirstOrDefault();
                    if (emailOrName == null)
                    {
                        jornaleiro.CPF = cpfUser;
                        _context.Add(jornaleiro);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msgSucesso"] = "E-mail ou nome de jornaleiro já cadastrado!";

                    }

                }
                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");

                return View(jornaleiro);
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");

                return View(jornaleiro);
            }

        }

        // GET: Jornaleiros/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (Request.Cookies["idDoUser"] == null)
            {
                return RedirectToAction("Login", "Jornaleiros");
            }
            ViewBag.admin = Request.Cookies["admin"];
            var idUserLogado = Request.Cookies["idDoUser"];
            var eadmin = Request.Cookies["admin"];
            if (id.ToString() == idUserLogado || eadmin.ToLower() == "admin")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var jornaleiro = await _context.Jornaleiros.FindAsync(id);
                if (jornaleiro == null)
                {
                    return NotFound();
                }
                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");

                return View(jornaleiro);
            }
            else if (id.ToString() != idUserLogado && eadmin.ToLower() != "true")
            {
                TempData["msgSucesso"] = "Não é possível editar dados de outros usuários!";
                return RedirectToAction("Index");
            }
            return View();
         
        }

        // POST: Jornaleiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CPF,NomeJornaleiro,EmailJornaleiro,TelefoneJornaleiro,SenhaJornaleiro,StatusId,TipoId")] Jornaleiro jornaleiro)
        {
            ModelState.Remove("CPF");
            ModelState.Remove("SenhaJornaleiro");
            try
            {
                if (id != jornaleiro.CPF)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {

                        var emailOrName = _context.Jornaleiros.Where(j => (j.EmailJornaleiro == jornaleiro.EmailJornaleiro || j.NomeJornaleiro == jornaleiro.NomeJornaleiro && j.StatusId == 1 && j.CPF != jornaleiro.CPF) && j.CPF != jornaleiro.CPF).Select(j => j.EmailJornaleiro).FirstOrDefault();
                        if (emailOrName == null)
                        {
                            jornaleiro.SenhaJornaleiro = _context.Jornaleiros.Where(j => j.CPF == id).Select(j => j.SenhaJornaleiro).FirstOrDefault();
                            _context.Update(jornaleiro);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            TempData["msgSucesso"] = "E-mail ou nome de jornaleiro já cadastrado!";
                            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");

                            return View(jornaleiro);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JornaleiroExists(jornaleiro.CPF))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (jornaleiro.TipoId == 1)
                    {
                        var option2 = new CookieOptions();
                        Response.Cookies.Delete("admin");
                        Response.Cookies.Append("admin", "admin", option2);

                    }
                    else
                    {
                        var option2 = new CookieOptions();
                        Response.Cookies.Delete("admin");
                        Response.Cookies.Append("admin", "false", option2);
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";

                return View(jornaleiro);
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";

                ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
                ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");

                return View(jornaleiro);
            }

        }

        // GET: Jornaleiros/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var jornaleiro = await _context.Jornaleiros
        //        .Include(j => j.Status)
        //        .FirstOrDefaultAsync(m => m.CPF == id);
        //    if (jornaleiro == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(jornaleiro);
        //}

        // POST: Jornaleiros/Delete/5
        [HttpPost]
        public async Task<String> Delete(long id)
        {
            var temBanca = _context.Bancas.Include(j => j.Jornaleiro).Where(j => j.Jornaleiro.CPF == id).Select(b => b.NomeBanca).FirstOrDefault();
            if (temBanca != null)
            {
                
                return "erro";
            }
            var jornaleiro = await _context.Jornaleiros.FindAsync(id);
            jornaleiro.StatusId = 2;
            _context.Update(jornaleiro);
            
            //_context.Jornaleiros.Remove(jornaleiro);
            await _context.SaveChangesAsync();
            return "ok";
        }

        public IActionResult ViewLoginCliente(String error)
        {
            if (error != null)
            {
                TempData["msgSucesso"] = "Chave de acesso incorreta!";
                error = "";
            }
            return View();
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("CPF;Nome;E-mail;Telefone;Permissao;Status");

            foreach (var item in registros)
            {


                arquivo.AppendLine(item.CPF + ";" + item.NomeJornaleiro + ";" + item.EmailJornaleiro + ";" + item.TelefoneJornaleiro + ";" + item.tipo.NomeTipo + ";" + item.Status.NomeStatus);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "jornaleiros.csv");
        }

        private bool JornaleiroExists(long id)
        {
            return _context.Jornaleiros.Any(e => e.CPF == id);
        }
    }
}
