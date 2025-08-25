using ControlStockWinForms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlStockWinForms.Data
{
    class RepositorioVenta
    {
        private readonly InventarioContext _context;

        public RepositorioVenta()
        {
            _context = new InventarioContext();
        }

        public void GuardarVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public List<Venta> GetVentasPorRango(DateTime inicio, DateTime fin)
        {
            return _context.Ventas
                           .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
                           .ToList();
        }

        public decimal GetTotalVentas(DateTime inicio, DateTime fin)
        {
            return _context.Ventas
                           .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
                           .Sum(v => (decimal?)v.Total) ?? 0;
        }

        public List<Venta> GetVentas()
        {
            using (var db = new InventarioContext())
            {
                return db.Ventas.Include("Detalles").ToList();
            }
        }

        public void ExportarVentasCSV(List<Venta> ventas)
        {
            string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            
            string nombreArchivo = "ventas.csv";

            
            string rutaArchivo = Path.Combine(escritorio, nombreArchivo);

            var sb = new StringBuilder();
            sb.AppendLine("Id,Fecha,Total");

            foreach (var v in ventas)
            {
                sb.AppendLine($"{v.Id},{v.Fecha},{v.Total}");
            }

            File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);

            System.Windows.Forms.MessageBox.Show($"Archivo guardado en: {rutaArchivo}",
                                                "Exportación Exitosa",
                                                System.Windows.Forms.MessageBoxButtons.OK,
                                                System.Windows.Forms.MessageBoxIcon.Information);
        }

    }
}
