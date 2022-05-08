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
using Microsoft.AspNetCore.Authorization;


namespace ShopApp.Controllers
{
    public class NewsLettersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsLettersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NewsLetters
        public async Task<IActionResult> Index(String SearchString)
        {
            var applicationDbContext = _context.NewsLetter.Include(n => n.Categoria)
            .Where(s => s.Titulo.Contains(SearchString)).
             OrderBy(m => m.Titulo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NewsLetters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetter = await _context.NewsLetter
                .Include(n => n.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsLetter == null)
            {
                return NotFound();
            }
            
            return View(newsLetter);
        }

        // GET: NewsLetters/Create
        [Authorize(Roles = "Cliente")]
        public IActionResult Create()
        {
            ViewData["CategoriaNombre"] = new SelectList(_context.Set<Categoria>(), "Nombre", "Nombre");
            return View();
        }

        // POST: NewsLetters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,marcaNombre,CategoriaNombre")] NewsLetter newsLetter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsLetter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaNombre"] = new SelectList(_context.Set<Categoria>(), "Nombre", "Nombre", newsLetter.Categoria.Nombre);
            return View(newsLetter);
        }

        // GET: NewsLetters/Edit/5 
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetter = await _context.NewsLetter.FindAsync(id);
            if (newsLetter == null)
            {
                return NotFound();
            }
            ViewData["CategoriaNombre"] = new SelectList(_context.Set<Categoria>(), "Nombre", "Nombre", newsLetter.Categoria.Nombre);
            return View(newsLetter);
        }

        // POST: NewsLetters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,marcaNombre,CategoriaNombre")] NewsLetter newsLetter)
        {
            if (id != newsLetter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsLetter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsLetterExists(newsLetter.Id))
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
            ViewData["CategoriaNombre"] = new SelectList(_context.Set<Categoria>(), "Nombre", "Nombre", newsLetter.Categoria.Nombre);
            return View(newsLetter);
        }

        // GET: NewsLetters/Delete/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsLetter = await _context.NewsLetter
                .Include(n => n.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsLetter == null)
            {
                return NotFound();
            }

            return View(newsLetter);
        }

        // POST: NewsLetters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsLetter = await _context.NewsLetter.FindAsync(id);
            _context.NewsLetter.Remove(newsLetter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsLetterExists(int id)
        {
            return _context.NewsLetter.Any(e => e.Id == id);
        }
        // GET: Newsletter/SelectNewsletterForSuscribe
        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public IActionResult SelectNewsletterForSuscribe(string newsletterCategoria, string newslettermarcaselected)
        {
            SelectNewsletterForSuscribeViewModel selectNewsletter = new SelectNewsletterForSuscribeViewModel();
            selectNewsletter.marcaNombres = new SelectList(_context.Marca.Select(g => g.Nombre).ToList());
            selectNewsletter.Newsletters = _context.NewsLetter
                .Include(m =>m.Marca)
                .Include(m => m.Categoria) //join table Movie and table Genre
                .Where(newsletter=> (newsletter.Categoria.Nombre.Contains(newsletterCategoria) || newsletterCategoria == null)
                && (newsletter.Marca.Nombre.Contains(newslettermarcaselected) || newslettermarcaselected == null));

            selectNewsletter.Newsletters = selectNewsletter.Newsletters.ToList();
            if(newsletterCategoria != null || newslettermarcaselected != null)
            {
                //ModelState.AddModelError(string.Empty ,"Filtros aplicados correctamente");
                ViewBag.Mesaje = "Filtros aplicados correctamente";

               
            }
            return View(selectNewsletter);
        }

        
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public IActionResult SelectNewsletterForSuscribe(SelectedNewsletterForSuscribeViewModel selectedNewslettter)
        {
            if (selectedNewslettter.IdsToAdd != null)
            {
                
                return RedirectToAction("Create", "Suscripcions", selectedNewslettter);
                
            }
            //a message error will be shown to the customer in case no movies are selectedq
            ModelState.AddModelError(string.Empty, "You must select at least one newsletter");

            //the View SelectMoviesForPurchase will be shown again
            return SelectNewsletterForSuscribe(selectedNewslettter.newsletterCategoria, selectedNewslettter.newslettermarcaselected);


        }
    }
}
