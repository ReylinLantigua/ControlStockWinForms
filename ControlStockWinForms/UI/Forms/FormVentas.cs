using ControlStockWinForms.Data;
using ControlStockWinForms.Models;
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
    public partial class FormVentas : Form
    {
        private List<DetalleVenta> carrito = new List<DetalleVenta>();

        public FormVentas()
        {
            InitializeComponent();
        }

        private void RefrescarGrid()
        {
            dgvDetalles.DataSource = null;

            dgvDetalles.DataSource = carrito.Select(c => new
            {
                Producto = c.Producto.Nombre,
                Cantidad = c.Cantidad,
                Precio = c.Precio,
                Subtotal = c.Subtotal
            }).ToList();

            txtTotal.Text = $"Total: {carrito.Sum(c => c.Subtotal):C2}";
        }


        private void FormVentas_Load(object sender, EventArgs e)
        {
            var repo = new RepositorioProducto();

            cmdProductos.DataSource = repo.GetProductos().ToList();

            cmdProductos.DisplayMember = "Nombre";
            cmdProductos.ValueMember = "Id";

            nudCantidad.Minimum = 1;
            nudCantidad.Maximum = 100000;

            ConfigGrid();
            btnAgregar.BackColor = Color.FromArgb(52, 152, 219);
            btnGuardar.BackColor = Color.FromArgb(52, 152, 219);
            btnQuitar.BackColor = Color.FromArgb(52, 152, 219);

        }

        private void ConfigGrid()
        {
            dgvDetalles.ReadOnly = true;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AllowUserToDeleteRows = false;
            dgvDetalles.MultiSelect = false;
            dgvDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void cmdProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic prod = cmdProductos.SelectedItem;

            if(prod != null)
            {
                nudPrecio.Value = Convert.ToDecimal(prod.Precio);
                nudCantidad.Value = 1;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic prob = cmdProductos.SelectedItem;
                if (prob == null) return;

                int productoId = (int)prob.Id;
                string nombre = (string)prob.Nombre;
                int DstockDisponible = (int)prob.Cantidad;

                int cantidadSolicitada = (int)nudCantidad.Value;
                decimal precioUnitario = nudPrecio.Value;

                int enCarrito = carrito
                            .Where(c => c.ProductoId == productoId)
                            .Sum(c => c.Cantidad);

                int totalSolicitado = enCarrito + cantidadSolicitada;

                if (totalSolicitado > DstockDisponible)
                {
                    MessageBox.Show("No hay suficiente stock disponible.",
                                "Stock insuficiente",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    return;
                }

                var detalles = new DetalleVenta
                {
                    ProductoId = productoId,
                    Producto = new Producto { Nombre = nombre },
                    Cantidad = cantidadSolicitada,
                    Precio = precioUnitario
                };

                carrito.Add(detalles);

                RefrescarGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.CurrentRow != null)
            {
                int index = dgvDetalles.CurrentRow.Index;

                carrito.RemoveAt(index);

                RefrescarGrid();

            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar.",
                    "aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto al carrito.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var repoProd = new RepositorioProducto();
                var repoVenta = new RepositorioVenta(); 

                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = carrito.Sum(c => c.Cantidad * c.Precio),
                    Detalles = new List<DetalleVenta>()
                };

                foreach (var item in carrito)
                {
                    // Descontar stock
                    repoProd.DescontarStock(item.ProductoId, item.Cantidad);

                    // Agregar detalle a la venta
                    venta.Detalles.Add(new DetalleVenta
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio
                    });
                }

                repoVenta.GuardarVenta(venta);

                MessageBox.Show("Venta registrada correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                // Limpiar carrito
                carrito.Clear();
                dgvDetalles.DataSource = null;
                txtTotal.Text = "Total: $0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la venta: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
