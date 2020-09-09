using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private static String emailParaReset;
        private static long cpfUser;
        private static int deuErro;
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
            ViewBag.admin = Request.Cookies["admin"];

            var query = _context.Jornaleiros.Include(j => j.Status).AsNoTracking().Where(j=>j.StatusId == 1).OrderBy(j => j.NomeJornaleiro);
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
        public string ValidateCpf(long id)
        {
            if (id.ToString().Length < 11)
            {
                TempData["msgSucesso"] = "Tamanho de CPF inválido!";
                return "nada";
            }
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
            ViewBag.admin = Request.Cookies["admin"];
            var idUserLogado = Request.Cookies["idDoUser"];
            var eadmin = Request.Cookies["admin"];
            if (id.ToString() == idUserLogado || eadmin.ToLower() == "true")
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

                        var emailOrName = _context.Jornaleiros.Where(j => (j.EmailJornaleiro == jornaleiro.EmailJornaleiro || j.NomeJornaleiro == jornaleiro.NomeJornaleiro && j.StatusId == 1) && j.CPF != jornaleiro.CPF).Select(j => j.EmailJornaleiro).FirstOrDefault();
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

        private bool JornaleiroExists(long id)
        {
            return _context.Jornaleiros.Any(e => e.CPF == id);
        }
    }
}
