using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public partial class Vehiculo
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una imagen del vehiculo ")]
        [Display(Name = "Imagen")]
        public string? ImagenVehiculo { get; set; }
       
        [Display(Name = "Marca")]
        public int? MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        [Required(ErrorMessage = "Por favor, debe ingresar una marca ")]
        public virtual Marca? Marca { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una marca ")]
        [Display(Name = "Marca")]
        public int? ModeloId { get; set; }
        [ForeignKey("ModeloId")]
        [Required(ErrorMessage = "Por favor, debe ingresar un modelo ")]
        public virtual Modelo? Modelo { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar un tipo ")]
        [Display(Name = "Tipo")]
        public int? TipoId { get; set; }
        [ForeignKey("TipoId")]
        [Required(ErrorMessage = "Por favor, debe ingresar un tipo ")]
        public virtual Tipo? Tipo { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una categoria ")]
        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }


        [Required(ErrorMessage = "Por favor, debe ingresar un año valido (entre 2001-2030)")]
        [Display(Name = "Año")]
        [Range(2001, 2030, ErrorMessage = "El año debe estar entre 2001 y 2030.")]
        public int ano { get; set; }


        [Required(ErrorMessage = "Por favor, debe ingresar un precio ")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Display(Name = "Fecha Registro")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]       
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaRegistro { get; set; }

        [Required]
        [Display(Name = "Disponible")]
        public bool Disponible { get; set; } = true;

    }


    //public EstadoVehiculo Estado { get; set; }

    ////posibles estados
    //public enum EstadoVehiculo
    //{
    //    Nuevo,
    //    Usado
    //}

}
