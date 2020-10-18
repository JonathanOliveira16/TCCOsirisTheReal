using System;
using System.Collections.Generic;
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
            try
            {
                List<RankingCliente> rankingClientes = new List<RankingCliente>();
                List<RankingCliente> ListaFinal = new List<RankingCliente>();
                List<String> ListaValores = new List<string>();
                List<Venda> listaDoForeach = new List<Venda>();
                var vendas = _context.Vendas.Include(v => v.Clientes).Include(v => v.Status).ToList();
                foreach (var item in vendas)
                {
                    item.ValorVenda = item.ValorVenda.Substring(2).Replace('.', ',');
                    listaDoForeach.Add(item);
                }

                var clientes = listaDoForeach.Select(v => v.Clientes).Distinct().ToList();
                int posicao = 1;
                foreach (var data in clientes)
                {
                    RankingCliente rcObject = new RankingCliente();

                    var soma = vendas.Where(v => v.Clientes.NomeCliente == data.NomeCliente && v.Clientes.Status.StatusId == 1).Sum(v => Convert.ToDouble(v.ValorVenda));
                    rcObject.Posicao = posicao.ToString() + "º";
                    rcObject.NomeCliente = data.NomeCliente;
                    rcObject.EmailCliente = data.EmailCliente;
                    rcObject.TelefoneCliente = data.TelefoneCliente;
                    rcObject.ValorVenda = soma.ToString();
                    ListaFinal.Add(rcObject);
                }
                ListaFinal = ListaFinal.OrderByDescending(b => Convert.ToDouble(b.ValorVenda.Replace('.', ','))).ToList();
                foreach (var obj in ListaFinal)
                {
                    obj.Posicao = posicao.ToString() + "º";
                    obj.ValorVenda = "R$" + obj.ValorVenda;
                    posicao++;
                    rankingClientes.Add(obj);
                }
                ListaParaCsvRanking = rankingClientes;
                ViewBag.ranking = rankingClientes;

            }
            catch (Exception ex)
            {

                throw;
            }


            return View();

        }

        public IActionResult BancasMaisVendem(int page=1)
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
            var query = ListaFinal.AsQueryable().OrderByDescending(b => b.QuantidadeDeVendas);
            var model =  PagingList.Create(query, 500, page);
            model.Action = "BancasMaisVendem";
            ViewBag.ordenado = "Ordenado por: Quantidade vendida";

            return View(model);
        }


        [HttpPost]
        public IActionResult BancasMaisVendem(string buscaName, int page =1)
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
                
                var model = PagingList.Create(query, 500, page);
                model.Action = "BancasMaisVendem";
                ViewBag.ordenado = "Ordenado por: Quantidade vendida";

                return View(model);
            }
            else if(buscaName == "2")
            {
                List<VendasBancaRelatorio> vbr = new List<VendasBancaRelatorio>();
                foreach (var x in ListaParaCsvBanca)
                {
                    x.ValorTotalVenda = x.ValorTotalVenda.Substring(2);
                    vbr.Add(x);
                }
                ListaParaCsvBanca = vbr.OrderByDescending(v=>Convert.ToDouble(v.ValorTotalVenda)).ToList();
                int posicao = 1;
                foreach (var y in ListaParaCsvBanca)
                {
                    y.Posicao = posicao + "º";
                    y.ValorTotalVenda = "R$" + y.ValorTotalVenda;
                    posicao++;
                }
                var query = ListaParaCsvBanca.AsQueryable();
                var model = PagingList.Create(query, 500, page);
                model.Action = "BancasMaisVendem";
                ViewBag.ordenado = "Ordenado por: Valor total";

                return View(model);
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
            var x = _context.Vendas.Include(v => v.Bancas).Where(v => v.Bancas.NomeBanca.ToLower() == NomeBanca).ToList();
            var numbers = DataUtil.GetMonthsNumbers();
            foreach (var item in numbers)
            {
                var z = x.Where(c => c.DataVenda.Month == item).Count();
                var valor = x.Where(c => c.DataVenda.Month == item).Select(v => Convert.ToDouble(v.ValorVenda.Substring(2).Replace('.',','))).Sum();
                ChartMonth cm = new ChartMonth();
                cm.NomeMes = DataUtil.GetNameMonth(item);
                cm.Quantidade = z;
                cm.Valor = valor;
                listaMes.Add(cm);
            }
            return Json(listaMes);
        }

        public IActionResult ItemMaisVende(int page=1)
        {
            List<VendaProduto> ListaDasVendasProduto = new List<VendaProduto>();
            List<ItemMaisVende> listaItems = new List<ItemMaisVende>();
            var produtoId = _context.Produto.Select(p => p.ProdutoId).Distinct().ToList();
            var VendaId = _context.Vendas.Select(p => p.VendaId).Distinct().ToList();
            foreach (var item in produtoId)
            {
                double soma = 0;
                var vendas = _context.VendaProduto.Where(v => v.ProdutoId == item).Select(v => v.VendaId).ToList();
                foreach (var valor in vendas)
                {
                    var valorVenda = _context.Vendas.Where(v => v.VendaId == valor).Select(v => v.ValorVenda).FirstOrDefault();
                    soma = soma + Convert.ToDouble(valorVenda.Substring(2).Replace('.', ','));
                }
                ItemMaisVende im = new ItemMaisVende();
                im.Quantidade = _context.VendaProduto.Where(v => v.ProdutoId == item).Sum(v => v.QuantidadeVendida);
                im.NomeProduto = _context.Produto.Where(c => c.ProdutoId == item).Select(c => c.NomeProduto).FirstOrDefault();
                im.ValorTotalGerado = "R$"+soma.ToString();
                listaItems.Add(im);
            }
            ListaParaCsvItem = listaItems.OrderByDescending(v => v.Quantidade).ToList();
            ViewBag.item = listaItems.OrderByDescending(v => v.Quantidade).ToList();
            var query = listaItems.AsQueryable().OrderByDescending(v => v.Quantidade);
            var model = PagingList.Create(query, 5, page);
            model.Action = "ItemMaisVende";
            return View(model);
           
        }

        public IActionResult EstoqueZerado(int page = 1)
        {
            var prods = _context.Produto.Include(p=>p.tipoProduto).Where(p => p.QuantideProduto == 0).ToList();
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
