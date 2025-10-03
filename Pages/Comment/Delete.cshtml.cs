using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Comment
{
    public class DeleteModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public DeleteModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guariba.Models.Comment Comment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }
            else
            {
                Comment = comment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                Comment = comment;
                _context.Comment.Remove(Comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
