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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("JornaleiroId,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro")] Jornaleiro jornaleiro)
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
        public IActionResult EnviarTokenSenha([Bind("JornaleiroId,NomeJornaleiro,EmailJornaleiro,SenhaJornaleiro")] Jornaleiro jornaleiro)
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
            var query = _context.Jornaleiros.AsNoTracking().OrderBy(j => j.NomeJornaleiro);
            var model = await PagingList.CreateAsync(query,5,page);
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
                .FirstOrDefaultAsync(m => m.JornaleiroId == id);
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
            return View();
        }

        // POST: Jornaleiros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JornaleiroId,NomeJornaleiro,EmailJornaleiro,TelefoneJornaleiro,SenhaJornaleiro,StatusId")] Jornaleiro jornaleiro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jornaleiro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", jornaleiro.StatusId);
            return View(jornaleiro);
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
            return View(jornaleiro);
        }

        // POST: Jornaleiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JornaleiroId,NomeJornaleiro,EmailJornaleiro,TelefoneJornaleiro,SenhaJornaleiro,StatusId")] Jornaleiro jornaleiro)
        {
            if (id != jornaleiro.JornaleiroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jornaleiro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JornaleiroExists(jornaleiro.JornaleiroId))
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
            return View(jornaleiro);
        }

        // GET: Jornaleiros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornaleiro = await _context.Jornaleiros
                .Include(j => j.Status)
                .FirstOrDefaultAsync(m => m.JornaleiroId == id);
            if (jornaleiro == null)
            {
                return NotFound();
            }

            return View(jornaleiro);
        }

        // POST: Jornaleiros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jornaleiro = await _context.Jornaleiros.FindAsync(id);
            _context.Jornaleiros.Remove(jornaleiro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JornaleiroExists(int id)
        {
            return _context.Jornaleiros.Any(e => e.JornaleiroId == id);
        }
    }
}
