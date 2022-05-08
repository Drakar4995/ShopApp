using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.NewsletterViewModels;
using ShopApp.Models.SuscripcionsViewModels;
using Microsoft.AspNetCore.Authorization;


namespace ShopApp.Controllers
{
   [Authorize]
    public class SuscripcionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suscripcions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Suscripcion.Include( m => m.motivoSuscripcion).ThenInclude(news => news.NewsLetter)
                .ThenInclude(mar => mar.Marca);
                //.Include(p => p.Cliente)
                //.Where(p => p.Cliente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Suscripcions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscription = await _context.Suscripcion.Include(motivoinclude => motivoinclude.motivoSuscripcion).ThenInclude(Ap => Ap.NewsLetter)
                .ThenInclude( ma=> ma.Marca)
                .Include(motivoinclude => motivoinclude.motivoSuscripcion).ThenInclude(Ap => Ap.NewsLetter).ThenInclude(ab => ab.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscription == null)
            {
                return NotFound();
            }

            return View(suscription);
        }

        // GET: Suscripcions/Create
        public IActionResult Create(SelectedNewsletterForSuscribeViewModel selectedNewsletter)
        {
            SuscipcionCreateViewModel suscripcions = new();
            suscripcions.MotivoSuscripcions = new List<MotivoSuscripcionViewModel>();
            suscripcions.Marca = _context.Marca.Where(g => g.Nombre != null).ToList();


            suscripcions.Prendas = _context.Prenda.Where(g => g.Nombre != null).ToList();
            

            if (selectedNewsletter.IdsToAdd == null)
            {
                ModelState.AddModelError("NewsletterNoSelected", "You should select at least a Newsletter to be suscribe, please");
            }
            else
            {
                suscripcions.MotivoSuscripcions = _context.NewsLetter.Include(NewsLetter1 => NewsLetter1.Marca).Include(NewsLetter2 => NewsLetter2.Categoria)
                                   .Select(NewsLetter => new MotivoSuscripcionViewModel()
                                   {
                                       NewsletterId = NewsLetter.Id,
                                       Marca = NewsLetter.Marca.Nombre,
                                       Categoria = NewsLetter.Categoria.Nombre,
                                       TituloNewssletter = NewsLetter.Titulo

                                   })
                                   .Where(NewsLetter2 => selectedNewsletter.IdsToAdd.Contains(NewsLetter2.NewsletterId)).ToList();

            }

           
            Cliente cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            suscripcions.Name = cliente.Name;
            suscripcions.FirstSurname = cliente.FirstSurname;
            suscripcions.SecondSurname = cliente.SecondSurname;
            return View(suscripcions);
        }

        // POST: Suscripcions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost (SuscipcionCreateViewModel suscripcionViewModel)
        {
            //NewsLetter newsletter; 
            MotivoSuscripcion motivosuscripcion;
            Cliente customer;
            Suscripcion suscripcion = new();
            suscripcion.motivoSuscripcion = new List<MotivoSuscripcion>();
            customer = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));


            if (ModelState.IsValid)
            {
                if (suscripcionViewModel.FechaCaducidad.CompareTo(DateTime.Today) < 0)
                {
                    suscripcionViewModel.Titulo = "Suscripcion" + (_context.Suscripcion.Count() + 1).ToString();
                    /*
                    foreach(var item in suscripcionViewModel.MotivoSuscripcions)
                    {
                        ViewData[item.NewsletterId.ToString()] = _context.MotivosSuscripcion.Include(c => c.Suscripcion)
                        .Where(pr => pr.newsletterId == item.NewsletterId && (pr.Suscripcion.FechaCaducidad.CompareTo(DateTime.Today.AddDays(-7))>0))
                        .Sum(ca => ca.Id);
                    }
                    */
                    ModelState.AddModelError("", $"Fecha no valida");
                }
              
                foreach (MotivoSuscripcionViewModel item in suscripcionViewModel.MotivoSuscripcions)
                {
                    NewsLetter newsletter = await _context.NewsLetter.FirstOrDefaultAsync<NewsLetter>(m => m.Id == item.NewsletterId);
                    motivosuscripcion = new MotivoSuscripcion
                    {
                        
                            NewsLetter = newsletter,
                            Suscripcion = suscripcion,
                            suscripcionId = suscripcion.Id,
                            
                            

                        };
                        suscripcion.motivoSuscripcion.Add(motivosuscripcion);


                           
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                suscripcionViewModel.Name = customer.Name;
                suscripcionViewModel.FirstSurname = customer.FirstSurname;
                suscripcionViewModel.SecondSurname = customer.SecondSurname;
                suscripcionViewModel.Marca = _context.Marca.Where(g => g.Nombre != null).ToList();
                suscripcionViewModel.Prendas = _context.Prenda.Where(g => g.Nombre != null).ToList();
                return View(suscripcionViewModel);
            }

            suscripcion.Cliente = customer;

            suscripcion.Titulo = suscripcionViewModel.Titulo;
            suscripcion.clienteID = customer.Id;
            suscripcion.FechaCaducidad = suscripcionViewModel.FechaCaducidad;
            suscripcion.motivo = suscripcionViewModel.Motivo;
            suscripcion.Descripcion = suscripcionViewModel.Descripcion;
            _context.Add(suscripcion);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = suscripcion.Id});
        }

        // GET: Suscripcions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            return View(suscripcion);
        }

        // POST: Suscripcions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Titulo,Descripcion,FechaCaducidad,clienteID")] Suscripcion suscripcion)
        {
            if (id != suscripcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.Id))
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
            return View(suscripcion);
        }

        // GET: Suscripcions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suscripcion = await _context.Suscripcion.FindAsync(id);
            _context.Suscripcion.Remove(suscripcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
            return _context.Suscripcion.Any(e => e.Id == id);
        }
    }
}
