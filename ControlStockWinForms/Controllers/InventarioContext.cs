using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ControlStockWinForms.Models;

namespace ControlStockWinForms.Data
{
    class InventarioContext : DbContext
    {
        public InventarioContext() : base("name=InventarioDB") { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas{ get; set; }
    }
}
