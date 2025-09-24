using System.ComponentModel.DataAnnotations;

namespace Guariba.Models
{
    public enum Gender
    {
        [Display(Name = "Masculino")]
        MASCULINO = 1,

        [Display(Name = "Feminino")]
        FEMININO = 2,

        [Display(Name = "Outro")]
        OUTRO = 3,

        [Display(Name = "Prefiro não dizer")]
        PREFIRO_NAO_DIZER = 4
    }
}