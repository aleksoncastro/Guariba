using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guariba.Data;
using Guariba.Models;

namespace Guariba.Pages.Like
{
    public class EditModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public EditModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guariba.Models.Like Like { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like =  await _context.Like.FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }
            Like = like;
           ViewData["PostId"] = new SelectList(_context.Post, "Id", "Id");
           ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(Like.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LikeExists(int id)
        {
            return _context.Like.Any(e => e.Id == id);
        }
    }
}
