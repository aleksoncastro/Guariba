using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public CreateModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        // Propriedade APENAS para receber os IDs dos interesses selecionados no formulário
        [BindProperty]
        public List<int> SelectedInterests { get; set; }

        // Propriedade APENAS para exibir as opções de checkbox na tela
        public List<Interest> AllInterests { get; set; }

        public IActionResult OnGet()
        {
            AllInterests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();

            return Page();
        }


       


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AllInterests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();
                return Page();
            }

            // Crie o usuário primeiro (sem os interesses) e salve para obter um ID
            // Se for uma página de Edição, você já terá o usuário carregado
            _context.User.Add(User);
            await _context.SaveChangesAsync(); // Agora o User.Id existe!

            // Agora, adicione os interesses selecionados
            if (SelectedInterests != null)
            {
                foreach (var interestId in SelectedInterests)
                {
                    var userInterest = new UserInterest
                    {
                        UserId = User.Id,
                        Interest = (Interest)interestId // Converte o ID de volta para o enum
                    };
                    _context.UserInterest.Add(userInterest);
                }
                await _context.SaveChangesAsync(); // Salva as relações de interesse
            }

            return RedirectToPage("./Index");
        }
    }
}
