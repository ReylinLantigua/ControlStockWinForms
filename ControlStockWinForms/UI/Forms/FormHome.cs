using ControlStockWinForms.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlStockWinForms
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
            Estilo();
            
        }

        private void EstiloBoton(Button btn)
        {
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 120, 215);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);
            btn.BackColor = Color.FromArgb(60, 60, 60);
        }

        private void Estilopanel()
        {
            panelTop.BackColor= Color.FromArgb(0, 120, 215);
            panelContenedor.BackColor= Color.FromArgb(245, 245, 245);
            panelMenu.BackColor = Color.FromArgb(60, 60, 60);
        }

        private void Estilo()
        {
            EstiloBoton(btnProductos);
            EstiloBoton(btnVentas);
            EstiloBoton(btnReportes);

            Estilopanel();

        }

        public void Abrirform(Form form)
        {
            panelContenedor.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContenedor.Controls.Add(form);
            form.Show();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            Abrirform(new FormProductos());
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            Abrirform(new FormVentas());
        }


        private void btnReportes_Click(object sender, EventArgs e)
        {
            Abrirform(new FormReportes());
        }

        private void FormHome_Load(object sender, EventArgs e)
        {

        }
    }
}
