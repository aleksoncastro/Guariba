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
        public new User User { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()  {
            // Removemos a validação dos campos que vamos definir manualmente
            ModelState.Remove("User.ProfilePhoto");
            ModelState.Remove("User.RegistrationDat");
            ModelState.Remove("User.PersonalInformation");

            if (!ModelState.IsValid)
            {
                // --- Adicione este bloco para depuração ---
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
                // ------------------------------------------

                // Se ainda houver erros (ex: Username vazio), retorna a página
                return Page();
            }

            // ---- DEFININDO VALORES PADRÃO ----
            User.ProfilePhoto = "default-avatar.png"; // Um valor padrão para a foto
            User.RegistrationDat = DateTime.Now; // A data de registro automática
            User.PersonalInformation = new PersonalInformation
            {
                FullName = "Nome a ser preenchido" // Valor padrão
            };
            // ------------------------------------

            _context.User.Add(User);
            await _context.SaveChangesAsync(); // Apenas um SaveChanges é necessário agora

            return RedirectToPage("./Index");
        }
    }
}