using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public partial class Venta
    {
        [Key]      
        [Column("ID")]
        public int Id { get; set; }

        [Display(Name = "Ventas Totales")]

        public decimal? PrecioVenta { get; set; }

        [Display(Name = "Cantidad")]
        public int ? Cantidad {  get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Display(Name = "Fecha Venta")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaRegistro { get; set; }
        [Required(ErrorMessage = "Por favor, debe agregar al menos un vehículo ")]
        [MinLength(1, ErrorMessage = "Por favor, debe agregar al menos un vehículo")]
        public virtual List<VentaVehiculo> Vehiculos { get; set; }

        public Venta() 
        {

            Vehiculos = new List<VentaVehiculo>();
        }
    }
}
