using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal.Models
{
    public partial class Repuesto
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un nombre del repuesto ")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Por favor, debe ingresar una marca ")]
        [Display(Name = "Marca")]
        public int? MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        
        public virtual Marca? Marca { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un costo ")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
        [Display(Name = "Costo")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Costo { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una cantidad ")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int? Cantidad { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }


    }
}
