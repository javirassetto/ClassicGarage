using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public partial class Tipo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un tipo ")]
        [Display(Name = "Descripcion Tipo")]
        [StringLength(50)]
        public string Descripcion { get; set; } = null!;


        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]

       [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        public DateTime? FechaRegistro { get; set; }
    }
}
