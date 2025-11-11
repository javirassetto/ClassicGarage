using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ProyectoFinal.Models
{
    public partial class Modelo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un modelo ")]
        [Display(Name = "Descripcion Modelo")]
        [StringLength(50)]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        // Un modelo pertenece a una marca
        [Required(ErrorMessage = "Por favor, debe seleccionar un marca ")]
        [Display(Name = "Marca")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public virtual Marca? Marca { get; set; }

    }
}
