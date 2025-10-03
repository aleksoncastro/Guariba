using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Like
{
    public class CreateModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public CreateModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Guariba.Models.Like Like { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Like.Add(Like);
            await _context.SaveChangesAsync();

            return RedirectToPage("/feed");
        }
    }
}
