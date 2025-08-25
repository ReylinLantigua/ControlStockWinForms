using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlStockWinForms.Models
{
    //Modelo de ventas
    [Table("Ventas")]
    public class Venta
    {
        [Key]
        public int Id { get; set; }  

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public decimal Total { get; set; }


        public virtual ICollection<DetalleVenta> Detalles { get; set; }
    }
}
