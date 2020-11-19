using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using OsirisPdvReal.Utils;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly Contexto _context;
        private static List<RankingCliente> ListaParaCsvRanking = new List<RankingCliente>();
        private static List<VendasBancaRelatorio> ListaParaCsvBanca = new List<VendasBancaRelatorio>();
        private static List<ItemMaisVende> ListaParaCsvItem = new List<ItemMaisVende>();
        private static List<Produto> ListaParaCsvProds = new List<Produto>();

        public RelatoriosController(Contexto context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RankingClientes()
        {
            String cpfAtualLogado = Request.Cookies["idDoUser"];
            if (cpfAtualLogado != null)
            {
                ViewBag.user = true;
            }
            try
            {
                List<RankingCliente> rankingClientes = new List<RankingCliente>();
                List<RankingCliente> ListaFinal = new List<RankingCliente>();
                List<String> ListaValores = new List<string>();
                List<Venda> listaDoForeach = new List<Venda>();
                var vendas = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Where(v => v.Clientes.StatusId == 1 && v.DataVenda.Year == DateTime.Now.Year).ToList();
                foreach (var item in vendas)
                {
                    item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                    listaDoForeach.Add(item);
                }

                var clientes = listaDoForeach.Where(v => v.Clientes.NomeCliente != "Sem Cliente" && v.StatusId == 1).Select(v => v.Clientes).Distinct().ToList();
                int posicao = 1;
                foreach (var data in clientes)
                {
                    RankingCliente rcObject = new RankingCliente();

                    var soma = vendas.Where(v => v.Clientes.NomeCliente == data.NomeCliente).Sum(v => Double.Parse(v.ValorVenda, new CultureInfo("pt-BR")));
                    rcObject.Posicao = posicao.ToString() + "º";
                    rcObject.NomeCliente = data.NomeCliente;
                    rcObject.EmailCliente = data.EmailCliente;
                    rcObject.TelefoneCliente = data.TelefoneCliente;
                    rcObject.ValorVenda = soma.ToString();
                    rcObject.CEP = data.CEPcliente;
                    ListaFinal.Add(rcObject);
                }
                ListaFinal = ListaFinal.OrderByDescending(b => Double.Parse(b.ValorVenda.Replace('.', ','), new CultureInfo("pt-BR"))).ToList();
                foreach (var obj in ListaFinal)
                {
                    obj.Posicao = posicao.ToString() + "º";
                    obj.ValorVenda = "R$" + obj.ValorVenda;
                    posicao++;
                    rankingClientes.Add(obj);
                }
                ListaParaCsvRanking = rankingClientes;
                ViewBag.ranking = rankingClientes;
                ViewBag.count = rankingClientes.Count();

                ViewBag.filtro = "Filtrado por - Todos";
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
            }
            var ceps = _context.Vendas.Include(v => v.Bancas).Where(v=>v.Clientes.NomeCliente != "Sem cliente").Select(v => v.Clientes.CEPcliente).Distinct();
            ViewBag.cep = ceps;
            ViewBag.anos = _context.Vendas.Select(v => v.DataVenda.Year).Distinct().ToList();
            return View();

        }

        [HttpPost]
        public IActionResult GerarChartCliente(string email)
        {
            List<Venda> listaDoForeach = new List<Venda>();
            List<clienteMensal> listaClientes = new List<clienteMensal>();

            var cpf = _context.Clientes.Where(c => c.EmailCliente.ToLower() == email.ToLower()).Select(v=>v.CPFcliente).FirstOrDefault();
            var vendas = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Where(v => v.Clientes.StatusId == 1 && v.DataVenda.Year == DateTime.Now.Year).ToList();
            foreach (var item in vendas)
            {
                item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                listaDoForeach.Add(item);
            }
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    var clienteValor = listaDoForeach.Where(v => v.CPFcliente == cpf && v.DataVenda.Month == i).Sum(v => Double.Parse(v.ValorVenda, new CultureInfo("pt-BR")));
                    clienteMensal cm = new clienteMensal();
                    cm.ValorMensal = clienteValor;
                    cm.NomeMes = Utils.DataUtil.GetNameMonth(i);
                    listaClientes.Add(cm);
                }
                return Json(listaClientes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult RankingClientes(string buscaMes, string buscaAno, string buscaCep)
        {
            try
            {
                List<RankingCliente> rankingClientes = new List<RankingCliente>();
                List<RankingCliente> ListaFinal = new List<RankingCliente>();
                List<String> ListaValores = new List<string>();
                List<Venda> listaDoForeach = new List<Venda>();
                List<Venda> listaDoIf = new List<Venda>();
                List<Venda> listaDaVendaDoParametro = new List<Venda>();
                if (buscaMes.ToLower() == "todos")
                {
                    listaDaVendaDoParametro = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Include(v=>v.Bancas).ToList();
                    listaDoIf = listaDaVendaDoParametro;
                }
                else
                {
                    listaDaVendaDoParametro = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Include(v=>v.Bancas).Where(v => v.DataVenda.Month == Convert.ToInt32(buscaMes)).ToList();
                    listaDoIf = listaDaVendaDoParametro;
                }

                if (buscaAno.ToLower() != "todos")
                {
                    var venda = listaDoIf.Where(v => v.DataVenda.Year == Convert.ToInt32(buscaAno)).ToList();
                    listaDoIf = venda;
                }


                if (buscaCep.ToLower() != "todos")
                {
                    var venda = listaDoIf.Where(v => v.Clientes.CEPcliente == buscaCep).ToList();
                    listaDoIf = venda;
                }

                foreach (var item in listaDoIf)
                {
                    item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                    listaDoForeach.Add(item);
                }

                var clientes = listaDoForeach.Where(v => v.Clientes.NomeCliente != "Sem Cliente").Select(v => v.Clientes).Distinct().ToList();
                int posicao = 1;
                foreach (var data in clientes)
                {
                    RankingCliente rcObject = new RankingCliente();

                    var soma = listaDoIf.Where(v => v.Clientes.NomeCliente == data.NomeCliente && v.Clientes.StatusId == 1).Sum(v => Double.Parse(v.ValorVenda, new CultureInfo("pt-BR")));
                    rcObject.Posicao = posicao.ToString() + "º";
                    rcObject.NomeCliente = data.NomeCliente;
                    rcObject.EmailCliente = data.EmailCliente;
                    rcObject.TelefoneCliente = data.TelefoneCliente;
                    rcObject.ValorVenda = soma.ToString();
                    rcObject.CEP = data.CEPcliente;
                    ListaFinal.Add(rcObject);
                }
                ListaFinal = ListaFinal.OrderByDescending(b => Double.Parse(b.ValorVenda.Replace('.', ','), new CultureInfo("pt-BR"))).ToList();
                foreach (var obj in ListaFinal)
                {
                    obj.Posicao = posicao.ToString() + "º";
                    obj.ValorVenda = "R$" + obj.ValorVenda;
                    posicao++;
                    rankingClientes.Add(obj);
                }
                ListaParaCsvRanking = rankingClientes;
                ViewBag.ranking = rankingClientes;
                ViewBag.count = rankingClientes.Count();

                ViewBag.filtro = buscaMes.ToLower() == "todos" ? "Filtrado por - Todos" : "Filtrado por - " + DataUtil.GetNameMonth(Convert.ToInt32(buscaMes));
            }
            catch (Exception ex)
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
            }
            var ceps = _context.Vendas.Include(v => v.Bancas).Select(v => v.Clientes.CEPcliente).Distinct();
            ViewBag.cep = ceps;
            ViewBag.anos = _context.Vendas.Select(v => v.DataVenda.Year).Distinct().ToList();

            return View();

        }

        public List<VendasBancaRelatorio> resetarLista()
        {
            List<VendasBancaRelatorio> VendaRelatorio = new List<VendasBancaRelatorio>();
            List<VendasBancaRelatorio> ListaFinal = new List<VendasBancaRelatorio>();
            List<Venda> listaDoForeach = new List<Venda>();
            var bancasNome = _context.Vendas.Include(b => b.Bancas).Select(b => b.Bancas.NomeBanca).Distinct().ToList();
            var vendas = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Bancas).ToList();
            foreach (var item in vendas)
            {
                item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                listaDoForeach.Add(item);
            }
            int posicao = 1;
            foreach (var data in bancasNome)
            {
                var soma = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower()).Sum(v => Convert.ToDouble(v.ValorVenda));

                VendasBancaRelatorio vbr = new VendasBancaRelatorio();
                vbr.QuantidadeDeVendas = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower()).Count();
                vbr.NomeBanca = data;
                vbr.ValorTotalVenda = "R$" + soma;
                vbr.Bairro = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower()).Select(v => v.Bancas.Bairro).FirstOrDefault();
                VendaRelatorio.Add(vbr);
            }
            VendaRelatorio = VendaRelatorio.OrderByDescending(b => b.QuantidadeDeVendas).ToList();
            foreach (var obj in VendaRelatorio)
            {
                obj.Posicao = posicao + "º";
                ListaFinal.Add(obj);
                posicao++;
            }
            ListaParaCsvBanca = ListaFinal;
            ViewBag.bancas = ListaFinal;
            ViewBag.ceps = _context.Vendas.Include(b => b.Bancas).Select(b => b.Bancas.CEPbanca).Distinct().ToList();
            ViewBag.countProd = ListaFinal.Select(p => p.NomeBanca).Distinct().Count();
            return ListaFinal.OrderByDescending(b => b.QuantidadeDeVendas).ToList();
           
        }

        public IActionResult BancasMaisVendem(int page = 1)
        {

            List<VendasBancaRelatorio> VendaRelatorio = new List<VendasBancaRelatorio>();
            List<VendasBancaRelatorio> ListaFinal = new List<VendasBancaRelatorio>();
            List<Venda> listaDoForeach = new List<Venda>();
            var bancasNome = _context.Vendas.Include(b => b.Bancas).Select(b => b.Bancas.NomeBanca).Distinct().ToList();
            var vendas = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).Include(v => v.Bancas).ToList();
            foreach (var item in vendas)
            {
                item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                listaDoForeach.Add(item);
            }
            int posicao = 1;
            foreach (var data in bancasNome)
            {
                var soma = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower() && v.DataVenda.Year == DateTime.Now.Year).Sum(v => Double.Parse(v.ValorVenda, new CultureInfo("pt-BR")));

                VendasBancaRelatorio vbr = new VendasBancaRelatorio();
                vbr.QuantidadeDeVendas = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower()).Count();
                vbr.NomeBanca = data;
                vbr.ValorTotalVenda = "R$" + soma;
                vbr.Bairro = vendas.Where(v => v.Bancas.NomeBanca.ToLower() == data.ToLower()).Select(v => v.Bancas.Bairro).FirstOrDefault();
                VendaRelatorio.Add(vbr);
            }
            VendaRelatorio = VendaRelatorio.OrderByDescending(b => b.QuantidadeDeVendas).ToList();
            foreach (var obj in VendaRelatorio)
            {
                obj.Posicao = posicao + "º";
                ListaFinal.Add(obj);
                posicao++;
            }
            ListaParaCsvBanca = ListaFinal;
            ViewBag.bancas = ListaFinal;
            ViewBag.ceps = _context.Vendas.Include(b=>b.Bancas).Select(b => b.Bancas.CEPbanca).Distinct().ToList();
            ViewBag.countProd = ListaFinal.Select(p => p.NomeBanca).Distinct().Count();
            var query = ListaFinal.AsQueryable().OrderByDescending(b => b.QuantidadeDeVendas);
            var model = PagingList.Create(query, 500, page);
            model.Action = "BancasMaisVendem";
            ViewBag.ordenado = "Ordenado por: Quantidade vendida";

            return View(model);
        }


        [HttpPost]
        public IActionResult BancasMaisVendem(string buscaName, string cep, int page = 1)
        {
            if (buscaName == "1")
            {
                ListaParaCsvBanca = ListaParaCsvBanca.OrderByDescending(b => b.QuantidadeDeVendas).ToList();
                int posicao = 1;
                foreach (var y in ListaParaCsvBanca)
                {
                    y.Posicao = posicao + "º";
                    posicao++;
                }
                var query = ListaParaCsvBanca.AsQueryable().OrderByDescending(b => b.QuantidadeDeVendas);
                ViewBag.countProd = ListaParaCsvBanca.Select(p => p.NomeBanca).Distinct().Count();
                ViewBag.ceps = _context.Bancas.Select(b => b.CEPbanca).Distinct().ToList();

                var model = PagingList.Create(query, 500, page);
                model.Action = "BancasMaisVendem";
                ViewBag.ordenado = "Ordenado por: Quantidade vendida";

                return View(model);
            }
            else if (buscaName == "2")
            {
                List<VendasBancaRelatorio> vbr = new List<VendasBancaRelatorio>();
                foreach (var x in ListaParaCsvBanca)
                {
                    x.ValorTotalVenda = x.ValorTotalVenda.Substring(2);
                    vbr.Add(x);
                }
                ListaParaCsvBanca = vbr.OrderByDescending(v => Double.Parse(v.ValorTotalVenda, new CultureInfo("pt-BR"))).ToList();
                int posicao = 1;
                foreach (var y in ListaParaCsvBanca)
                {
                    y.Posicao = posicao + "º";
                    y.ValorTotalVenda = "R$" + y.ValorTotalVenda;
                    posicao++;
                }
                ViewBag.countProd = ListaParaCsvBanca.Select(p => p.NomeBanca).Distinct().Count();
                ViewBag.ceps = _context.Vendas.Include(b => b.Bancas).Select(b => b.Bancas.CEPbanca).Distinct().ToList();

                var query = ListaParaCsvBanca.AsQueryable();
                var model = PagingList.Create(query, 500, page);
                model.Action = "BancasMaisVendem";
                ViewBag.ordenado = "Ordenado por: Valor total";

                return View(model);
            }
            else if(buscaName == "cep")
            {
                if (cep.ToLower() == "todos")
                {
                    return RedirectToAction("BancasMaisVendem");
                }
                else
                {
                    ListaParaCsvBanca = resetarLista();
                    List<VendasBancaRelatorio> vbr = new List<VendasBancaRelatorio>();
                    List<VendasBancaRelatorio> listaNew = new List<VendasBancaRelatorio>();
                    foreach (var x in ListaParaCsvBanca)
                    {
                        x.ValorTotalVenda = x.ValorTotalVenda.Substring(2);
                        vbr.Add(x);
                    }
                   
                    var bancaDoCep = _context.Bancas.Where(b => b.CEPbanca == cep).Select(v=>v.NomeBanca).ToList();

                    ViewBag.ceps = _context.Vendas.Include(b => b.Bancas).Select(b => b.Bancas.CEPbanca).Distinct().ToList();

                    foreach (var b in bancaDoCep)
                    {
                        var result = vbr.Where(p => p.NomeBanca.ToLower() == b.ToLower()).Distinct().FirstOrDefault();
                        listaNew.Add(result);
                    }
                    ListaParaCsvBanca = listaNew.OrderByDescending(v => Double.Parse(v.ValorTotalVenda, new CultureInfo("pt-BR"))).ToList();
                    int posicao = 1;

                    foreach (var y in ListaParaCsvBanca)
                    {
                        y.Posicao = posicao + "º";
                        y.ValorTotalVenda = "R$" + y.ValorTotalVenda;
                        posicao++;
                    }
                    ViewBag.countProd = ListaParaCsvBanca.Select(p => p.NomeBanca).Distinct().Count();
                    var query = ListaParaCsvBanca.AsQueryable();
                    var model = PagingList.Create(query, 500, page);
                    model.Action = "BancasMaisVendem";
                    ViewBag.ordenado = "Ordenado por: Valor total";

                    return View(model);
                }
            }
            else
            {

                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return RedirectToAction("BancasMaisVendem");
            }
        }

        [HttpPost]
        public JsonResult GenerateChart(string NomeBanca)
        {
            List<ChartMonth> listaMes = new List<ChartMonth>();
            var x = _context.Vendas.Include(v => v.Bancas).Where(v => v.Bancas.NomeBanca.ToLower() == NomeBanca && v.DataVenda.Year == DateTime.Now.Year).ToList();
            var numbers = DataUtil.GetMonthsNumbers();
            foreach (var item in numbers)
            {
                var z = x.Where(c => c.DataVenda.Month == item).Count();
                var valor = x.Where(c => c.DataVenda.Month == item).Select(v => Double.Parse(v.ValorVenda.Substring(2).Replace('.', ','), new CultureInfo("pt-BR"))).Sum();
                ChartMonth cm = new ChartMonth();
                cm.NomeMes = DataUtil.GetNameMonth(item);
                cm.Quantidade = z;
                cm.Valor = valor;
                listaMes.Add(cm);
            }
            return Json(listaMes);
        }

        [HttpPost]
        public JsonResult GenerateTopBancas(string tipo)
        {
            var bancas = _context.Bancas.Select(b => b.NomeBanca).Distinct().ToList();
            List<BancaAnualMensal> Listba = new List<BancaAnualMensal>();
            foreach (var item in bancas)
            {
                List<double> listaValores = new List<double>();
                var valorTotal = _context.Vendas.Include(v => v.Bancas).Where(v => v.Bancas.NomeBanca.ToLower() == item.ToLower() && v.DataVenda.Year == DateTime.Now.Year).Select(v => Double.Parse(v.ValorVenda.Substring(2).Replace('.', ','), new CultureInfo("pt-BR"))).ToList();
                
                var valorMensal = _context.Vendas.Include(v => v.Bancas).Where(v => v.Bancas.NomeBanca == item && v.DataVenda.Month == DateTime.Now.Month && v.DataVenda.Year == DateTime.Now.Year).Select(v => Double.Parse(v.ValorVenda.Substring(2).Replace('.', ','), new CultureInfo("pt-BR"))).ToList();
                BancaAnualMensal ba = new BancaAnualMensal();
                ba.NomeBanca = item;
                ba.ValorAnual = valorTotal.Sum();
                ba.ValorMensal = valorMensal.Sum();
                Listba.Add(ba);
            
            }
            if (tipo == "mes")
            {
                return Json(Listba.Where(v=>v.ValorMensal != 0).OrderByDescending(v=>v.ValorMensal).Take(10));
            }
            else
            {
                return Json(Listba.Where(v=>v.ValorAnual != 0).OrderByDescending(v =>v.ValorAnual).Take(10));
            }
            
        }

        [HttpPost]
        public JsonResult GenerateTopItem(string tipo)
        {
            List<ProdutoQuantidade> listpq = new List<ProdutoQuantidade>();
            var idsProdsAndVenda = _context.VendaProduto.Select(v => new { v.ProdutoId ,v.VendaId,v.ValorTotalDoProduto}).ToList();
            var idsProdsDistintos = idsProdsAndVenda.Select(v => v.ProdutoId).Distinct();
            
            foreach (var item in idsProdsDistintos)
            {
                double valorTotalMes = 0;
                ProdutoQuantidade pq = new ProdutoQuantidade();
                pq.ValorAnual =_context.VendaProduto.Where(p => p.ProdutoId == item).Select(p => p.ValorTotalDoProduto).Sum();
                var vendas = idsProdsAndVenda.Where(v => v.ProdutoId == item).Select(v => v.VendaId).ToList();
                foreach (var x in vendas)
                {
                    var vendaUnica = _context.Vendas.Where(v => v.VendaId == x && v.DataVenda.Month == DateTime.Now.Month && v.DataVenda.Year == DateTime.Now.Year).FirstOrDefault();
                    if (vendaUnica != null )
                    {
                        valorTotalMes = valorTotalMes + _context.VendaProduto.Where(v => v.VendaId == vendaUnica.VendaId && v.ProdutoId == item).Select(v => v.ValorTotalDoProduto).FirstOrDefault();
                    }

                }
                pq.ValorMensal = valorTotalMes;
                pq.NomeProduto = _context.Produto.Where(p => p.ProdutoId == item).Select(p => p.NomeProduto).FirstOrDefault();
                listpq.Add(pq);
            }
          
            if (tipo == "mes")
            {
                return Json(listpq.Where(v=>v.ValorMensal != 0).OrderByDescending(v => v.ValorMensal).Take(10));
            }
            else
            {
                return Json(listpq.Where(v=>v.ValorAnual != 0).OrderByDescending(v => v.ValorAnual).Take(10));
            }

        }

        [HttpPost]
        public JsonResult GenerateChartItem(string NomeProduto)
        {
            List<ChartMonth> listaMes = new List<ChartMonth>();
            List<Venda> listaVenda = new List<Venda>();

            var idProd = _context.Produto.Where(p => p.NomeProduto.ToLower() == NomeProduto.ToLower()).Select(p => p.ProdutoId).FirstOrDefault();
            var prods = _context.VendaProduto.Where(v => v.Produtos.NomeProduto == NomeProduto).Select(v => v.VendaId).ToList();

            foreach (var prod in prods)
            {
                var result = _context.Vendas.Where(p => p.VendaId == prod).FirstOrDefault();
                listaVenda.Add(result);


            }
            var numbers = DataUtil.GetMonthsNumbers();
            foreach (var item in numbers)
            {
                double valPraSomar = 0;
                int quantPraSomar = 0;
                var noMesProd = _context.VendaProduto.Where(p => p.ProdutoId == idProd).Select(v => v.VendaId).ToList();
                foreach (var zum in noMesProd)
                {
                    quantPraSomar = quantPraSomar + listaVenda.Where(c => c.DataVenda.Month == item && c.VendaId == zum).Count();
                }
                var VendaComProd = listaVenda.Where(c => c.DataVenda.Month == item && c.DataVenda.Year == DateTime.Now.Year).Select(v => v.VendaId).ToList();
                foreach (var x in VendaComProd)
                {
                    valPraSomar = valPraSomar + _context.VendaProduto.Where(v => v.VendaId == x && v.ProdutoId == idProd).Select(v => v.ValorTotalDoProduto).FirstOrDefault();
                }
                ChartMonth cm = new ChartMonth();
                cm.NomeMes = DataUtil.GetNameMonth(item);
                cm.Quantidade = quantPraSomar;
                cm.Valor = valPraSomar;
                listaMes.Add(cm);
            }

            return Json(listaMes);
        }

        public IActionResult ItemMaisVende(int page = 1)
        {
            List<VendaProduto> ListaDasVendasProduto = new List<VendaProduto>();
            List<ItemMaisVende> listaItems = new List<ItemMaisVende>();
            var produtoId = _context.Produto.Select(p => p.ProdutoId).Distinct().ToList();
            var VendaId = _context.Vendas.Select(p => p.VendaId).Distinct().ToList();
            foreach (var item in produtoId)
            {
                var somaTotal = _context.VendaProduto.Where(v => v.ProdutoId == item).Select(v => v.ValorTotalDoProduto).Sum();
           
                ItemMaisVende im = new ItemMaisVende();
                im.Quantidade = _context.VendaProduto.Where(v => v.ProdutoId == item).Sum(v => v.QuantidadeVendida);
                im.NomeProduto = _context.Produto.Where(c => c.ProdutoId == item).Select(c => c.NomeProduto).FirstOrDefault();
                im.ValorTotalGerado = "R$" + somaTotal.ToString();
                listaItems.Add(im);
            }
            ListaParaCsvItem = listaItems.OrderByDescending(v => v.Quantidade).ToList();
            ViewBag.countProd = listaItems.Select(p => p.NomeProduto).Distinct().Count();
            ViewBag.item = listaItems.OrderByDescending(v => v.Quantidade).ToList();
            var query = listaItems.AsQueryable().OrderByDescending(v => v.Quantidade);
            var model = PagingList.Create(query, 500, page);
            ViewBag.ordenado = "Ordenado por: Quantidade vendida";

            model.Action = "ItemMaisVende";
            return View(model);

        }

        public JsonResult bancaHistorico(String bancaId)
        {
            int anoBase = DateTime.Now.Year - 10;
            List<BancaHistorico> listBh = new List<BancaHistorico>();
            try
            {
                for (int i = anoBase; i <= anoBase+10; i++)
                {
                    var bancaQuant = _context.Vendas.Include(v => v.Bancas).Where(v => v.Bancas.NomeBanca.ToLower() == bancaId.ToLower() && v.DataVenda.Year == i).Select(v => v.QuantidadeVendida).Sum();
                    BancaHistorico bh = new BancaHistorico();
                    bh.Quantidade = bancaQuant;
                    bh.Ano = i;
                    listBh.Add(bh);
                }
                return Json(listBh);
            }
            catch (Exception ex )
            {
                return null;
            }
        }

        public JsonResult ItemHistorico(String nomeProd)
        {
            int anoBase = DateTime.Now.Year - 10;
            int quantidade = 0;
            var produtoId = _context.Produto.Where(v => v.NomeProduto == nomeProd).Select(v=>v.ProdutoId).FirstOrDefault();
            List<BancaHistorico> listBh = new List<BancaHistorico>();
            try
            {
                for (int i = anoBase; i <= anoBase + 10; i++)
                {
                    var itemQuant = _context.Vendas.Where(v => v.ProdutosSalvos.Contains(nomeProd) && v.DataVenda.Year == i).Select(v => v.VendaId).ToList();
                    foreach (var item in itemQuant)
                    {
                        quantidade = quantidade + _context.VendaProduto.Where(p => p.ProdutoId == produtoId && p.VendaId == item).Select(p => p.QuantidadeVendida).FirstOrDefault();
                    }

                    BancaHistorico bh = new BancaHistorico();
                    bh.Quantidade = quantidade;
                    bh.Ano = i;
                    listBh.Add(bh);
                }
                return Json(listBh);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public IActionResult ItemMaisVende(string buscaName, int page = 1)
        {
            if (buscaName == "1")
            {
                ListaParaCsvItem = ListaParaCsvItem.OrderByDescending(b => b.Quantidade).ToList();

                var query = ListaParaCsvItem.AsQueryable().OrderByDescending(b => b.Quantidade);
                ViewBag.countProd = ListaParaCsvItem.Select(p => p.NomeProduto).Count();

                var model = PagingList.Create(query, 500, page);
                model.Action = "ItemMaisVende";
                ViewBag.ordenado = "Ordenado por: Quantidade vendida";

                return View(model);
            }
            else if (buscaName == "2")
            {
                List<ItemMaisVende> vbr = new List<ItemMaisVende>();
                foreach (var x in ListaParaCsvItem)
                {
                    x.ValorTotalGerado = x.ValorTotalGerado.Substring(2);
                    vbr.Add(x);
                }
                ListaParaCsvItem = vbr.OrderByDescending(v => Convert.ToDouble(v.ValorTotalGerado)).ToList();
                foreach (var y in ListaParaCsvItem)
                {

                    y.ValorTotalGerado = "R$" + y.ValorTotalGerado;
                }
                ViewBag.countProd = ListaParaCsvItem.Select(p => p.NomeProduto).Count();

                var query = ListaParaCsvItem.AsQueryable();
                var model = PagingList.Create(query, 500, page);
                model.Action = "ItemMaisVende";
                ViewBag.ordenado = "Ordenado por: Valor total";

                return View(model);
            }
            else
            {
                TempData["msgSucesso"] = "Erro na sua solicitação, favor tentar novamente!";
                return RedirectToAction("ItemMaisVende");
            }
        }


        public IActionResult EstoqueZerado(int page = 1)
        {
            var prods = _context.Produto.Include(p => p.tipoProduto).Where(p => p.QuantideProduto == 0).ToList();
            ListaParaCsvProds = prods.OrderBy(v => v.NomeProduto).ToList();
            var query = prods.AsQueryable().OrderBy(v => v.NomeProduto);
            var model = PagingList.Create(query, 5, page);
            model.Action = "EstoqueZerado";
            return View(model);

        }

        public IActionResult LoginRankingClientes(String email)
        {
            var emailFound = _context.Clientes.Where(c => c.EmailCliente == email).FirstOrDefault();
            if (emailFound != null)
            {
                return RedirectToAction("RankingClientes");
            }
            else
            {
                return RedirectToAction("ViewLoginCliente", "Jornaleiros", new { error = "error" });
            }
        }

        public IActionResult GerarCSVRanking()
        {
            var registros = ListaParaCsvRanking;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("Posicao;Nome Cliente;Email Cliente;Telefone Cliente,ValorVenda");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.NomeCliente);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

                byte[] tempBytes2;
                tempBytes2 = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.Posicao);
                string asciiStr2 = System.Text.Encoding.UTF8.GetString(tempBytes2);

                arquivo.AppendLine(asciiStr2 + ";" + asciiStr + ";" + item.EmailCliente + ";" + item.TelefoneCliente + ";" + item.ValorVenda);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "RankingClientes.csv");
        }

        public IActionResult GerarCSVBancas()
        {
            var registros = ListaParaCsvBanca;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("Posicao;Nome Banca;Bairro;Quantidade De Vendas;Valor Total Venda");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.NomeBanca);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

                byte[] tempBytes2;
                tempBytes2 = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.Posicao);
                string asciiStr2 = System.Text.Encoding.UTF8.GetString(tempBytes2);

                byte[] tempBytes3;
                tempBytes3 = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.Bairro);
                string asciiStr3 = System.Text.Encoding.UTF8.GetString(tempBytes3);

                arquivo.AppendLine(asciiStr2 + ";" + asciiStr + ";" + asciiStr3 + ";" + item.QuantidadeDeVendas + ";" + item.ValorTotalVenda);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "BancasMaisVendem.csv");
        }

        public IActionResult GerarCSVItem()
        {
            var registros = ListaParaCsvItem;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("Nome Produto;Quantidade;Valor Total Gerado");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.NomeProduto);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);


                arquivo.AppendLine(asciiStr + ";" + item.Quantidade + ";" + item.ValorTotalGerado);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "ItemMaisVendem.csv");
        }

        public IActionResult GerarCSVProds()
        {
            var registros = ListaParaCsvProds;
            StringBuilder arquivo = new StringBuilder();
            arquivo.AppendLine("Nome Produto;Valor;Categoria");

            foreach (var item in registros)
            {

                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(item.NomeProduto);
                string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);


                arquivo.AppendLine(asciiStr + ";" + item.ValorProduto + ";" + item.tipoProduto.NomeTipoProduto);
            }

            return File(Encoding.ASCII.GetBytes(arquivo.ToString()), "text/csv", "EstoqueZerado.csv");
        }
    }
}
