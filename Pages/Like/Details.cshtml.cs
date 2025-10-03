using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Like
{
    public class DetailsModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public DetailsModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        public Guariba.Models.Like Like { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like.FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }
            else
            {
                Like = like;
            }
            return Page();
        }
    }
}
