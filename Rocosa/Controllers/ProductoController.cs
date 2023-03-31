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
        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria)
                                                      .Include(t => t.TipoAplicacion);
            return View(lista);
        }

        public IActionResult Upsert(int? id) 
        {
            //IEnumerable<SelectListItem> categoriaDropDown = _db.Categoria.Select(c => new SelectListItem 
            //{
            //    Text = c.NombreCategoria,
            //    Value = c.Id.ToString()          
            //});

            //ViewBag.categoriaDropDown = categoriaDropDown;

            //Producto producto = new Producto();


            ProductoVM productoVM = new ProductoVM() 
            {
              Producto =new Producto(),
              CategoriaLista = _db.Categoria.Select(c => new SelectListItem 
              { 
                 Text = c.NombreCategoria,
                 Value = c.Id.ToString(),        
              }),
              TipoAplicacionLista =  _db.TipoAplicacion.Select(d => new SelectListItem
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
    }
}
