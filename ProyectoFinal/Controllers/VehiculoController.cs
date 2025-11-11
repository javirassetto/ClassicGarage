using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Controllers
{
    [Authorize]
    public class VehiculoController : Controller
    {
        private readonly AutoCarDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;//OBTIENE LOS DATOS DE NUESTRO SERVIDOR PARA SER USADO

        public VehiculoController(AutoCarDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Vehiculo
        //public async Task<IActionResult> Index()
        //{
        //    var autoCarDBcontext = _context.Vehiculos.Include(v => v.Categoria).Include(v => v.Marca).Include(v => v.Modelo).Include(v => v.Tipo);

        //    return View(await autoCarDBcontext.ToListAsync());
        //}

        //ORDENADOS
        [HttpGet]
        public JsonResult GetModelosByMarca(int marcaId)
        {
            var modelos = _context.Modelos
                .Where(m => m.MarcaId == marcaId)
                .Select(m => new { id = m.Id, descripcion = m.Descripcion })
                .ToList();

            return Json(modelos);
        }

        public async Task<IActionResult> Index()
        {
            var vehiculosOrdenados = await _context.Vehiculos
                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .Include(v => v.Tipo)
                .Where(v => v.Disponible) //solo disponibles
                .OrderBy(v => v.Marca.Descripcion)  // Ordenar por fecha                      
                .ThenBy(v =>v.FechaRegistro)// Ordenar por marca
                .ToListAsync();

            return View(vehiculosOrdenados);
        }

        //Buscar usados
        public IActionResult FiltrarPorCategoriaUsados()
        {
            var vehiculosFiltrados = _context.Vehiculos

                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .Include(v => v.Tipo)
                .Where(v => v.Categoria.Descripcion == "Usado")
                .Where(v => v.Disponible) //solo disponibles
                .OrderBy(v => v.FechaRegistro)  // Ordenar por fecha                      
                .ThenBy(v => v.Marca.Descripcion)// Ordenar por marca
                .ToList();

            return View("_ListaVehiculos", vehiculosFiltrados);
        }
        //Buscar 0km
        public IActionResult FiltrarPorCategoriaNuevos()
        {
            var vehiculosFiltrados = _context.Vehiculos

                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .Include(v => v.Tipo)
                .Where(v => v.Categoria.Descripcion == "0km Nuevo")
                .Where(v => v.Disponible) //solo disponibles
                .OrderBy(v => v.Marca.Descripcion)
                .ToList();

            return View("_ListaVehiculos", vehiculosFiltrados);
        }




        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vehiculos == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .Include(v => v.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculo/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion");
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion");
            ViewData["ModeloId"] = new SelectList(_context.Modelos, "Id", "Descripcion");
            ViewData["TipoId"] = new SelectList(_context.Tipos, "Id", "Descripcion");
            return View();
        }

        // POST: Vehiculo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        
        public async Task<IActionResult> Create(VehiculoViewModels model)
        {
            string uniqueFileName = UploadedFile(model);

            if (ModelState.IsValid)
            {
                Vehiculo vehiculo = new Vehiculo()
                {
                    ImagenVehiculo=uniqueFileName,
                    MarcaId=model.MarcaId,
                    ModeloId=model.ModeloId,
                    TipoId=model.TipoId,
                    CategoriaId=model.CategoriaId,
                    ano=model.ano,
                    Precio=Convert.ToDecimal(model.Precio),
                    FechaRegistro=model.FechaRegistro,
                    
                   
                };
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", model.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", model.MarcaId);
            ViewData["ModeloId"] = new SelectList(_context.Modelos, "Id", "Descripcion", model.ModeloId);
            ViewData["TipoId"] = new SelectList(_context.Tipos, "Id", "Descripcion", model.TipoId);
            return View(model);
        }

        private string UploadedFile(VehiculoViewModels model)
        {
            string uniqueFileName = string.Empty;

            if (model.Imagen != null)
            {
                //
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\vehiculos");
                //GENERA UN NOMBRE ALEATORIO PARA LAS FOTOS
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Imagen.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Imagen.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vehiculos == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);

            VehiculoViewModels vehiculoViewModels = new VehiculoViewModels()
            {
                ImagenVehiculo=vehiculo.ImagenVehiculo,
                MarcaId = vehiculo.MarcaId,
                ModeloId = vehiculo.ModeloId,
                TipoId = vehiculo.TipoId,
                CategoriaId = vehiculo.CategoriaId,
                ano = vehiculo.ano,                
                Precio = vehiculo.Precio.ToString(),    
                FechaRegistro = vehiculo.FechaRegistro,

            };

            if (vehiculo == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", vehiculo.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", vehiculo.MarcaId);
            ViewData["ModeloId"] = new SelectList(_context.Modelos, "Id", "Descripcion", vehiculo.ModeloId);
            ViewData["TipoId"] = new SelectList(_context.Tipos, "Id", "Descripcion", vehiculo.TipoId);
            return View(vehiculoViewModels);
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehiculoViewModels model)
        {
            
           
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehiculo = await _context.Vehiculos.FindAsync(id);
                    if (vehiculo == null) {
                        return NotFound();
                    }
                    string uniqueFileName = UploadedFile(model);
                    // Si no se subió nueva imagen, mantener la actual
                    if (string.IsNullOrEmpty(uniqueFileName))
                    {
                        uniqueFileName = vehiculo.ImagenVehiculo;
                         
                    }

                    vehiculo.ImagenVehiculo = uniqueFileName;
                    vehiculo.MarcaId = model.MarcaId;
                    vehiculo.ModeloId = model.ModeloId;
                    vehiculo.TipoId = model.TipoId;
                    vehiculo.CategoriaId = model.CategoriaId;
                    vehiculo.ano = model.ano;
                    vehiculo.Precio = Convert.ToDecimal(model.Precio);                        
                    vehiculo.FechaRegistro = model.FechaRegistro;

                    

                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(model.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", model.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", model.MarcaId);
            ViewData["ModeloId"] = new SelectList(_context.Modelos, "Id", "Descripcion", model.ModeloId);
            ViewData["TipoId"] = new SelectList(_context.Tipos, "Id", "Descripcion", model.TipoId);
            return View(model);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vehiculos == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.Modelo)
                .Include(v => v.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vehiculos == null)
            {
                return Problem("Entity set 'AutoCarDBcontext.Vehiculos'  is null.");
            }
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(int id)
        {
          return (_context.Vehiculos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
