using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Categoria
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un categoria ")]
        [Display(Name = "Descripcion Categoria")]
        [StringLength(50)]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }
    }
}
