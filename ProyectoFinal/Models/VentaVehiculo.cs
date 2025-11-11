using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class VentaVehiculo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
       
        public int VehiculoId { get; set; }
  
        public virtual Vehiculo? Vehiculo { get; set; }
        
        public int VentaId {  get; set; }
       
        public virtual Venta? Venta { get; set; }

        public decimal? PrecioDetalle { get; set; }

        public int? Orden {  get; set; }

    }
}
