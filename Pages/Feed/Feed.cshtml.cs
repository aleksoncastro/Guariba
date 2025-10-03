using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guariba.Pages
{
    public class FeedModel : PageModel
    {
        private readonly SocialMediaContext _context;
        private readonly UserManager<User> _userManager;

        public FeedModel(SocialMediaContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Guariba.Models.                                                                                                                                                                                                                                                                                                                                                                                                                                                 Post> Posts { get; set; } = new List<Guariba.Models.Post>();
        // IDs de posts que o usuário atual já curtiu (usado na view)                                                                       
        public HashSet<int> LikedPostIds { get; set; } = new HashSet<int>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user?.Id ?? 0;

            Posts = await _context.Post
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            foreach (var p in Posts)
            {
                p.LikesCount = p.Likes?.Count ?? 0;
                p.CommentsCount = p.Comments?.Count ?? 0;
            }

            if (currentUserId != 0)
            {
                var liked = await _context.Like
                    .Where(l => l.UserId == currentUserId)
                    .Select(l => l.PostId)
                    .ToListAsync();

                LikedPostIds = new HashSet<int>(liked);
            }
        }

        // Handler para curtir/descurtir via fetch
        public async Task<IActionResult> OnPostToggleLikeAsync(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new JsonResult(new { success = false, error = "Unauthorized" }) { StatusCode = 401 };
            }

            var existing = await _context.Like
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == user.Id);

            bool isNowLiked;
            if (existing != null)
            {
                _context.Like.Remove(existing);
                isNowLiked = false;
            }
            else
            {
                _context.Like.Add(new Guariba.Models.Like
                {
                    PostId = postId,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                });
                isNowLiked = true;
            }

            await _context.SaveChangesAsync();

            // Uso do CountAsync aqui é intencional — garante o valor correto direto do banco.
            var likesCount = await _context.Like.CountAsync(l => l.PostId == postId);

            return new JsonResult(new { success = true, likes = likesCount, liked = isNowLiked });
        }
    }
}
