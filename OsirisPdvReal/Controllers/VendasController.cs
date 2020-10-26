using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class VendasController : Controller
    {
        private readonly Contexto _context;
        private static List<Venda> ListaParaCsv = new List<Venda>();
        public VendasController(Contexto context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index(int page= 1)
        {

            var query = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v=>v.Jornaleiros).OrderByDescending(v=>v.DataVenda);
            ListaParaCsv.Clear();
            ListaParaCsv = query.ToList();
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string tipo, DateTime busca, string buscaCliente, string buscaBanca,int page=1)
        {
            switch (tipo)
            {
                case "date":
                    var model = await SearchByDate(busca);
                    return View(model);
                case "client":
                    var modelClient = await SearchByCliente(buscaCliente);
                    return View(modelClient);
                case "banca":
                    var modelBanca = await SearchByBanca(buscaBanca);
                    return View(modelBanca);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SearchProduto(string busca, int page = 1)
        {
            try
            {
                if (busca == null)
                {
                    String cpf = Request.Cookies["idDoUser"];

                    var prods = _context.Produto.Include(p => p.tipoProduto).Where(p=>p.QuantideProduto >= 1).AsNoTracking().OrderBy(c => c.NomeProduto);
                    ViewBag.produtos = prods;
                    ViewBag.Tipos = _context.TipoProdutos.Select(t => t.NomeTipoProduto).OrderBy(t => t);
                    ViewBag.countCli = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().Count();
                    ViewBag.countProd = prods.Count();
                    ViewBag.banca = _context.Bancas.Where(c => c.CPF == Convert.ToInt64(cpf)).AsNoTracking().OrderBy(c => c.NomeBanca);
                    ViewBag.clientes = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().OrderBy(c => c.NomeCliente);
                    return View("Create");
                }
                else
                {
                    String cpf = Request.Cookies["idDoUser"];

                    var prods = _context.Produto.Include(p => p.tipoProduto).Where(b => b.NomeProduto.Contains(busca) && b.QuantideProduto>=1).OrderBy(b => b.NomeProduto);
                    ViewBag.produtos = prods;
                    ViewBag.Tipos = _context.TipoProdutos.Select(t => t.NomeTipoProduto).OrderBy(t => t);
                    ViewBag.countCli = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().Count();
                    ViewBag.countProd = prods.Count();
                    ViewBag.banca = _context.Bancas.Where(c => c.CPF == Convert.ToInt64(cpf)).AsNoTracking().OrderBy(c => c.NomeBanca);

                    ViewBag.clientes = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().OrderBy(c => c.NomeCliente);
                    return View("Create");
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }


        }

        [HttpPost]
        public async Task<IActionResult> SearchTipo(string buscaTipo, int page = 1)
        {
            try
            {
                if (buscaTipo == null)
                {
                    String cpf = Request.Cookies["idDoUser"];

                    var prods = _context.Produto.Include(p => p.tipoProduto).Where(p=>p.QuantideProduto>=1).AsNoTracking().OrderBy(c => c.NomeProduto);
                    ViewBag.produtos = prods;
                    ViewBag.banca = _context.Bancas.Where(c => c.CPF == Convert.ToInt64(cpf)).AsNoTracking().OrderBy(c => c.NomeBanca);

                    ViewBag.Tipos = _context.TipoProdutos.Select(t => t.NomeTipoProduto).OrderBy(t => t);
                    ViewBag.countCli = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().Count();
                    ViewBag.countProd = prods.Count();
                    ViewBag.clientes = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().OrderBy(c => c.NomeCliente);
                    return View("Create");
                }
                else
                {
                    String cpf = Request.Cookies["idDoUser"];

                    var prods = _context.Produto.Include(p => p.tipoProduto).Where(b => b.tipoProduto.NomeTipoProduto.Contains(buscaTipo) && b.QuantideProduto>=1).OrderBy(b => b.NomeProduto);
                    ViewBag.produtos = prods;
                    ViewBag.banca = _context.Bancas.Where(c => c.CPF == Convert.ToInt64(cpf)).AsNoTracking().OrderBy(c => c.NomeBanca);

                    ViewBag.Tipos = _context.TipoProdutos.Select(t => t.NomeTipoProduto).OrderBy(t => t);
                    ViewBag.countCli = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().Count();
                    ViewBag.countProd = prods.Count();
                    ViewBag.clientes = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().OrderBy(c => c.NomeCliente);
                    return View("Create");
                }
            }
            catch (Exception)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return View();
            }


        }

        [HttpPost]
        public async Task<PagingList<Venda>> SearchByDate(DateTime busca, int page = 1)
        {
            if (busca == DateTime.MinValue)
            {
                var query2 = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).OrderBy(v => v.DataVenda);
                ListaParaCsv.Clear();
                ListaParaCsv = query2.ToList();
                var model2 = await PagingList.CreateAsync(query2, 5, page);
                return model2;
            }
            var query = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).Where(c => c.DataVenda.Year == busca.Year && c.DataVenda.Month == busca.Month && c.DataVenda.Day == busca.Day ).OrderBy(v => v.DataVenda);
            ListaParaCsv.Clear();
            ListaParaCsv = query.ToList();
            var model = await PagingList.CreateAsync(query, 100, page);
            return model;
        }

        [HttpPost]
        public async Task<PagingList<Venda>> SearchByCliente(String buscaCliente, int page = 1)
        {
            if (buscaCliente == null)
            {
                var query2 = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).OrderBy(v => v.DataVenda);
                ListaParaCsv.Clear();
                ListaParaCsv = query2.ToList();
                var model2 = await PagingList.CreateAsync(query2, 5, page);
                return model2;
            }
            else
            {
                var query = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).Where(c => c.Clientes.NomeCliente.ToLower() == buscaCliente.ToLower()).OrderBy(v => v.DataVenda);
                ListaParaCsv.Clear();
                ListaParaCsv = query.ToList();
                var model = await PagingList.CreateAsync(query, 100, page);
                return model;
            }
         
        }

        [HttpPost]
        public async Task<PagingList<Venda>> SearchByBanca(String buscaBanca, int page = 1)
        {
            if (buscaBanca == null)
            {
                var query2 = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).OrderBy(v => v.DataVenda);
                ListaParaCsv.Clear();
                ListaParaCsv = query2.ToList();
                var model2 = await PagingList.CreateAsync(query2, 5, page);
                return model2;
            }
            else
            {
                var query = _context.Vendas.Include(v => v.Bancas).Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Jornaleiros).Where(c => c.Bancas.NomeBanca.ToLower() == buscaBanca.ToLower()).OrderBy(v => v.DataVenda);
                ListaParaCsv.Clear();
                ListaParaCsv = query.ToList();
                var model = await PagingList.CreateAsync(query, 100, page);
                return model;
            }

        }


        [HttpPost]
        public async Task<IActionResult> SalvarVenda(String[] dados, string valTotal)
        {
            string[] x = dados;
            int intBanca = 0;
            bool NotErro = false;
            int quantidadeTotalVendida = 0;
            int idVenda = 0;
            List<VendaProduto> listvp = new List<VendaProduto>();
            long intCliente = 0;
            Random rnd = new Random();
            //List<long> generatedNumbers = new List<long>();
            try
            {
                List<int?> generatedNumbers = _context.Vendas.Select(x => x.VendaId).ToList();
                do
                {
                    int rndsize = rnd.Next(1, 2000);
                    if (generatedNumbers.Contains(rndsize))
                    {
                        NotErro = false;
                    }
                    else
                    {
                        NotErro = true;
                        idVenda = rndsize;
                    }
                } while (NotErro == false);


                List<Produto> listaProduto = new List<Produto>();

                for (int i = 5; i < dados.Length; i += 5)
                {
                    string[] y = new string[5];
                    y[0] = dados[i];  //nome item
                    y[1] = dados[i + 1]; //valor
                    y[2] = dados[i + 2]; //cliente
                    y[3] = dados[i + 3]; //quantidade
                    y[4] = dados[i + 4]; //nome banca
                    var prod = _context.Produto.Where(p => p.NomeProduto == y[0]).Select(p => p).FirstOrDefault();
                    prod.QuantideProduto = prod.QuantideProduto - Convert.ToInt32(y[3]);
                    _context.Update(prod);
                    await _context.SaveChangesAsync();
                    VendaProduto vp = new VendaProduto();
                    vp.VendaId = idVenda;
                    vp.ProdutoId = prod.ProdutoId;
                    vp.QuantidadeVendida = Convert.ToInt32(y[3]);
                    vp.ValorTotalDoProduto = Convert.ToDouble(y[1].Substring(2).Replace('.',',')); 
                    listvp.Add(vp);
                    listaProduto.Add(prod);
                    
                    intBanca = _context.Bancas.Where(b => b.NomeBanca == y[4]).Select(b => b.BancaId).FirstOrDefault();
                    intCliente = _context.Clientes.Where(c => c.NomeCliente == y[2]).Select(c => c.CPFcliente).FirstOrDefault();
                    quantidadeTotalVendida = quantidadeTotalVendida + Convert.ToInt32(y[3]);
                    //_context.Add(venda);
                    //await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                }
                String cpf = Request.Cookies["idDoUser"];
                String prodD = "";
                foreach(var z in listaProduto)
                {
                    prodD = prodD + "," + z.NomeProduto;
                }
                var jorna = _context.Jornaleiros.Where(j => j.CPF == Convert.ToInt64(cpf)).FirstOrDefault();
                Venda venda = new Venda();
                venda.VendaId = idVenda;
                venda.ItemVenda = listaProduto;
                venda.BancaId = intBanca;
                venda.CPFvJ = Convert.ToInt64(cpf);
                venda.CPFcliente = intCliente;
                venda.DataVenda = DateTime.Now.Date;
                venda.StatusId = 1;
                venda.ProdutosSalvos = prodD.Substring(1); ;
                venda.ValorVenda = "R$"+ valTotal;
                venda.Jornaleiros = jorna;
                venda.QuantidadeVendida = quantidadeTotalVendida;
                _context.Add(venda);
                await _context.SaveChangesAsync();

                foreach(var vendaProd in listvp)
                {
                    _context.Add(vendaProd);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";

            }

            return RedirectToAction("Index");
        }


        public JsonResult ShowInfo(int id)
        {
            var vp = _context.VendaProduto.Where(v => v.VendaId == id).ToList();
            List<JsonVendaProduto> listaJson = new List<JsonVendaProduto>();
            foreach(var vendaProd in vp)
            {
                var produto = _context.Produto.Where(p => p.ProdutoId == vendaProd.ProdutoId).Select(p => p.NomeProduto).FirstOrDefault();
                JsonVendaProduto data = new JsonVendaProduto
                {
                    NomeProduto = produto ,
                    QuantidadeProduto = vendaProd.QuantidadeVendida
                };
                listaJson.Add(data);
            }
           
            return Json(listaJson);
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Bancas)
                .Include(v => v.Clientes)
                .Include(v => v.Status)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create(int page = 1)
        {
            String cpf = Request.Cookies["idDoUser"];
            ViewBag.countCli = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().Count();
            ViewBag.countProd = _context.Produto.AsNoTracking().Where(p=>p.QuantideProduto>=1).Count();
            ViewBag.clientes = _context.Clientes.Where(c => c.StatusId == 1).AsNoTracking().OrderBy(c => c.NomeCliente);
            ViewBag.banca = _context.Bancas.Where(c => c.CPF == Convert.ToInt64(cpf)).AsNoTracking().OrderBy(c => c.NomeBanca);
            ViewBag.produtos = _context.Produto.AsNoTracking().Where(p=>p.QuantideProduto>=1).OrderBy(c => c.NomeProduto);
            ViewBag.Tipos = _context.TipoProdutos.Select(t => t.NomeTipoProduto).OrderBy(t => t);
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca");
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,DataVenda,ValorVenda,StatusId,BancaId,CPFcliente")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("VendaId,DataVenda,ValorVenda,StatusId,BancaId,CPFcliente")] Venda venda)
        {
            if (id != venda.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancaId"] = new SelectList(_context.Bancas, "BancaId", "NomeBanca", venda.BancaId);
            ViewData["CPFcliente"] = new SelectList(_context.Clientes, "CPFcliente", "EmailCliente", venda.CPFcliente);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "NomeStatus", venda.StatusId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var venda = await _context.Vendas
        //        .Include(v => v.Bancas)
        //        .Include(v => v.Clientes)
        //        .Include(v => v.Status)
        //        .FirstOrDefaultAsync(m => m.VendaId == id);
        //    if (venda == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(venda);
        //}

        // POST: Vendas/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            var vendaComProdutoSeparado = _context.VendaProduto.Where(v => v.VendaId == id).ToList();
            foreach(var x in vendaComProdutoSeparado)
            {
                var prod = _context.Produto.Where(p => p.ProdutoId == x.ProdutoId).FirstOrDefault();
                prod.QuantideProduto = x.QuantidadeVendida + prod.QuantideProduto;
            }
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GerarCSV()
        {
            var registros = ListaParaCsv;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("VendaId;Data;Valor;Quantidade total;Produtos;Banca;Cliente;Jornaleiro");

            foreach (var item in registros)
            {
                
             
                arquivo.AppendLine(item.VendaId + ";" + item.DataVenda.ToShortDateString() + ";" + item.ValorVenda + ";" + item.QuantidadeVendida + ";" + item.ProdutosSalvos + ";" +item.Bancas.NomeBanca + ";" + item.Clientes.NomeCliente + ";" + item.Jornaleiros.NomeJornaleiro);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "vendas.csv");
        }

        private bool VendaExists(int? id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
    }
}
