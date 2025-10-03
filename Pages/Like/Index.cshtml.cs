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
    public class IndexModel : PageModel
    {
        private readonly Guariba.Data.SocialMediaContext _context;

        public IndexModel(Guariba.Data.SocialMediaContext context)
        {
            _context = context;
        }

        public IList<Guariba.Models.Like> Like { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Like = await _context.Like
                .Include(l => l.Post)
                .Include(l => l.User).ToListAsync();
        }
    }
}
