using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsirisPdvReal.Models;
using ReflectionIT.Mvc.Paging;

namespace OsirisPdvReal.ViewComponents
{
    public class ProdutoViewComponent : ViewComponent
    {
        private readonly Contexto _context;

        public ProdutoViewComponent(Contexto context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var query = _context.Produto.Include(p => p.tipoProduto).AsNoTracking().OrderBy(j => j.NomeProduto);
            //var contexto = _context.Bancas.Include(b => b.Jornaleiro);

            var model = await PagingList.CreateAsync(query, 5, page);
            model.Action = "GetProdutos";
            return View(model);
        }
    }
}
