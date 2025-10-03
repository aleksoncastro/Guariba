using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Adicione para [Authorize]

namespace Guariba.Pages.Comment
{
    [Authorize] // Garante que apenas usuários logados possam acessar esta página
    public class CreateModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;
        private readonly UserManager<User> _userManager; // Para pegar o usuário logado

        public CreateModel(Guariba.Data.SocialMediaContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Guariba.Models.Comment Comment { get; set; } = default!;

        // Propriedade para receber o PostId da URL e guardá-lo no formulário
        [BindProperty(SupportsGet = true)]
        public int PostId { get; set; }

        public IActionResult OnGet(int postId)
        {
            // Apenas preparamos o PostId para ser usado no formulário
            PostId = postId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove validações automáticas desnecessárias
            ModelState.Remove("Comment.Post");
            ModelState.Remove("Comment.User");
            ModelState.Remove("Comment.ParentComment");
            ModelState.Remove("Comment.Replies");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Comment.CreatedAt = DateTime.UtcNow;
            Comment.UserId = user.Id;
            Comment.PostId = this.PostId;

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"Campo: {entry.Key}");
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($"Erro: {error.ErrorMessage}");
                        }
                    }
                }
                return Page();
            }

            _context.Comment.Add(Comment);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Post/Details", new { id = Comment.PostId });
        }
    }
}