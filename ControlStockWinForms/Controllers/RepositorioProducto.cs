using ControlStockWinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ControlStockWinForms.Data
{
    public class RepositorioProducto
    {
        public List<Producto> GetProductos() // Trae la lista de productos de la db
        {
            using(var db = new InventarioContext())
            {
                return db.Productos.ToList();
            }
        }

        public void AddProducto(Producto p) //Agregar produtos 
        {
            using(var db= new InventarioContext())
            {
                db.Productos.Add(p);
                db.SaveChanges();
            }
        }

        public void EditProducto(Producto p) //Editar Productos 
        {
            using (var db = new InventarioContext())
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteProducto(int id) //Eliminar producto
        {
            using (var db = new InventarioContext())
            {
                var prod = db.Productos.Find(id);

                if (prod != null)
                {
                    db.Productos.Remove(prod);
                    db.SaveChanges();
                }
            }
        }

        public void DescontarStock(int productoId, int cantidad)
        {
            using (var db = new InventarioContext())
            {
                var producto = db.Productos.Find(productoId);
                if (producto == null)
                    throw new Exception("Producto no encontrado.");

                if (producto.Cantidad < cantidad)
                    throw new Exception($"Stock insuficiente para {producto.Nombre}.");

                producto.Cantidad -= cantidad;

                db.SaveChanges();
            }
        }

    }
}
