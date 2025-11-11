using ProyectoFinal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ProyectoFinal.ViewModels
{
    public class VehiculoViewModels
    {
       
        public int Id { get; set; }

        
        [Display(Name = "Imagen")]
        public string? ImagenVehiculo { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una imagen del vehiculo")]
        [Display(Name = "Imagen Vehiculo")]
        public IFormFile? Imagen { get; set; }
        [Required(ErrorMessage = "Por favor, debe ingresar una marca")]
        [Display(Name = "Marca")]
        public int? MarcaId { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un modelo")]
        [Display(Name = "Modelo")]
        public int? ModeloId { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar un tipo")]
        [Display(Name = "Tipo")]
        public int? TipoId { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una categoria")]
        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }
     


        [Required(ErrorMessage = "Por favor, debe ingresar un año ")]
        [Display(Name = "Año")]
        [Range(2001, 2030, ErrorMessage = "El año debe estar entre 2001 y 2030")]
        public int ano { get; set; }


        [Required(ErrorMessage = "Por favor, debe ingresar un precio ")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //[Column(TypeName = "decimal(18, 2)")]
        public string? Precio { get; set; }

        [Required(ErrorMessage = "Por favor, debe ingresar una fecha ")]
        [Display(Name = "Fecha Registro")]
        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaRegistro { get; set; }

    }
}
