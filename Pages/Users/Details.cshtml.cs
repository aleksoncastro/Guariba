using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Identity;      // 1. Adicionado para o UserManager
using Microsoft.AspNetCore.Authorization;  // 2. Adicionado para o [Authorize]

namespace Guariba.Pages.Users
{
    // 3. Adiciona o atributo para exigir que o usuário esteja logado
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;
        private readonly UserManager<User> _userManager; // 4. Declara o UserManager

        // 5. Injeta o UserManager no construtor
        public DetailsModel(Guariba.Data.SocialMediaContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Renomeado de 'User' para 'UserProfile' para evitar conflito com a propriedade 'User' da PageModel
        public User UserProfile { get; set; } = default!;

        // 6. Altera o OnGetAsync para não receber um ID pela URL
        public async Task<IActionResult> OnGetAsync()
        {
            // Pega o usuário logado atualmente a partir do cookie/sessão
            // A propriedade 'User' aqui é o ClaimsPrincipal da página
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // O atributo [Authorize] já deve ter redirecionado para o login,
                // mas esta é uma verificação de segurança adicional.
                return Challenge(); // Retorna 401 Unauthorized ou redireciona
            }

            // Agora, busca a versão completa do usuário do banco de dados com os dados relacionados.
            // É importante fazer isso para carregar as coleções como 'UserInterests'.
            UserProfile = await _context.User
                .Include(u => u.UserInterests) // Inclui os interesses na consulta
                                               // Se UserInterests tiver uma propriedade de navegação para a tabela Interest,
                                               // você pode usar .ThenInclude(ui => ui.Interest) para carregar os detalhes do interesse.
                .FirstOrDefaultAsync(m => m.Id == currentUser.Id);

            if (UserProfile == null)
            {
                // Caso raro: o usuário foi excluído do banco, mas a sessão ainda está ativa.
                return NotFound($"Usuário com ID {currentUser.Id} não encontrado no banco de dados.");
            }

            return Page();
        }
    }
}