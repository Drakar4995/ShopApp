using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.PrendaViewModels;
using ShopApp.Models.RetiradaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ShopApp.Controllers
{
    [Authorize(Roles = "Gestor")]
    public class RetiradasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetiradasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Retiradas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Retirada.Include(r => r.Gestor).Where(g => g.Gestor.UserName == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Retiradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retirada = await _context.Retirada
                .Include(r => r.Gestor)
                .Include(r => r.motivosRetirada).ThenInclude(r=>r.Prenda)
                .ThenInclude(m=>m.Marca)
                .FirstOrDefaultAsync(m => m.id == id);
            if (retirada == null)
            {
                return NotFound();
            }
            foreach (var item in retirada.motivosRetirada)
            {
                ViewData[item.Prenda.PrendaID.ToString()] = _context.ItemCompra.Include(c => c.Compra)
                   .Where(pr => pr.PrendaID == item.Prenda.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad);
            }
            return View(retirada);
        }

        // GET: Retiradas/Create
        public IActionResult Create(SelectedPrendasForRetirarViewModel selectedPrendas)
        {
            RetiradaCreateViewModel retirada = new();
            retirada.MotivosRetirada = new List<MotivoRetiradaViewModel>();

            if (selectedPrendas.IdsToAdd == null)
            {
                ModelState.AddModelError("PrendaNoSeleccionada", "Tienes que seleccionar una prenda");
            }
            else
                retirada.MotivosRetirada = _context.Prenda.Include(prenda => prenda.Marca)
                    .Select(prenda => new MotivoRetiradaViewModel()
                    {

                        PrendaID = prenda.PrendaID,
                        VentasSemanales = _context.ItemCompra.Include(c => c.Compra)
                    .Where(pr => pr.PrendaID == prenda.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad),
                        Marca = prenda.Marca.Nombre,
                        Precio = prenda.PrecioPrenda,
                        Nombre = prenda.Nombre

                    })
                    .Where(prenda => selectedPrendas.IdsToAdd.Contains(prenda.PrendaID.ToString())).ToList();
            UsuarioApp Gestor = _context.Users.OfType<UsuarioApp>().FirstOrDefault<UsuarioApp>(u => u.UserName.Equals(User.Identity.Name));
            retirada.GestorId = Gestor.Id;
            retirada.Titulo = "Retirada" + (_context.Retirada.Count()+1).ToString();
            foreach (var p in retirada.MotivosRetirada)
            {

                ViewData[p.PrendaID.ToString()] = _context.ItemCompra.Include(c => c.Compra)
                    .Where(pr => pr.PrendaID == p.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad);

            }
            return View(retirada);
        }

        // POST: Retiradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> CreatePost(RetiradaCreateViewModel reritadaViewModel)
        {
            Prenda prenda; MotivoRetirada motivoRetirada;
            Gestor gestor;
            Retirada retirada = new();
            retirada.fechaEfectiva = reritadaViewModel.FechaEfectiva;
            retirada.motivosRetirada = new List<MotivoRetirada>();
            retirada.titulo = "Retirada" + (_context.Retirada.Count() + 1).ToString();
            gestor = await _context.Users.OfType<Gestor>().FirstOrDefaultAsync<Gestor>(u => u.UserName.Equals(User.Identity.Name));

            

            if (ModelState.IsValid)
            {
                if (reritadaViewModel.FechaEfectiva.CompareTo(DateTime.Today)<0)
                {
                    reritadaViewModel.Titulo= "Retirada" + (_context.Retirada.Count() + 1).ToString();
                    foreach(var item in reritadaViewModel.MotivosRetirada)
                    {
                        ViewData[item.PrendaID.ToString()]= _context.ItemCompra.Include(c => c.Compra)
                    .Where(pr => pr.PrendaID == item.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad);
                    }
                    ModelState.AddModelError("",$"Fecha no valida");
                }
                foreach (MotivoRetiradaViewModel item in reritadaViewModel.MotivosRetirada)
                {
                    prenda = await _context.Prenda.FirstOrDefaultAsync<Prenda>(m => m.PrendaID == item.PrendaID);
                   
                        
       
                            motivoRetirada = new MotivoRetirada
                            {
                                Prenda = prenda,
                                Retirada = retirada,
                                descripcion = item.Descripcion
                            };

                            motivoRetirada.Prenda.isRetired = true;
                            motivoRetirada.Prenda.Marca = prenda.Marca;
                            retirada.motivosRetirada.Add(motivoRetirada);
                        
                    
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                
                return View(reritadaViewModel);
            }


            
            retirada.Gestor = gestor;
            retirada.Gestor.Id = gestor.Id;
            retirada.descripcion = reritadaViewModel.Descripcion;
            _context.Add(retirada);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = retirada.id});
        }

        // GET: Retiradas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retirada = await _context.Retirada.FindAsync(id);
            if (retirada == null)
            {
                return NotFound();
            }
            ViewData["gestorId"] = new SelectList(_context.Set<Gestor>(), "Id", "Id", retirada.gestorId);
            return View(retirada);
        }

        // POST: Retiradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,titulo,descripcion,fechaEfectiva,gestorId")] Retirada retirada)
        {
            if (id != retirada.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retirada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetiradaExists(retirada.id))
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
            ViewData["gestorId"] = new SelectList(_context.Set<Gestor>(), "Id", "Id", retirada.Gestor.Id);
            return View(retirada);
        }

        // GET: Retiradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retirada = await _context.Retirada
                .Include(r => r.Gestor).Include( ir => ir.motivosRetirada)
                .FirstOrDefaultAsync(m => m.id == id);
            
            if (retirada == null)
            {
                return NotFound();
            }
            
            return View(retirada);
        }

        // POST: Retiradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retirada = await _context.Retirada.Include(r => r.motivosRetirada).ThenInclude(mr => mr.Prenda).FirstOrDefaultAsync(r=>r.id == id);
            var prendas = retirada.motivosRetirada.Select(p => p.Prenda);
            foreach(var prenda in prendas)
            {
                prenda.isRetired = false;
                _context.Prenda.Update(prenda);
            }
            
            _context.Retirada.Remove(retirada);
           //_context.MotivosRetirada.Remove(retirada.motivosRetirada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetiradaExists(int id)
        {
            return _context.Retirada.Any(e => e.id == id);
        }
    }
}
