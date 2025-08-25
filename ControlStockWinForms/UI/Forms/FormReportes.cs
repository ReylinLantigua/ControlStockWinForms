using ControlStockWinForms.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlStockWinForms.UI.Forms
{
    public partial class FormReportes : Form
    {
        public FormReportes()
        {
            InitializeComponent();
            CargarInventario();
        }

        private void CargarInventario()
        {
            var repo = new RepositorioProducto();
            var productos = repo.GetProductos()
                                .Select(p => new
                                {
                                    p.Nombre,
                                    p.Cantidad,
                                    p.Precio,
                                    Total = p.Cantidad * p.Precio
                                })
                                .ToList();

            dgvInventario.DataSource = productos;
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void btnGenerarVentas_Click(object sender, EventArgs e)
        {
            var repo = new RepositorioVenta();
            var ventas = repo.GetVentas()
                             .Where(v => v.Fecha >= dtInicio.Value && v.Fecha <= dtFin.Value)
                             .Select(v => new
                             {
                                 v.Id,
                                 v.Fecha,
                                 v.Total
                             })
                             .ToList();

            dgvVentas.DataSource = ventas;

            lblTotalVentas.Text = $"Ingresos: {ventas.Sum(v => v.Total):C2}";
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {

            var repoVenta = new RepositorioVenta();
            var ventas = repoVenta.GetVentas();
            repoVenta.ExportarVentasCSV(ventas);
            MessageBox.Show("Reporte exportado como CSV al escritorio");

        }
    }
}
