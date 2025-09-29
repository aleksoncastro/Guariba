using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public DetailsModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca o usuário no banco de dados, incluindo seus interesses
            User = await _context.User
                                 .Include(u => u.UserInterests)
                                 .FirstOrDefaultAsync(m => m.Id == id);

            // Se nenhum usuário for encontrado com esse ID, User será nulo
            if (User == null)
            {
                return NotFound();
            }

            // Se o usuário foi encontrado, apenas exibe a página
            return Page();
        }
    }
}
