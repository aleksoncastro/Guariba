using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations; // Usado para [Required] e ValidationResult

namespace Guariba.Models
{
    // 1. Implemente a interface IValidatableObject
    public class Post : IValidatableObject
    {
        public int Id { get; set; }

        // Ambos são opcionais individualmente...
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }

        // --- Relacionamentos ---
        [Required] // ...mas o UserId ainda é obrigatório
        public int UserId { get; set; }
        [ValidateNever] // 👈 evita a validação desnecessária
        public User User { get; set; }

        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

        // --- Contagens ---
        public int CommentsCount { get; set; } = 0;
        public int LikesCount { get; set; } = 0;
        public int RetweetsCount { get; set; } = 0;
        public int SharesCount { get; set; } = 0;

        // 2. Adicione este método de validação
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // A REGRA: Se o TextContent ESTÁ VAZIO E a ImageUrl TAMBÉM ESTÁ VAZIA...
            if (string.IsNullOrWhiteSpace(TextContent) && string.IsNullOrWhiteSpace(ImageUrl))
            {
                // ...então a validação falha.
                yield return new ValidationResult(
                    "O post não pode estar vazio. Adicione um texto ou uma imagem.",
                    // Opcional: associa o erro a ambos os campos para que apareçam em ambos os lugares
                    new[] { nameof(TextContent), nameof(ImageUrl) }
                );
            }
        }
    }
}