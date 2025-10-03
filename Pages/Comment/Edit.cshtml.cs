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

namespace Guariba.Pages.Comment
{
    public class EditModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public EditModel(Guariba.Data.SocialMediaContext context)
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

            var comment =  await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            Comment = comment;
           ViewData["ParentCommentId"] = new SelectList(_context.Comment, "Id", "Id");
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

            _context.Attach(Comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(Comment.Id))
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

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
