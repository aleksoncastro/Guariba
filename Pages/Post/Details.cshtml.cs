using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Guariba.Pages.Post
{
    public class DetailsModel : PageModel
    {
        private readonly SocialMediaContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(SocialMediaContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Guariba.Models.Post Post { get; set; }

        [BindProperty]
        public string NewCommentText { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Post
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/login", new { area = "Identity" });

            if (string.IsNullOrWhiteSpace(NewCommentText))
            {
                ModelState.AddModelError("NewCommentText", "O comentário não pode estar vazio.");
                return await OnGetAsync(id);
            }

            var comment = new Guariba.Models.Comment
            {
                Text = NewCommentText,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                PostId = id
            };

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id }); // recarrega a página do post
        }
    }
}
