using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.ItemCompraViewModels;


namespace ShopApp.Controllers
{
    public class ItemComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemComprasController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: ItemCompras
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.ItemCompra.Include(i => i.Compra).Include(i => i.Prenda)
                ; //CAMBIO
            // Esta bien?
            //Quitado .toList() de applicationDbContext
            if (id != null)
            {
                var idSearch = _context.ItemCompra.Where(s => s.CompraID == id);

                return View(await idSearch.ToListAsync());
            }

            return View( await applicationDbContext.ToListAsync());
        }

        // GET: ItemCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCompra = await _context.ItemCompra
                .Include(i => i.Compra)
                .Include(i => i.Prenda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCompra == null)
            {
                return NotFound();
            }

            return View(itemCompra);
        }

        // GET: ItemCompras/Create
        public IActionResult Create()
        {
            ViewData["CompraID"] = new SelectList(_context.Compra, "Id", "ClienteId");
            ViewData["PrendaID"] = new SelectList(_context.Prenda, "PrendaID", "Nombre");
            return View();
        }

        // POST: ItemCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrendaID,Cantidad,CompraID")] ItemCompra itemCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompraID"] = new SelectList(_context.Compra, "Id", "ClienteId", itemCompra.CompraID);
            ViewData["PrendaID"] = new SelectList(_context.Prenda, "PrendaID", "Nombre", itemCompra.PrendaID);
            return View(itemCompra);
        }

        // GET: ItemCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCompra = await _context.ItemCompra.FindAsync(id);
            if (itemCompra == null)
            {
                return NotFound();
            }
            ViewData["CompraID"] = new SelectList(_context.Compra, "Id", "ClienteId", itemCompra.CompraID);
            ViewData["PrendaID"] = new SelectList(_context.Prenda, "PrendaID", "Nombre", itemCompra.PrendaID);
            return View(itemCompra);
        }

        // POST: ItemCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrendaID,Cantidad,CompraID")] ItemCompra itemCompra)
        {
            if (id != itemCompra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCompraExists(itemCompra.Id))
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
            ViewData["CompraID"] = new SelectList(_context.Compra, "Id", "ClienteId", itemCompra.CompraID);
            ViewData["PrendaID"] = new SelectList(_context.Prenda, "PrendaID", "Nombre", itemCompra.PrendaID);
            return View(itemCompra);
        }

        // GET: ItemCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCompra = await _context.ItemCompra
                .Include(i => i.Compra)
                .Include(i => i.Prenda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCompra == null)
            {
                return NotFound();
            }

            return View(itemCompra);
        }

        // POST: ItemCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemCompra = await _context.ItemCompra.FindAsync(id);
            _context.ItemCompra.Remove(itemCompra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemCompraExists(int id)
        {
            return _context.ItemCompra.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public IActionResult SelectPrendasForDevolucion(int? id, string marcaSelect, string nombrePrenda)
        {

            SelectPrendasForDevolucionViewModel selectPrendas = new SelectPrendasForDevolucionViewModel();
            selectPrendas.Marcas = new SelectList(_context.Marca.Select(m => m.Nombre).ToList());

          
             selectPrendas.ItemCompras = _context.ItemCompra
                .Include(m => m.Prenda).ThenInclude( g=>g.Marca).Include(p => p.Compra).ThenInclude(m=>m.MetodoPago).Include( item => item.Compra).ThenInclude( item => item.Cliente)
                .Where( items => items.Compra.Id == id  &&
                (items.Compra.Cliente.UserName == User.Identity.Name)&& 
                (items.Prenda.Marca.Nombre.Contains(marcaSelect) || marcaSelect == null)
                && (items.Prenda.Nombre.Contains(nombrePrenda) || nombrePrenda == null )).ToList();
            var devoluciones = _context.ItemDevolucion.Where(item => selectPrendas.ItemCompras.Contains(item.ItemCompra));
           
            foreach( ItemDevolucion it in devoluciones)
            {
                selectPrendas.ItemCompras.Remove(it.ItemCompra);
            }
            
            selectPrendas.ItemCompras = selectPrendas.ItemCompras.ToList();
            selectPrendas.Marcas = new SelectList(_context.Marca.Select(m => m.Nombre).ToList());
            return View(selectPrendas);
        }


        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPrendasForDevolucion(SelectedPrendasForDevolucionViewModel selectedPrendas)
        {
            if (selectedPrendas.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Devoluciones", selectedPrendas);
            }
            //mensaje de error si el gestor no selecciona ninguna prenda
            ModelState.AddModelError(string.Empty, "Tienes que elegir al menos una prenda");

            //the View SelectMoviesForPurchase will be shown again
            return SelectPrendasForDevolucion(selectedPrendas.id, selectedPrendas.marcaSelect, selectedPrendas.nombrePrenda);
        }
    }
}
