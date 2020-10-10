using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;

namespace OsirisPdvReal.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly Contexto _context;

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
                var vendas = _context.Vendas.Include(v=>v.Clientes).Include(v=>v.Status).ToList();
                var clientes = _context.Vendas.Select(v => v.Clientes).Distinct().ToList();
                int posicao = 1;
                foreach (var data in clientes)
                {
                    RankingCliente rcObject = new RankingCliente();

                    var soma = vendas.Where(v => v.Clientes.NomeCliente == data.NomeCliente && v.Clientes.Status.StatusId == 1).Sum(v => Convert.ToDouble(v.ValorVenda.Substring(2).Replace('.', ',')));
                    rcObject.Posicao = posicao.ToString() + "º";
                    rcObject.NomeCliente = data.NomeCliente;
                    rcObject.EmailCliente = data.EmailCliente;
                    rcObject.TelefoneCliente = data.TelefoneCliente;
                    rcObject.ValorVenda = "R$" + soma;
                    ListaFinal.Add(rcObject);
                    posicao++;
                }
                ViewBag.ranking = ListaFinal;
            }
            catch (Exception ex)
            {

                throw;
            }
          

            return View();

        }
    }
}
