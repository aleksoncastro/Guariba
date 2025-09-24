using System.ComponentModel.DataAnnotations;

namespace Guariba.Models
{
    public enum Interest
    {
        [Display(Name = "Tecnologia")]
        TECNOLOGIA = 1,

        [Display(Name = "Esportes")]
        ESPORTES = 2,

        [Display(Name = "Jogos")]
        JOGOS = 3,

        [Display(Name = "Moda")]
        MODA = 4,

        [Display(Name = "Política")]
        POLITICA = 5,

        [Display(Name = "Música")]
        MUSICA = 6,

        [Display(Name = "Filmes")]
        FILMES = 7
    }
}
