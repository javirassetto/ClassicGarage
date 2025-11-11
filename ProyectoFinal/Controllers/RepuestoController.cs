using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinal.Controllers
{
    [Authorize]
    public class RepuestoController : Controller
    {
        private readonly AutoCarDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;//OBTIENE LOS DATOS DE NUESTRO SERVIDOR PARA SER USADO

        public RepuestoController(AutoCarDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Repuesto
        public async Task<IActionResult> Index()
        {
            var autoCarDBcontext = _context.Repuestos.Include(r => r.Marca);
            return View(await autoCarDBcontext.ToListAsync());
        }

        // GET: Repuesto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }

            var repuesto = await _context.Repuestos
                .Include(r => r.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }

        // GET: Repuesto/Create
        public IActionResult Create()
        {
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion");
            return View();
        }

        // POST: Repuesto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,MarcaId,Costo,Cantidad,FechaRegistro")] Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                

                _context.Add(repuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", repuesto.MarcaId);
            return View(repuesto);
        }

        // GET: Repuesto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }        

            var repuesto = await _context.Repuestos.FindAsync(id);            

            if (repuesto == null)
            {
                return NotFound();
            }
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", repuesto.MarcaId);
            return View(repuesto);
        }

        // POST: Repuesto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,MarcaId,Costo,Cantidad,FechaRegistro")] Repuesto repuesto)
        {
            if (id != repuesto.Id)
            {
                return NotFound();
            }
                        

            if (ModelState.IsValid)
            {
                try
                {             
                   
                    _context.Update(repuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepuestoExists(repuesto.Id))
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
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Descripcion", repuesto.MarcaId);
            return View(repuesto);
        }

        // GET: Repuesto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Repuestos == null)
            {
                return NotFound();
            }

            var repuesto = await _context.Repuestos
                .Include(r => r.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repuesto == null)
            {
                return NotFound();
            }

            return View(repuesto);
        }

        // POST: Repuesto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Repuestos == null)
            {
                return Problem("Entity set 'AutoCarDBcontext.Repuestos'  is null.");
            }
            var repuesto = await _context.Repuestos.FindAsync(id);
            if (repuesto != null)
            {
                _context.Repuestos.Remove(repuesto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepuestoExists(int id)
        {
          return (_context.Repuestos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //*************************
        //Vista Importar Lista*****
        public IActionResult ImportarRepuestos()
        {

            return View();
        }


        [HttpPost, ActionName("MostrarDatos")]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
        {
            if (ArchivoExcel != null)
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
                {
                    MiExcel = new XSSFWorkbook(stream);
                }
                else
                {
                    MiExcel = new HSSFWorkbook(stream);
                }

                ISheet HojaExcel = MiExcel.GetSheetAt(0);

                int cantidadFilas = HojaExcel.LastRowNum;

                List<Repuesto> lista = new List<Repuesto>();

                for (int i = 1; i <= cantidadFilas; i++)
                {

                    IRow fila = HojaExcel.GetRow(i);

                    lista.Add(new Repuesto
                    {

                        Nombre = fila.GetCell(0).ToString(),
                        MarcaId = Int16.Parse(fila.GetCell(1).ToString()),
                        Costo = decimal.Parse(fila.GetCell(2).ToString()),
                        Cantidad = Int16.Parse(fila.GetCell(3).ToString()),
                        FechaRegistro = DateTime.Now,

                    });
                }

                return StatusCode(StatusCodes.Status200OK, lista);
            }
            else
            {

                return View();
            }

        }

        [HttpPost, ActionName("EnviarDatos")]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            if (ArchivoExcel != null)
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
                {
                    MiExcel = new XSSFWorkbook(stream);
                }
                else
                {
                    MiExcel = new HSSFWorkbook(stream);
                }

                ISheet HojaExcel = MiExcel.GetSheetAt(0);

                int cantidadFilas = HojaExcel.LastRowNum;
                List<Repuesto> lista = new List<Repuesto>();

                for (int i = 1; i <= cantidadFilas; i++)
                {

                    IRow fila = HojaExcel.GetRow(i);

                    lista.Add(new Repuesto
                    {
                        //*****
                        Nombre = fila.GetCell(0).ToString(),
                        MarcaId = Int16.Parse(fila.GetCell(1).ToString()),
                        Costo = decimal.Parse(fila.GetCell(2).ToString()),
                        Cantidad = Int16.Parse(fila.GetCell(3).ToString()),
                        FechaRegistro = DateTime.Now,


                    });
                }

                _context.BulkInsert(lista);

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            else
            {
                return View();
            }

        }

        //Descarga archivo Excel
        public IActionResult DownloadFile()
        {
            var filepath = Path.Combine(_webHostEnvironment.WebRootPath, "archivos", "ListaDeRepuestos.xlsx");

            if (!System.IO.File.Exists(filepath))
            {
                return NotFound(); 
            }

            return File(System.IO.File.ReadAllBytes(filepath), "application/vnd.ms-excel", System.IO.Path.GetFileName(filepath));
           
        }
    }
}
