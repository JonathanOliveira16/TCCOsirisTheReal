using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private static String emailParaReset;
        private static int cpfUser;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("CPF,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro")] Jornaleiro jornaleiro)
        {
            var jornaleiroOk = _context.Jornaleiros.Where(j => j.EmailJornaleiro == jornaleiro.EmailJornaleiro && j.SenhaJornaleiro == jornaleiro.SenhaJornaleiro).Select(j => j.EmailJornaleiro).FirstOrDefault();
            if (jornaleiroOk != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["alertColor"] = "alert-danger";
                TempData["msgSucesso"] = "E-mail ou senha inválida";
                return RedirectToAction("Login");
            }
        }



        public IActionResult EnviarTokenSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarTokenSenha([Bind("CPF,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro")] Jornaleiro jornaleiro)
        {
            try
            {
                var jornaleiroExists = _context.Jornaleiros.Where(j => j.EmailJornaleiro == jornaleiro.EmailJornaleiro).Select(j => j.EmailJornaleiro).FirstOrDefault();
                if (jornaleiroExists != null)
                {
                    var code = RandomHelper.RandomString(6);
                    EmailHelper.SendEmail(jornaleiroExists, code);
                    emailParaReset = jornaleiroExists;
                    codeEmail = code;
                    return RedirectToAction("ValidarCodigo");
                }
                else
                {
                    TempData["msgSucesso"] = "E-mail não encontrado em nossa base de dados";
                    return RedirectToAction("EnviarTokenSenha");
                }
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
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
                var jornaleiroUser = _context.Jornaleiros.Where(j => j.EmailJornaleiro == emailParaReset).FirstOrDefault();
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
            //essas linhas sao necessarias para a paginaca so trocar o tipo de context, jornaleiro, produto etc
            var query = _context.Jornaleiros.Include(j => j.Status).AsNoTracking().OrderBy(j => j.NomeJornaleiro);
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        // GET: Jornaleiros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornaleiro = await _context.Jornaleiros
                .Include(j => j.Status)
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (jornaleiro == null)
            {
                return NotFound();
            }

            return View(jornaleiro);
        }

        // GET: Jornaleiros/Create
        public IActionResult Create()
        {

            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            ViewData["disable"] = "readonly";
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "NomeTipo");
            return View();
        }

        [HttpPost]
        public string ValidateCpf(int id)
        {
            var cpfExist = _context.Jornaleiros.Where(j => j.CPF == id).Select(j => j.NomeJornaleiro).FirstOrDefault();
            if (cpfExist == null)
            {
                cpfUser = id;
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
                    var emailOrName = _context.Jornaleiros.Where(j => j.EmailJornaleiro == jornaleiro.EmailJornaleiro || j.NomeJornaleiro == jornaleiro.NomeJornaleiro).Select(j => j.EmailJornaleiro).FirstOrDefault();
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
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Jornaleiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CPF,NomeJornaleiro,EmailJornaleiro,TelefoneJornaleiro,SenhaJornaleiro,StatusId,TipoId")] Jornaleiro jornaleiro)
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

                        var emailOrName = _context.Jornaleiros.Where(j => (j.EmailJornaleiro == jornaleiro.EmailJornaleiro || j.NomeJornaleiro == jornaleiro.NomeJornaleiro) && j.CPF != jornaleiro.CPF).Select(j => j.EmailJornaleiro).FirstOrDefault();
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
        public async Task<IActionResult> Delete(int id)
        {
            var jornaleiro = await _context.Jornaleiros.FindAsync(id);
            _context.Jornaleiros.Remove(jornaleiro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JornaleiroExists(int id)
        {
            return _context.Jornaleiros.Any(e => e.CPF == id);
        }
    }
}
