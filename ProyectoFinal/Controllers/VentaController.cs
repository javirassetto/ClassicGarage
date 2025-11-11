using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly AutoCarDBcontext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;//OBTIENE LOS DATOS DE NUESTRO SERVIDOR PARA SER USADO

        public VentaController(AutoCarDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Venta
        public async Task<IActionResult> Index()
        {           
            var ventas = await _context.Ventas
            .Include(v => v.Vehiculos)
                .ThenInclude(vv => vv.Vehiculo)
                .ThenInclude(v => v.Marca)
            .Include(v => v.Vehiculos)
                .ThenInclude(vv => vv.Vehiculo)
                    .ThenInclude(v => v.Modelo)
            .OrderBy(v => v.FechaRegistro)
            .ToListAsync();

            foreach (var venta in ventas)
            {
                if (venta.Vehiculos != null && venta.Vehiculos.Any())
                {
                    decimal ventaTotales = Convert.ToDecimal(venta.Vehiculos.Sum(vv => vv.Vehiculo.Precio));
                  
                    venta.PrecioVenta = ventaTotales;
                }
            }

            return View(ventas);
        }

        // GET: Venta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }
            var model = new Venta();
            model.Vehiculos.Add(new VentaVehiculo());
                               
            var venta = await _context.Ventas
                .Include(v => v.Vehiculos)
                .ThenInclude(vv => vv.Vehiculo)
                .ThenInclude(vm => vm.Marca)
                .ThenInclude(vm => vm.Modelos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        //Calcular el precio sumado de todos los vehiculos
        public async Task<IActionResult> CalcularPrecioTotal(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }
            
            var venta = await _context.Ventas
                .Include(v => v.Vehiculos)
                .ThenInclude(vv => vv.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta != null)
            {
                decimal ventaTotales = Convert.ToDecimal(venta.Vehiculos.Sum(vv => vv.Vehiculo.Precio));
                venta.PrecioVenta=ventaTotales;
            }
            else
            {
                return NotFound();
            }

            return View(venta);
        }


        // GET: Venta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrecioVenta,Cantidad,FechaRegistro,Vehiculos")] Venta venta)
        {
            if (ModelState.IsValid)
            {
               // venta.FechaRegistro=DateTime.Now;   // si quisier asigno la fecha del dia que creo la venta
                _context.Add(venta);

                // marcar los vehículos como no disponibles
                foreach (var vv in venta.Vehiculos)
                {
                    var vehiculo = await _context.Vehiculos.FindAsync(vv.VehiculoId);
                    if (vehiculo != null)
                    {
                        vehiculo.Disponible = false;
                        _context.Update(vehiculo);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }


        //cargar Venta

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVentaVehiculo(Venta venta)
        {
            venta.Vehiculos.Add(new VentaVehiculo());

            var ventas = _context.Vehiculos
               .Include(v => v.Marca)
               .Include(v=> v.Modelo)
               .OrderBy(v => v.Marca.Descripcion)//ver
               .Where(v => v.Disponible)
               .Select(x => new
               {
                   x.Id,
                   DescVehiculoPrecio = $"{x.Marca.Descripcion} - {x.Modelo.Descripcion} - {x.ano} - ${x.Precio}"
               })
               .ToList();

            ViewData["VehiculoId"] = new SelectList(ventas, "Id", "DescVehiculoPrecio");

            return PartialView("VentaVehiculo", venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditVentaVehiculo(Venta venta)
        {
            venta.Vehiculos.Add(new VentaVehiculo());

            var ventas = _context.Vehiculos
               .Include(v => v.Marca)
               .Include(v => v.Modelo)
               .Where(v => v.Disponible)
               .Select(x => new
               {
                   x.Id,
                   DescVehiculoPrecio = $"{x.Marca.Descripcion} - {x.Modelo.Descripcion} - {x.ano} - ${x.Precio}"
               })
               .ToList();

            ViewData["VehiculoId"] = new SelectList(ventas, "Id", "DescVehiculoPrecio");

            return PartialView("VentaVehiculoEdit", venta);//ver
        }

        // GET: Venta/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Ventas == null)
        //    {
        //        return NotFound();
        //    }

        //    var venta = await _context.Ventas
        //        .Include(v => v.Vehiculos)
        //        .ThenInclude(vv => vv.Vehiculo)
        //        .ThenInclude(v => v.Marca)
        //        .Include(v => v.Vehiculos)
        //        .ThenInclude(vv => vv.Vehiculo)
        //        .ThenInclude(v => v.Modelo)
        //        .FirstOrDefaultAsync(m => m.Id == id);

        //    if (venta == null)
        //    {
        //        return NotFound();
        //    }

        //    // Arma lista de vehículos para el dropdown
        //    var vehiculos = _context.Vehiculos
        //        .Include(v => v.Marca)
        //        .Include(v => v.Modelo)
        //        .Select(x => new
        //        {
        //            x.Id,
        //            DescVehiculoPrecio = $"{x.Marca.Descripcion} - {x.Modelo.Descripcion} - ${x.Precio}"
        //        }).ToList();

        //    ViewData["VehiculoId"] = new SelectList(vehiculos, "Id", "DescVehiculoPrecio");

        //    return View(venta);
        //}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
 
            var venta = await _context.Ventas
                .Include(v => v.Vehiculos)
                .ThenInclude(vv => vv.Vehiculo)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            //obtengo los vehiculos ya asociados a la venta
            var vehiculosDeLaVenta = venta.Vehiculos
            .Select(vv => vv.Vehiculo.Id)
            .ToList();

            // Lista de vehículos
            var vehiculos = _context.Vehiculos
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .OrderBy(v => v.Marca.Descripcion)//ver
                .Where(v => v.Disponible || vehiculosDeLaVenta.Contains(v.Id))
                .Select(x => new {
                    x.Id,
                    DescVehiculoPrecio = $"{x.Marca.Descripcion} - {x.Modelo.Descripcion} - {x.ano} - ${x.Precio}"
                }).ToList();

            ViewData["VehiculoId"] = new SelectList(vehiculos, "Id", "DescVehiculoPrecio", vehiculosDeLaVenta.FirstOrDefault());

            return View(venta);
        }



        // POST: Venta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,PrecioVenta,Cantidad,FechaRegistro,Vehiculos")] Venta venta)//ver
        //{
        //    if (id != venta.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(venta);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!VentaExists(venta.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(venta);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Venta venta)
        {
            var ventaEnDb = await _context.Ventas
                .Include(v => v.Vehiculos)
                .FirstOrDefaultAsync(v => v.Id == venta.Id);

            if (id != venta.Id)
            {
               return NotFound();
            }
                if (ventaEnDb == null) return NotFound();

            ventaEnDb.FechaRegistro = venta.FechaRegistro;

            // Recorro los vehículos que vinieron del form
            for (int i = 0; i < venta.Vehiculos.Count; i++)
            {
                var vvEditado = venta.Vehiculos[i];
                var vvEnDb = ventaEnDb.Vehiculos.FirstOrDefault(v => v.Id == vvEditado.Id);
                if (vvEnDb != null)
                {
                    vvEnDb.VehiculoId = vvEditado.VehiculoId;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Venta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
           .Include(v => v.Vehiculos)
           .ThenInclude(vv => vv.Vehiculo)
           .ThenInclude(vm => vm.Marca)
           .ThenInclude(vm => vm.Modelos)
           .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ventas == null)
            {
                return Problem("Entity set 'AutoCarDBcontext.Ventas'  is null.");
            }
            var venta = await _context.Ventas
           .Include(v => v.Vehiculos)
           
           .FirstOrDefaultAsync(m => m.Id == id);
            if (venta != null)
            {
                // volver a marcar disponibles
                foreach (var vv in venta.Vehiculos)
                {
                    var vehiculo = await _context.Vehiculos.FindAsync(vv.VehiculoId);
                    if (vehiculo != null)
                    {
                        vehiculo.Disponible = true;
                        _context.Update(vehiculo);
                    }
                }
                _context.Ventas.Remove(venta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
          return (_context.Ventas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
