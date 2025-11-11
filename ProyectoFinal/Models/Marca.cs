using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ProyectoFinal.Models.Modelo;

namespace ProyectoFinal.Models
{
    public partial class Marca
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una marca ")]
        [Display(Name = "Descripcion Marca")]
        [StringLength(50)]
        public string Descripcion { get; set; } = null!;
        //Una marca puede tener muchos modelos        
        public virtual ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();

        [Required (ErrorMessage ="Por favor, debe ingresar una fecha")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }        

        

    }   
}

