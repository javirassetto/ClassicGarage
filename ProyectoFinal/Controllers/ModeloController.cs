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
    public class ModeloController : Controller
    {
        private readonly AutoCarDBcontext _context;

        public ModeloController(AutoCarDBcontext context)
        {
            _context = context;
        }

        // GET: Modelo
        public async Task<IActionResult> Index()
        {
            //return _context.Modelos != null ? 
            //            View(await _context.Modelos.ToListAsync()) :
            //            Problem("Entity set 'AutoCarDBcontext.Modelos'  is null.");
            //var autoCarDBcontext = _context.Modelos.Include(m => m.Marca);
            //return View(await autoCarDBcontext.ToListAsync());
            var modelosOrdenados = await _context.Modelos
              .Include(m => m.Marca)
              .OrderBy(v => v.Marca.Descripcion)  // Ordenar por MARCA                      
              .ThenBy(v => v.FechaRegistro)// Ordenar por FECHA
              .ToListAsync();

            return View(modelosOrdenados);
        }

        // GET: Modelo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Modelos == null)
            {
                return NotFound();
            }

            var modelo = await _context.Modelos
                .Include(m => m.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        // GET: Modelo/Create
        public IActionResult Create()
        {
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion");
            return View();
        }

        // POST: Modelo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MarcaId,Descripcion,FechaRegistro")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modelo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Modelos == null)
            {
                return NotFound();
            }

            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", modelo.MarcaId);
            return View(modelo);
        }

        // POST: Modelo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MarcaId,Descripcion,FechaRegistro")] Modelo modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeloExists(modelo.Id))
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
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", modelo.MarcaId);
            return View(modelo);
        }

        // GET: Modelo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Modelos == null)
            {
                return NotFound();
            }

            var modelo = await _context.Modelos
                 .Include(m => m.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        // POST: Modelo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Modelos == null)
            {
                return Problem("Entity set 'AutoCarDBcontext.Modelos'  is null.");
            }
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo != null)
            {
                _context.Modelos.Remove(modelo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeloExists(int id)
        {
          return (_context.Modelos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
