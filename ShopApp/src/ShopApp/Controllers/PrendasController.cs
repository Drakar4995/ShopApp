using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Models.PrendaViewModels;
using ShopApp.Data;
using ShopApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShopApp.Controllers
{

    //[AllowAnonymous]
    [Authorize]
    public class PrendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prendas
        public async Task<IActionResult> Index(string SearchString)
        {
            var applicationDbContext = _context.Prenda.Include(p => p.Marca);

            //If the search string is NOT empty
            if (!String.IsNullOrEmpty(SearchString))
            {
                //Searching Prendas whose title contains SearchString
                var prendas = _context.Prenda
                                .Where(s => s.Nombre.Contains(SearchString)).
                                OrderBy(m => m.Nombre);
                 return View(await prendas.ToListAsync());
            }
            else

            return View(await _context.Prenda.OrderBy(m => m.Nombre).ToListAsync());


        }

        // GET: Prendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenda = await _context.Prenda
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(m => m.PrendaID == id);
            if (prenda == null)
            {
                return NotFound();
            }

            return View(prenda);
        }

        // GET: Prendas/Create
        public IActionResult Create()
        {
            ViewData["marcaNombre"] = new SelectList(_context.Marca, "Nombre", "Nombre");
            return View();
        }

        // POST: Prendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrendaID,Nombre,PrecioPrenda,FechaLanzamiento,CantidadCompra,marcaNombre,isRetired")] Prenda prenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["marcaNombre"] = new SelectList(_context.Marca, "Nombre", "Nombre", prenda.Marca.Nombre);
            return View(prenda);
        }

        // GET: Prendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenda = await _context.Prenda.FindAsync(id);
            if (prenda == null)
            {
                return NotFound();
            }
            ViewData["marcaNombre"] = new SelectList(_context.Marca, "Nombre", "Nombre", prenda.Marca.Nombre);
            return View(prenda);
        }

        // POST: Prendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrendaID,Nombre,PrecioPrenda,FechaLanzamiento,CantidadCompra,marcaNombre,isRetired")] Prenda prenda)
        {
            if (id != prenda.PrendaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrendaExists(prenda.PrendaID))
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
            ViewData["marcaNombre"] = new SelectList(_context.Marca, "Nombre", "Nombre", prenda.Marca.Nombre);
            return View(prenda);
        }


        // GET: Prendas/SelectPrendasForPurchase
        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public IActionResult SelectPrendasForPurchase(string prendaNombre, string prendaMarcaSeleccionada, int prendaPrecio)
        {
            SelectPrendasForPurchaseViewModel selectPrendas = new SelectPrendasForPurchaseViewModel();
            selectPrendas.Marcas = new SelectList(_context.Marca.Select(g => g.Nombre).ToList());
            selectPrendas.Prendas = _context.Prenda
                .Include(m => m.Marca) //join marca and prenda
                .Where(prenda => prenda.CantidadCompra > 0 // where clause
                && (prenda.Nombre.Contains(prendaNombre) || prendaNombre == null) && prenda.isRetired == false
                && (((prenda.Marca.Nombre.Contains(prendaMarcaSeleccionada) || prendaMarcaSeleccionada == null) && prenda.isRetired == false)
                && (prenda.PrecioPrenda == prendaPrecio || prendaPrecio == 0) && prenda.isRetired == false));

            selectPrendas.Prendas = selectPrendas.Prendas.ToList();
            return View(selectPrendas);
        }


        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPrendasForPurchase(SelectedPrendasForPurchaseViewModel selectedPrendas)
        {
            // Si el usuario ha seleccionado alguna prenda, entonces crearemos la compra.
            // Para ello llamaremos al método de acción Create (GET) de Purchase.
            if (selectedPrendas.IdsToAdd != null)
            {
                return RedirectToAction("Create", "Compras", selectedPrendas);
            }
            // Si el usuario no ha seleccionado ninguna película, le informaremos y
            // se vuelve a generar el ViewModel
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos una prenda");
            selectedPrendas.prendaNombre = "";
            selectedPrendas.prendaMarcaSeleccionada = "";
            selectedPrendas.prendaPrecio = 0;
            return SelectPrendasForPurchase(selectedPrendas.prendaNombre, selectedPrendas.prendaMarcaSeleccionada, selectedPrendas.prendaPrecio);
        }


        // GET: Prendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenda = await _context.Prenda
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(m => m.PrendaID == id);
            if (prenda == null)
            {
                return NotFound();
            }

            return View(prenda);
        }

        // POST: Prendas/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prenda = await _context.Prenda.FindAsync(id);
            _context.Prenda.Remove(prenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrendaExists(int id)
        {
            return _context.Prenda.Any(e => e.PrendaID == id);
        }

        
        [HttpGet]
        [Authorize(Roles = "Gestor")]
        public IActionResult SelectPrendasForRetirar(int prendaVentasSemanales, string prendaMarcaSelected)
        {
            
            if (prendaVentasSemanales == 0) { prendaVentasSemanales = 30; }

            SelectPrendasForRetirarViewModel selectPrendas = new SelectPrendasForRetirarViewModel();
            selectPrendas.Marcas = new SelectList(_context.Marca.Select(m => m.Nombre).ToList());
            selectPrendas.Prendas = _context.Prenda
                .Include(m => m.Marca).Include(itemC => itemC.PrendasCompradas).ThenInclude(c => c.Compra) //join table Prenda and table Marca
                .Where(prenda => (prenda.isRetired == false) // where clause
                && (prenda.Marca.Nombre.Contains(prendaMarcaSelected) || prendaMarcaSelected == null) &&
                (prenda.PrendasCompradas.Where(pc => pc.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0).Sum(pc => pc.Cantidad) < prendaVentasSemanales));

            foreach (var p in selectPrendas.Prendas)
            {
                
                ViewData[p.PrendaID.ToString()] = _context.ItemCompra.Include(c => c.Compra)
                    .Where(pr => pr.PrendaID == p.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad);

            }

            selectPrendas.Prendas = selectPrendas.Prendas.ToList();
            selectPrendas.Marcas = new SelectList(_context.Marca.Select(m => m.Nombre).ToList());
            return View(selectPrendas);
    }

        //[Authorize(Roles = "Gestor")]
        [HttpPost]
        [Authorize(Roles = "Gestor")]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPrendasForRetirar(SelectedPrendasForRetirarViewModel selectedPrendas)
        {
            if (selectedPrendas.IdsToAdd != null)
            {

                return RedirectToAction("Create","Retiradas",selectedPrendas);
            }
            //mensaje de error si el gestor no selecciona ninguna prenda
            ModelState.AddModelError(string.Empty, "Tienes que elegir al menos una prenda");

            //the View SelectMoviesForPurchase will be shown again
            return SelectPrendasForRetirar(selectedPrendas.prendaVentasSemanales, selectedPrendas.prendaMarcaSelected);


        }

       }
}
