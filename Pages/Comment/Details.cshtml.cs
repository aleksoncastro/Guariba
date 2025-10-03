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
    public class DetailsModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public DetailsModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

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
    }
}
