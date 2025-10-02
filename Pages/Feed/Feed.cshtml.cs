using Guariba.Data;
using Guariba.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guariba.Pages
{
    public class FeedModel : PageModel
    {
        private readonly SocialMediaContext _context;

        public FeedModel(SocialMediaContext context)
        {
            _context = context;
        }

        // Lista de posts para exibir no feed
        public List<Guariba.Models.Post> Posts { get; set; }

        public async Task OnGetAsync()
        {
            // Carrega posts com os usuários
            Posts = await _context.Post
                .Include(p => p.User) // pega info do autor
                .OrderByDescending(p => p.CreatedAt) // mais recentes primeiro
                .ToListAsync();
        }
    }
}
