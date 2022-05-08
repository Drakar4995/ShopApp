    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models.ItemCompraViewModels;
using ShopApp.Models;
using ShopApp.Models.DevolucionViewModels;

namespace ShopApp.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevolucionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Devolucion.Include(d => d.ItemsDevolucion).ThenInclude(d1=>d1.ItemCompra).ThenInclude(d2 =>d2.Compra).ToListAsync());
        }
    
        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion.Include(mp => mp.MetodoPago).Include(d=> d.ItemsDevolucion).ThenInclude(id =>id.ItemCompra).ThenInclude(ip=>ip.Prenda)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (devolucion == null)
            {
                return NotFound();
            } 

            return View(devolucion);
        }

        // GET: Devoluciones/Create
        public IActionResult Create(SelectedPrendasForDevolucionViewModel prendasSeleccionadas) //Mostrarlo en el View los datos
        {
            CreateDevolucionViewModel devolucion = new();
            devolucion.ItemsDevolucion = new List<ItemDevolucionViewModel>();
            if(prendasSeleccionadas.IdsToAdd == null)
            {
                ModelState.AddModelError("PrendaNoSelected", "Tienes que Seleccionar al menos una prenda a Devolver");
            }
             
            else
            {
                devolucion.ItemsDevolucion = _context.ItemCompra
                    .Include(item2 => item2.Prenda).ThenInclude(g => g.Marca).Include(compra => compra.Compra)
                    .Select(item => new ItemDevolucionViewModel()
                    {
                        ItemCompraID = item.Id,
                        nombrePrenda = item.Prenda.Nombre,
                        precio = item.Prenda.PrecioPrenda,
                        cantidad = item.Cantidad,
                        nombreMarca = item.Prenda.Marca.Nombre,
                        CompraId = item.CompraID
                        
                    })
                    .Where(item3 => prendasSeleccionadas.IdsToAdd.Contains(item3.ItemCompraID.ToString())).ToList();
                        
            
            }

            Cliente Customer = _context.Users.OfType<Cliente>().FirstOrDefault<Cliente>(u => u.UserName.Equals(User.Identity.Name));
            devolucion.Nombre = Customer.Name;
            devolucion.PrimerApellido = Customer.FirstSurname;
            devolucion.SegundoApellido = Customer.SecondSurname;
            
            return View(devolucion);
        }

        // POST: Devoluciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreateDevolucionViewModel createDevolucionViewModel){
            //asignar los valores de la devolucion
            Cliente cliente ;
            ItemDevolucion itemDevolucion;
            ItemCompra itemCompra;
            Devolucion devolucion = new();
            devolucion.precioTotal = 0;
            
            devolucion.ItemsDevolucion = new List<ItemDevolucion>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(u => u.UserName.Equals(User.Identity.Name));

            if(createDevolucionViewModel.tipoRecogida != null)
            {
                if (createDevolucionViewModel.tipoRecogida == "Correos")
                {
                    devolucion.tipoRecogida = "Correos";
                }
                else //(createDevolucionViewModel.tipoRecogida == "Domicilio")
                {
                    devolucion.tipoRecogida = "Domicilio";
                    devolucion.Direccion = createDevolucionViewModel.Direccion;
                }
                //else { ModelState.AddModelError("", $"Metodo no valido"); }
            }
            else { ModelState.AddModelError("", $"Seleccione un metodo de Recogida"); }

            if (ModelState.IsValid)
            {
                foreach (ItemDevolucionViewModel item in createDevolucionViewModel.ItemsDevolucion)
                {
                    
                    itemCompra = await _context.ItemCompra.Include(item => item.Compra).Include(item => item.Prenda).FirstOrDefaultAsync<ItemCompra>(m => m.Id == item.ItemCompraID); 
                    //Anadir condicional                                                                                                                       
                    //Hago un ThenInclude no esta en la misma Tabla                                       
                    itemDevolucion = new ItemDevolucion //Cada uno de los itemsDevolucion
                    {
                        ItemCompraID = itemCompra.Id,
                        ItemCompra = itemCompra,
                        Devolucion = devolucion,
                        DevolucionID = devolucion.ID,
                        MotivoDevolucion = item.motivoDevolucion
                    };
                    devolucion.precioTotal += itemCompra.Cantidad * itemCompra.Prenda.PrecioPrenda;
                    devolucion.ItemsDevolucion.Add(itemDevolucion);
                }               
                
            }
            if (ModelState.ErrorCount > 0)
            {
                createDevolucionViewModel.Nombre = cliente.Name;
                createDevolucionViewModel.PrimerApellido = cliente.FirstSurname;
                createDevolucionViewModel.SegundoApellido = cliente.SecondSurname;
                
                return View(createDevolucionViewModel);
            }

            if (createDevolucionViewModel.PaymentMethod == "PayPal")
            {
                devolucion.MetodoPago = new PayPal()
                {
                    Email = createDevolucionViewModel.Email,
                    Prefix = createDevolucionViewModel.Prefix,
                    Phone = createDevolucionViewModel.Phone
                };

            }
            else devolucion.MetodoPago = new TarjetaBancaria()
            {
                CreditCardNumber = createDevolucionViewModel.CreditCardNumber,
                CCV = createDevolucionViewModel.CCV,
                ExpirationDate = (DateTime)createDevolucionViewModel.ExpirationDate
            };

            

            devolucion.MetodoDevolucion = createDevolucionViewModel.PaymentMethod ; //Tipo de Pago Paypal o Tarjeta de Credito
            devolucion.cliente = cliente;
            devolucion.clienteId = cliente.Id;
            devolucion.FechaDevolucion = DateTime.Now;
            _context.Add(devolucion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = devolucion.ID});

        }
        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }
            return View(devolucion);
        }

        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FechaDevolucion,MetodoDevolucion")] Devolucion devolucion)
        {
            if (id != devolucion.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.ID))
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
            return View(devolucion);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucion
                .FirstOrDefaultAsync(m => m.ID == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devolucion = await _context.Devolucion.FindAsync(id);
            _context.Devolucion.Remove(devolucion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
            return _context.Devolucion.Any(e => e.ID == id);
        }
    }
}
