using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using System.Linq;
using System.Security.Cryptography;

namespace Rocosa.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria)
                                                      .Include(t => t.TipoAplicacion);
            return View(lista);
        }

        public IActionResult Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString(),
                }),
                TipoAplicacionLista = _db.TipoAplicacion.Select(d => new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString()
                })
            };


            if (id == null)
            {
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = _db.Producto.Find(id);

                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id == 0)
                {
                    // crear
                    string upload = webRootPath + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    _db.Producto.Add(productoVM.Producto);
                }
                else
                {
                    // actualizar
                }
            }
            return View(productoVM);

        }
    }
}
