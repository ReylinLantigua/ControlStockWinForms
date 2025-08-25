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
    public partial class FormProductos : Form
    {
        

        public FormProductos()
        {
            InitializeComponent();
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            loadData();
            Style();
            
        }

        private void Style()
        {
            panelHeader.BackColor = Color.FromArgb(0, 120, 215);
            panelBody.BackColor = Color.FromArgb(245, 245, 245);
            btnstyle(btnAgregar);
            btnstyle(btnEditar);
            btnstyle(btnEliminar);
            btnstyle(btnNew);
        }

        private void btnstyle(Button btn)
        {
            btn.BackColor = Color.FromArgb(40, 40, 40);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = BackColor = Color.FromArgb(0, 120, 215);
        }

        public void loadData()
        {
            var repo = new RepositorioProducto();
            dgvProductos.DataSource = repo.GetProductos();
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;    
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            flowBody.BackColor = Color.FromArgb(60, 60, 60);
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            nubPrecio.Value = 0;
            nudCantidad.Value = 0;
            nudStockMinimo.Value = 5;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
            try
            {
                
                
                    var repo = new RepositorioProducto();
                    var producto = new Producto
                    {
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text,
                        Precio = nubPrecio.Value,
                        Cantidad = (int)nudCantidad.Value,
                        StockMinimo = (int)nudStockMinimo.Value,
                        FechaIngreso = DateTime.Now
                    };

                    repo.AddProducto(producto);
                    MessageBox.Show("Producto agregado correctamente");
                    LimpiarCampos();





                
                loadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar/editar: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            

        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //lblTitular.Text = "Editar producto";
                    btnNew.Visible = true;
                    DataGridViewRow row = dgvProductos.Rows[e.RowIndex];

                    txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
                    txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString() ?? "";

                    nubPrecio.Value = row.Cells["Precio"].Value != null ?
                        Convert.ToDecimal(row.Cells["Precio"].Value) : 0;

                    nudCantidad.Value = row.Cells["Cantidad"].Value != null ?
                        Convert.ToDecimal(row.Cells["Cantidad"].Value) : 0;

                    nudStockMinimo.Value = row.Cells["StockMinimo"].Value != null ?
                        Convert.ToDecimal(row.Cells["StockMinimo"].Value) : 0;

                    btnEditar.Visible = true;
                    btnAgregar.Visible = false;
                    btnEliminar.Visible = true;
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //lblTitular.Text = "Agregar producto";
            btnAgregar.Visible = true; 
            btnEditar.Visible = false;
            btnEliminar.Visible = false;
            btnNew.Visible = false;
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Id"].Value);

                    DialogResult result = MessageBox.Show("¿Seguro que deseas eliminar este producto?",
                                                  "Confirmar eliminación",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        var repo = new RepositorioProducto();

                        repo.DeleteProducto(id);
                        btnEditar.Visible = false;
                        btnAgregar.Visible = true;
                        btnNew.Visible = false;
                        btnEliminar.Visible = false;
                        loadData();
                        LimpiarCampos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para eliminar.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {

                    if (dgvProductos.CurrentRow != null)
                    {
                        int id = (int)dgvProductos.CurrentRow.Cells["Id"].Value;
                        var repo = new RepositorioProducto();
                        var producto = new Producto
                        {
                            Id = id,
                            Nombre = txtNombre.Text,
                            Descripcion = txtDescripcion.Text,
                            Precio = nubPrecio.Value,
                            Cantidad = (int)nudCantidad.Value,
                            StockMinimo = (int)nudStockMinimo.Value
                        };

                        repo.EditProducto(producto);
                        //lblTitular.Text = "Agregar Producto";
                    btnAgregar.Visible = true;
                    btnEditar.Visible = false;
                        btnNew.Visible = false;

                        LimpiarCampos();
                        MessageBox.Show("Producto actualizado correctamente.",
                           "Éxito",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un producto para editar.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                loadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void panellateral_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCantidad_Click(object sender, EventArgs e)
        {

        }
    }
}
