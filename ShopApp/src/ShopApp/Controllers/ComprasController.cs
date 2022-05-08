using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.PedidosViewModels;
using ShopApp.Models.PrendaViewModels;
using ShopApp.Models.CompraViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ShopApp.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            SelectPedidosForDevolucionViewModel selectPrendas = new SelectPedidosForDevolucionViewModel();
            selectPrendas.Compras = _context.Compra.Where(c => c.Cliente.UserName == User.Identity.Name).ToList();

            foreach (var item in selectPrendas.Compras)
            {
                ViewData[item.Id.ToString()] = _context.ItemCompra
                    .Where(compra => compra.CompraID == item.Id).Sum(cant => cant.Cantidad);
                               
            }
           
            return View(await _context.Compra.Where(c => c.Cliente.UserName == User.Identity.Name).ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Compra
                .Include(p => p.Cliente)
                .Include(p => p.ItemsCompra).ThenInclude(p => p.Prenda.Marca)
                .Include(p => p.ItemsCompra).ThenInclude(p => p.Prenda)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Compras/Create
        public IActionResult Create(SelectedPrendasForPurchaseViewModel selectedPrendas)
        {
            CompraCreateViewModel purchase = new();
            purchase.ItemsCompra = new List<ItemCompraViewModel>();

            if (selectedPrendas.IdsToAdd == null)
            {
                ModelState.AddModelError("PrendaNoSelected", "You should select at least a Prenda to be purchased, please");
            }
            else
                purchase.ItemsCompra = _context.Prenda.Include(prenda => prenda.Marca)
                    .Select(prenda => new ItemCompraViewModel()
                    {
                        PrendaID = prenda.PrendaID,
                        Marca = prenda.Marca.Nombre,
                        PrecioPrenda = prenda.PrecioPrenda,
                        Nombre = prenda.Nombre,
                        Cantidad = 1
                    })
                    .Where(prenda => selectedPrendas.IdsToAdd.Contains(prenda.PrendaID.ToString())).ToList();

            Cliente Cliente = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            purchase.Nombre = Cliente.Name;
            purchase.FirstSurname = Cliente.FirstSurname;
            purchase.SecondSurname = Cliente.SecondSurname;
           

            return View(purchase);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraCreateViewModel compraViewModel)
        {
            Prenda prenda; ItemCompra purchaseItem;
            Cliente customer;
            Compra purchase = new();
            purchase.PrecioTotal = 0;
            purchase.ItemsCompra = new List<ItemCompra>();
            customer = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));
               

            if (ModelState.IsValid)
            {
                foreach (ItemCompraViewModel item in compraViewModel.ItemsCompra)
                {
                    prenda = await _context.Prenda.FirstOrDefaultAsync<Prenda>(m => m.PrendaID == item.PrendaID);
                    
                    if (prenda.CantidadCompra < item.Cantidad)
                    {
                        ModelState.AddModelError("", $"No hay prendas suficientes llamadas: {prenda.Nombre}, por favor selecciona una prenda menor o igual que: {prenda.CantidadCompra}");
                    }
                    else
                    {
                        if (item.Cantidad > 0)
                        {
                            prenda.CantidadCompra -= item.Cantidad;
                            purchaseItem = new ItemCompra
                            {
                                Prenda = prenda,
                                Compra = purchase,
                                Cantidad = item.Cantidad
                            };
                            purchase.PrecioTotal += item.Cantidad * prenda.PrecioPrenda;
                            purchase.ItemsCompra.Add(purchaseItem);
                        } 
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                compraViewModel.Nombre = customer.Name;
                compraViewModel.FirstSurname = customer.FirstSurname;
                compraViewModel.SecondSurname = customer.SecondSurname;
                return View(compraViewModel);
            }


            purchase.Cliente = customer;
            purchase.FechaCompra = DateTime.Now;
            if (compraViewModel.MetodoPago == "PayPal")
                purchase.MetodoPago = new PayPal()
                {
                    Email = compraViewModel.Email,
                    Prefix = compraViewModel.Prefix,
                    Phone = compraViewModel.Phone
                };
            else
                purchase.MetodoPago = new TarjetaBancaria()
                {
                    CreditCardNumber = compraViewModel.CreditCardNumber,
                    CCV = compraViewModel.CCV,
                    ExpirationDate = (DateTime)compraViewModel.ExpirationDate
                };
            purchase.DireccionEnvio = compraViewModel.DireccionEnvio;
            _context.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = purchase.Id });
        }


        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrecioTotal,FechaCompra,DireccionEnvio,ClienteId")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.Id == id);
        }
        /*
       [HttpGet]
        [Authorize(Roles = "Cliente")]
        public IActionResult SelectPedidosforDevolucion()
        {
           
            SelectPedidosForDevolucionViewModel selectPrendas= new SelectPedidosForDevolucionViewModel();
            
            selectPrendas.Compras = _context.Compra
                .Include(nombre => nombre.ItemsCompra);

            foreach (var item in selectPrendas.Compras)
            {
                ViewData[item.Id.ToString()] = _context.ItemCompra
                    .Where(compra =>compra.CompraID == item.Id).Sum(cant => cant.Cantidad);
            }
            selectPrendas.Compras = selectPrendas.Compras.ToList();
            return View(selectPrendas);
            
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPedidosforDevolucion(SelectedPedidosForDevolucionViewModel selectedPedidos)
        {
            
            if(selectedPedidos.IdsToAdd != null)
            {
                return RedirectToAction("SelectPrendasForDevolucion", "ItemCompras", selectedPedidos);
            }
            
            return SelectPedidosforDevolucion();

        }
        */
       
    }
}
