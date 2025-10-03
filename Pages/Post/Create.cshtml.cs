using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting; // 1. Adicionar using para IWebHostEnvironment
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO; // 2. Adicionar using para trabalhar com arquivos
using System.Threading.Tasks;

namespace Guariba.Pages.Post
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly Guariba.Data.SocialMediaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // 3. Adicionar campo para o ambiente


        public CreateModel(
            Guariba.Data.SocialMediaContext context,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment) // 4. Injetar no construtor
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment; // 5. Atribuir
        }

        // Remove essa linha, não está sendo usada no seu formulário.
        // public IActionResult OnGet() { ... }

        [BindProperty]
        public Guariba.Models.Post Post { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; } // 6. Propriedade para receber o arquivo de imagem

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. PROCESSAR A IMAGEM PRIMEIRO
            // Assim, a propriedade Post.ImageUrl será preenchida antes da validação.
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Define o caminho onde as imagens dos posts serão salvas
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts");
                // Garante que o diretório exista
                Directory.CreateDirectory(uploadsFolder);

                // Cria um nome de arquivo único para evitar conflitos
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Salva o arquivo no disco
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Salva apenas o caminho relativo no banco de dados
                Post.ImageUrl = "/uploads/posts/" + uniqueFileName;
            }

            // 2. AGORA, VALIDAR O MODELO
            // O método Validate() da sua classe Post será chamado aqui e verá o ImageUrl preenchido.
            if (!ModelState.IsValid)
            {
                // ---Seu bloco para depuração ---
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
                // --
                return Page();
            }

            // 3. O RESTO DA LÓGICA
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/login");
            }

            Post.UserId = user.Id;
            Post.CreatedAt = DateTime.UtcNow;

            _context.Post.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Feed/Feed");
        }
    }
}