using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guariba.Pages.Post
{

    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly Guariba.Data.SocialMediaContext _context;

        public CreateModel(Guariba.Data.SocialMediaContext context, UserManager<User> userManager)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.User, "Id", "Username");
            return Page();
        }

        [BindProperty]
        public Guariba.Models.Post Post { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);       
            if (user == null)
            {
                return Unauthorized(); // ou redirecione para login
            }

            Post.UserId = user.Id; // Associando o post ao usuário logado
            Post.CreatedAt = DateTime.UtcNow; // Definindo a data de criação como a data atual
            _context.Post.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("/feed");
        }
    }
}
