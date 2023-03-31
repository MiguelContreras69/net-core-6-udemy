using System.ComponentModel.DataAnnotations;
namespace Rocosa.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id  { get; set; }
        [Required(ErrorMessage ="El nombre de tipo de aplicacion es obligatorio")]
        public string  Nombre { get; set; }
    }
}
